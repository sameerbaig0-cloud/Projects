using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ServicePlatform.Services;
using Microsoft.AspNetCore.Authorization;

namespace ServicePlatform.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [CustomAuthorize]
    public class POS_PackingToRetailTransferController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public POS_PackingToRetailTransferController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		//// GET: POS_PackingToRetailTransfer
		//[HttpGet] // Specify that this action responds to HTTP GET requests
		//public async Task<IActionResult> Index()
		//{
		//    //var applicationDbContext = _context.POS_PackingToRetailTransfers.Include(p => p.ApplicationUser).Include(p => p.POS_RetailStore);
		//    //return View(await applicationDbContext.ToListAsync());

		//    var emptyList = new List<POS_PackingToRetailTransfer>();

		//    return View(emptyList);


		//}


		// GET: POS_PackingToRetailTransfer/Index/DateTime
		[HttpGet]
		public async Task<IActionResult> Index(DateTime? dateTime, string? sortOrder, string? currentFilter, string? searchString, int? pageNumber, int? pageSize)
		{
			var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			var user = await _userManager.FindByEmailAsync(userEmail);

			if (user == null)
			{
				return NotFound();
			}

			var applicationDbContext = _context.POS_PackingToRetailTransfers.Include(p => p.ApplicationUser).Include(p => p.POS_RetailStore).AsQueryable();


			ViewData["DateTime"] = dateTime;
			ViewData["CurrentSort"] = sortOrder;
			ViewData["NameSortParm"] = System.String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
			ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;

			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewData["CurrentFilter"] = searchString;


			if (dateTime.HasValue)
			{
				applicationDbContext = applicationDbContext.Where(x => x.TransferDate == dateTime.Value.Date);
				ViewBag.DateTime = dateTime.Value.ToString("yyyy-MM-dd");
				//var applicationDbContext = _context.POS_PackingToRetailTransfers.Where(x => x.TransferDate == dateTime.Value.Date).Include(p => p.ApplicationUser).Include(p => p.POS_RetailStore);
				//            return View(await applicationDbContext.ToListAsync());
			}
			else
			{
				applicationDbContext = applicationDbContext.Where(x => x.TransferDate == DateTime.Now.Date);
				ViewBag.DateTime = DateTime.Now.ToString("yyyy-MM-dd");
			}

			if (!string.IsNullOrEmpty(currentFilter))
			{
				applicationDbContext = applicationDbContext.Where(x => x.POS_RetailStore.StoreName.Contains(currentFilter)
															|| x.OrderNo.Contains(currentFilter)
															|| x.Article.Contains(currentFilter)
															|| x.TrackingID.Equals(currentFilter)
															|| x.ApplicationUser.UserName.Contains(currentFilter));
			}


			switch (sortOrder)
			{
				case "date_desc":
					applicationDbContext = applicationDbContext.OrderByDescending(x => x.TransferDate);
					break;
				default:
					applicationDbContext = applicationDbContext.OrderBy(x => x.TransferDate);
					break;
			}

			return View(await PaginatedList<POS_PackingToRetailTransfer>.CreateAsync(applicationDbContext.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10));


		}


		// GET: POS_PackingToRetailTransfer/PackingToRetailTransfer
		[HttpGet]
		public async Task<IActionResult> CreateTransferViaScan()
		{

			var model = new POS_PackingToRetailStoreTransfer
			{
				currentDate = DateTime.Now,
				RetailStores = _context.POS_RetailStores.ToList()
			};

			return View(model);
		}


		// POST: POS_PackingToRetailTransfer/PackingToRetailTransfer
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateTransferViaScan(POS_PackingToRetailStoreTransfer model)
		{
			if (ModelState.IsValid)
			{

				// Inside a controller action or a view
				var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
				//HttpContext.Response.Redirect("/AccessDenied");

				// Retrieve the currently authenticated user
				var user = await _userManager.FindByEmailAsync(useremail);

				if (user == null)
				{   // Handle the case where the user is not found
					return NotFound();
				}

				var outFlag = new SqlParameter
				{
					ParameterName = "@OutFlag",
					SqlDbType = System.Data.SqlDbType.Bit,
					Direction = System.Data.ParameterDirection.Output
				};

				var outRemarks = new SqlParameter
				{
					ParameterName = "@OutRemarks",
					SqlDbType = System.Data.SqlDbType.NVarChar,
					Size = 255,
					Direction = System.Data.ParameterDirection.Output
				};

				var transferDate = new SqlParameter
				{
					ParameterName = "@TransferDate",
					SqlDbType = System.Data.SqlDbType.SmallDateTime,
					Value = model.currentDate
				};

				var retailStoreID = new SqlParameter
				{
					ParameterName = "@RetailStoreID",
					SqlDbType = System.Data.SqlDbType.BigInt,
					Value = model.POSRetailStoreId
				};

				var userID = new SqlParameter
				{
					ParameterName = "@UserID",
					SqlDbType = System.Data.SqlDbType.NVarChar,
					Value = user.Id
				};

				var qrCodeParam = new SqlParameter
				{
					ParameterName = "@QRCodeData",
					SqlDbType = System.Data.SqlDbType.NVarChar,
					Value = model.qrCodeData
				};

				try
				{
					await _context.Database.ExecuteSqlRawAsync(
						"EXEC dbo.POS_PackingToRetailTransferSP @OutFlag OUTPUT, @OutRemarks OUTPUT, @TransferDate, @RetailStoreID, @UserID, @QRCodeData",
						outFlag, outRemarks, transferDate, retailStoreID, userID, qrCodeParam);

					ViewBag.OutFlag = (bool)outFlag.Value;
					ViewBag.OutRemarks = outRemarks.Value.ToString();
				}
				catch (Exception ex)
				{
					ViewBag.OutFlag = false;
					ViewBag.OutRemarks = ex.Message;
				}
			}
			else
			{
				ViewBag.OutFlag = false;
				ViewBag.OutRemarks = "Model validation failed.";
			}

			model.RetailStores = _context.POS_RetailStores.ToList(); // Repopulate the retail stores list
																	 // Clear the qrCodeData input after successful submission
			model.qrCodeData = string.Empty;
			return View(model);
		}




		// GET: POS_PackingToRetailTransfer/Details/5
		public async Task<IActionResult> Details(long? id)
		{
			if (id == null || _context.POS_PackingToRetailTransfers == null)
			{
				return NotFound();
			}

			var pOS_PackingToRetailTransfer = await _context.POS_PackingToRetailTransfers
				.Include(p => p.ApplicationUser)
				.Include(p => p.POS_RetailStore)
				.FirstOrDefaultAsync(m => m.TransferID == id);
			if (pOS_PackingToRetailTransfer == null)
			{
				return NotFound();
			}

			return View(pOS_PackingToRetailTransfer);
		}



		// GET: POS_PackingToRetailTransfer/Create
		public IActionResult Create()
		{
			//ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
			ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreName");
			return View();
		}

		// POST: POS_PackingToRetailTransfer/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("TransferID,TransferDate,OrderNo,Article,Size,TrackingID,Quantity,StoreID,UserID,CreatedAt")] POS_PackingToRetailTransfer pOS_PackingToRetailTransfer)
		{
			// Inside a controller action or a view
			var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			//HttpContext.Response.Redirect("/AccessDenied");

			// Retrieve the currently authenticated user
			var user = await _userManager.FindByEmailAsync(useremail);

			if (user == null)
			{   // Handle the case where the user is not found
				return NotFound();
			}

			pOS_PackingToRetailTransfer.UserID = user.Id;
			pOS_PackingToRetailTransfer.CreatedAt = DateTime.Now;

			if (ModelState.IsValid)
			{
				_context.Add(pOS_PackingToRetailTransfer);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			//ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", pOS_PackingToRetailTransfer.UserID);
			ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreName", pOS_PackingToRetailTransfer.StoreID);
			return View(pOS_PackingToRetailTransfer);
		}

		// GET: POS_PackingToRetailTransfer/Edit/5
		public async Task<IActionResult> Edit(long? id)
		{
			if (id == null || _context.POS_PackingToRetailTransfers == null)
			{
				return NotFound();
			}

			var pOS_PackingToRetailTransfer = await _context.POS_PackingToRetailTransfers.FindAsync(id);
			if (pOS_PackingToRetailTransfer == null)
			{
				return NotFound();
			}
			ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", pOS_PackingToRetailTransfer.UserID);
			ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreName", pOS_PackingToRetailTransfer.StoreID);
			return View(pOS_PackingToRetailTransfer);
		}

		// POST: POS_PackingToRetailTransfer/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(long id, [Bind("TransferID,TransferDate,OrderNo,Article,Size,TrackingID,Quantity,StoreID,UserID,CreatedAt")] POS_PackingToRetailTransfer pOS_PackingToRetailTransfer)
		{
			if (id != pOS_PackingToRetailTransfer.TransferID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(pOS_PackingToRetailTransfer);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!POS_PackingToRetailTransferExists(pOS_PackingToRetailTransfer.TransferID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", pOS_PackingToRetailTransfer.UserID);
			ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreName", pOS_PackingToRetailTransfer.StoreID);
			return View(pOS_PackingToRetailTransfer);
		}

		// GET: POS_PackingToRetailTransfer/Delete/5
		public async Task<IActionResult> Delete(long? id)
		{
			if (id == null || _context.POS_PackingToRetailTransfers == null)
			{
				return NotFound();
			}

			var pOS_PackingToRetailTransfer = await _context.POS_PackingToRetailTransfers
				.Include(p => p.ApplicationUser)
				.Include(p => p.POS_RetailStore)
				.FirstOrDefaultAsync(m => m.TransferID == id);
			if (pOS_PackingToRetailTransfer == null)
			{
				return NotFound();
			}

			return View(pOS_PackingToRetailTransfer);
		}

		// POST: POS_PackingToRetailTransfer/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
			if (_context.POS_PackingToRetailTransfers == null)
			{
				return Problem("Entity set 'ApplicationDbContext.POS_PackingToRetailTransfers'  is null.");
			}
			var pOS_PackingToRetailTransfer = await _context.POS_PackingToRetailTransfers.FindAsync(id);
			if (pOS_PackingToRetailTransfer != null)
			{
				_context.POS_PackingToRetailTransfers.Remove(pOS_PackingToRetailTransfer);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool POS_PackingToRetailTransferExists(long id)
		{
			return (_context.POS_PackingToRetailTransfers?.Any(e => e.TransferID == id)).GetValueOrDefault();
		}
	}
}
