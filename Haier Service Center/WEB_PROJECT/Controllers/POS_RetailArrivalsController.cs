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

    public class POS_RetailArrivalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public POS_RetailArrivalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: POS_RetailArrivals
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.POS_RetailArrivals.Include(p => p.ApplicationUser).Include(p => p.POS_RetailStore);
            //return View(await applicationDbContext.ToListAsync());
            return View();
        }

        // GET: POS_RetailArrivals/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            //if (id == null || _context.POS_RetailArrivals == null)
            //{
            //    return NotFound();
            //}

            //var pOS_RetailArrivals = await _context.POS_RetailArrivals
            //    .Include(p => p.ApplicationUser)
            //    .Include(p => p.POS_RetailStore)
            //    .FirstOrDefaultAsync(m => m.ArrivalID == id);
            //if (pOS_RetailArrivals == null)
            //{
            //    return NotFound();
            //}

            //return View(pOS_RetailArrivals);
            return View();

        }

        // GET: POS_RetailArrivals/Create
        public IActionResult Create()
        {
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreName");
            return View();
        }

        // POST: POS_RetailArrivals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArrivalID,ArrivalDate,OrderNo,Article,Size,TrackingID,Quantity,StoreID,UserID,CreatedAt")] POS_RetailArrivals pOS_RetailArrivals)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pOS_RetailArrivals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", pOS_RetailArrivals.UserID);
            ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreName", pOS_RetailArrivals.StoreID);
            return View(pOS_RetailArrivals);
        }



        // GET: POS_PackingToRetailTransfer/PackingToRetailTransfer
        [HttpGet]
        public async Task<IActionResult> ArrivalViaScan()
        {

            var model = new POS_RetailStoreArrivals
            {
                currentDate = DateTime.Now,
                RetailStores = _context.POS_RetailStores.ToList()
            };

            return View(model);
        }


        // POST: POS_PackingToRetailTransfer/PackingToRetailTransfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArrivalViaScan(POS_RetailStoreArrivals model)
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
                        "EXEC dbo.POS_RetailArrivalsSP @OutFlag OUTPUT, @OutRemarks OUTPUT, @TransferDate, @RetailStoreID, @UserID, @QRCodeData",
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





        // GET: POS_RetailArrivals/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            //if (id == null || _context.POS_RetailArrivals == null)
            //{
            //    return NotFound();
            //}

            //var pOS_RetailArrivals = await _context.POS_RetailArrivals.FindAsync(id);
            //if (pOS_RetailArrivals == null)
            //{
            //    return NotFound();
            //}
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", pOS_RetailArrivals.UserID);
            //ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreID", pOS_RetailArrivals.StoreID);
            //return View(pOS_RetailArrivals);

            return View();
        }

        // POST: POS_RetailArrivals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ArrivalID,ArrivalDate,OrderNo,Article,Size,TrackingID,Quantity,StoreID,UserID,CreatedAt")] POS_RetailArrivals pOS_RetailArrivals)
        {
            if (id != pOS_RetailArrivals.ArrivalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pOS_RetailArrivals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!POS_RetailArrivalsExists(pOS_RetailArrivals.ArrivalID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", pOS_RetailArrivals.UserID);
            ViewData["StoreID"] = new SelectList(_context.POS_RetailStores, "StoreID", "StoreID", pOS_RetailArrivals.StoreID);
            return View(pOS_RetailArrivals);
        }

        // GET: POS_RetailArrivals/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            //if (id == null || _context.POS_RetailArrivals == null)
            //{
            //    return NotFound();
            //}

            //var pOS_RetailArrivals = await _context.POS_RetailArrivals
            //    .Include(p => p.ApplicationUser)
            //    .Include(p => p.POS_RetailStore)
            //    .FirstOrDefaultAsync(m => m.ArrivalID == id);
            //if (pOS_RetailArrivals == null)
            //{
            //    return NotFound();
            //}

            //return View(pOS_RetailArrivals);

            return View();
        }

        // POST: POS_RetailArrivals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            //if (_context.POS_RetailArrivals == null)
            //{
            //    return Problem("Entity set 'ApplicationDbContext.POS_RetailArrivals' is null.");
            //}
            //var pOS_RetailArrivals = await _context.POS_RetailArrivals.FindAsync(id);
            //if (pOS_RetailArrivals != null)
            //{
            //    _context.POS_RetailArrivals.Remove(pOS_RetailArrivals);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            return View();
        }

        private bool POS_RetailArrivalsExists(long id)
        {
            //return (_context.POS_RetailArrivals?.Any(e => e.ArrivalID == id)).GetValueOrDefault();
            return true;
        }
    }
}
