using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.IdentityModel.Tokens;
using NuGet.ContentModel;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using System.Drawing.Printing;
using ServicePlatform.Services;
using NuGet.Versioning;
using System.IO;
using System.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ServicePlatform.Controllers
{
	[Authorize]
	[AutoValidateAntiforgeryToken]
	[CustomAuthorize]
	public class IndentMastersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly BlobService _blobService;
		private readonly string _azureBlobContainer;


		public IndentMastersController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, BlobService blobService)
		{
			_context = context;
			_configuration = configuration;
			_userManager = userManager;
			_blobService = blobService;
			_azureBlobContainer = _configuration.GetValue<string>("baershoespurchasequotestorein");
		}


		//// GET: IndentMasters
		//public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		//{
		//	// Inside a controller action or a view
		//	var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
		//	//HttpContext.Response.Redirect("/AccessDenied");

		//	// Retrieve the currently authenticated user
		//	var user = await _userManager.FindByEmailAsync(useremail);

		//	if (user == null)
		//	{   // Handle the case where the user is not found
		//		return NotFound();
		//	}

		//	var applicationDbContext = _context.IndentMasters
		//		.Where(x => x.IsCancel == false && x.IsPoGen == false && x.IsApproved == false)
		//		.Include(a => a.ItemTypeMaster)//.ThenInclude(x => x.TaxMaster)
		//		.Include(b => b.ColorMaster)
		//		.Include(c => c.SupplierMaster)
		//		.Include(d => d.SizeMaster)
		//		.Include(e => e.SupplierPaymentTerm)
		//		//.Include(f => f.IndentMasterFiles)
		//		.Include(g => g.ApplicationUser).Where(x => x.Loguser == user.AccountID).AsQueryable();

		//	//.Include(d => d.SizeMaster).Take(2).OrderByDescending(x => x.Sno).AsQueryable();

		//	//.Include(e => e.UnitMaster).AsQueryable();

		//	//var applicationDbContext2 = _context.IndentMasters
		//	//    .Where(x => x.IsCancel == false && x.IsPoGen == false && x.IsApproved == false)
		//	//    .Include(IndentMaster => IndentMaster.ItemTypeMaster)
		//	//    .Include(b => b.ColorMaster)
		//	//    .Include(c => c.SupplierMaster)
		//	//    .Include(d => d.SizeMaster)
		//	//    .Include(e => e.UnitMaster)
		//	//    .Select(IndentMaster => new
		//	//    {
		//	//        IndentMaster.IndentDate,
		//	//        IndentMaster.IndentType,
		//	//        IndentMaster.Refno,
		//	//        IndentMaster.Size,
		//	//        IndentMaster.IndentQty,
		//	//        IndentMaster.Remarks,
		//	//        IndentMaster.Price,
		//	//        IndentMaster.Logtime,

		//	//        ItemTypeMaster = IndentMaster

		//	//    });



		//	//var applicationDbContext = (from Qindentmasters in _context.IndentMasters
		//	//                            join Qitemtypemasters in _context.ItemTypeMasters on Qindentmasters.ItemType equals Qitemtypemasters.Code
		//	//                            join Qcolormasters in _context.ColorMasters on Qindentmasters.ItemColor equals Qcolormasters.SCode
		//	//                            join Qsuppliermasters in _context.SupplierMasters on Qindentmasters.SupplierID equals Qsuppliermasters.Code
		//	//                            join Qsizemasters in _context.SizeMasters on Qindentmasters.Size equals Qsizemasters.Name
		//	//                            join Qunitmasters in _context.UnitMasters on Qindentmasters.UOM equals Qunitmasters.Name
		//	//                            where Qindentmasters.IsCancel == false && Qindentmasters.IsPoGen == false && Qindentmasters.IsApproved == false
		//	//                            select new
		//	//                            {
		//	//                                Qindentmasters.IndentDate,
		//	//                                Qindentmasters.IndentType,
		//	//                                Qindentmasters.Refno,
		//	//                                Qindentmasters.Size,
		//	//                                Qindentmasters.UOM,
		//	//                                Qindentmasters.IndentQty,
		//	//                                Qindentmasters.Remarks,
		//	//                                Qindentmasters.Price,
		//	//                                Qindentmasters.Logtime,
		//	//                                Qitemtypemasters.ItemCode,
		//	//                                Qcolormasters.Code,
		//	//                                Qindentmasters.Loguser,
		//	//                                Name =  Qsuppliermasters.Name
		//	//                            });


		//	ViewData["CurrentSort"] = sortOrder;
		//	ViewData["NameSortParm"] = System.String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
		//	ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
		//	//ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;

		//	if (searchString != null)
		//	{
		//		pageNumber = 1;
		//	}
		//	else
		//	{
		//		searchString = currentFilter;
		//	}

		//	ViewData["CurrentFilter"] = searchString;

		//	if (!System.String.IsNullOrEmpty(searchString))
		//	{
		//		applicationDbContext = applicationDbContext.Where(IndentMaster => IndentMaster.ItemTypeMaster.ItemCode.Contains(searchString)
		//													|| IndentMaster.ColorMaster.Code.Contains(searchString)
		//													|| IndentMaster.SupplierMaster.Name.Contains(searchString)
		//													|| IndentMaster.Remarks.Contains(searchString));
		//	}

		//	switch (sortOrder)
		//	{
		//		case "name_desc":
		//			applicationDbContext = applicationDbContext.OrderByDescending(IndentMaster => IndentMaster.ItemTypeMaster.ItemCode);
		//			break;
		//		case "Date":
		//			applicationDbContext = applicationDbContext.OrderBy(IndentMaster => IndentMaster.IndentDate);
		//			break;
		//		case "date_desc":
		//			applicationDbContext = applicationDbContext.OrderByDescending(IndentMaster => IndentMaster.IndentDate);
		//			break;
		//		default:  // Name ascending 
		//			applicationDbContext = applicationDbContext.OrderByDescending(IndentMaster => IndentMaster.Sno);
		//			break;
		//	}

		//	if (pageSize == 0 || pageSize == null)
		//	{
		//		pageSize = _configuration.GetValue("PageSize", 10); ;
		//		ViewData["PageSize"] = pageSize;
		//	}
		//	else
		//	{
		//		ViewData["PageSize"] = ViewBag.PageSize;
		//		pageSize = ViewBag.PageSize;
		//	}

		//	//return View(await applicationDbContext.ToListAsync());

		//	return View(await PaginatedList<IndentMaster>.CreateAsync(applicationDbContext.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10));
		//}


		//// GET: IndentMasters/Details/5
		//public async Task<IActionResult> Details(long? id)
		//{
		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return NotFound();
		//	}

		//	var indentMaster = await _context.IndentMasters
		//		.Include(i => i.IndentMasterFiles)
		//		.Include(i => i.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//		.Include(i => i.ColorMaster)
		//		.Include(i => i.SupplierMaster)
		//		.Include(i => i.SizeMaster)
		//		.Include(i => i.UnitMaster)
		//		.Include(i => i.SupplierPaymentTerm)
		//		.Include(i => i.ApplicationUser)
		//		.FirstOrDefaultAsync(m => m.Sno == id);


		//	if (indentMaster == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(indentMaster);
		//}


		//// GET: IndentMasters/Create
		//public IActionResult Create()
		//{



		//	//string sqlstr = $@"SELECT Top 1 It.SNo, It.Code, CONCAT(TRIM(It.ItemCode),'-',It.ItemName) As [ItemCode], It.ItemName, It.GerName, It.PurName, It.Catagory, It.HSNNo, It.Unit, It.Rate, It.IndianCustomsCode, It.GermanCustomsCode, It.BOMExtraAllowance, It.Picture, Im.SupplierID, sm.Name From PriceMaster a " +
		//	//										"INNER JOIN " +
		//	//										"ItemTypeMaster it on it.code=a.itemtype " +
		//	//										"INNER JOIN " +
		//	//										"colormaster co on co.Scode=a.itemcolor " +
		//	//										"INNER JOIN " +
		//	//										"CatagorySubMaster b on b.Code=it.Catagory " +
		//	//										"INNER JOIN " +
		//	//										"CatagoryMaster c on c.Code=b.MainCatName " +
		//	//										"LEFT OUTER JOIN " +
		//	//										"Indentmaster im on im.itemtype=it.code " +
		//	//										"LEFT OUTER JOIN " +
		//	//										"SupplierMaster sm on sm.Code= im.SupplierID " +
		//	//										"WHERE C.Name IN ('Cutting Die', 'General', 'Machinery Spares', 'Shoe Last', 'Fixed Assests') " +
		//	//										"";

		//	//var indentMaster = _context.IndentMasters.FirstOrDefault();



		//	//List<string> indentTypes = new List<string> { "General", "HR", "IT", "Maintenance" };
		//	//ViewData["IndentType"] = new SelectList(indentTypes);

		//	ViewData["IndentType"] = new SelectList(_context.IndentMasterMaterialDisplays.Where(x => x.ProcurementOf == "InDirect" && x.Flag == true), "ID", "Department");


		//	ViewData["ItemColor"] = new SelectList(_context.ColorMasters, "SCode", "Code");
		//	////ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters, "Code", "ItemCode");
		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters.FromSqlRaw(sqlstr), "Code", "ItemCode", indentMaster.ItemType);

		//	////ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters.FromSqlRaw(sqlstr), "Code", "ItemCode");
		//	ViewData["SupplierID"] = new SelectList(_context.SupplierMasters, "Code", "Name");
		//	ViewData["Size"] = new SelectList(_context.SizeMasters, "Name", "Name");
		//	ViewData["UOM"] = new SelectList(_context.UnitMasters, "Name", "Name");
		//	ViewData["SupplierPaymentTermID"] = new SelectList(_context.SupplierPaymentTerms, "ID", "PaymentTerms");
		//	return View();
		//}


		//// POST: IndentMasters/Create
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Sno,IndentDate,IndentType,Refno,StoreRefno,ItemType,ItemColor,UOM,Size,IndentQty,Remarks,Price,SupplierID,SupplierPaymentTermID,DeliveryPeriod,Store,IsCancel,IsPoGen,Loguser,Logtime,IsApproved,IsPassed,ApprovedBy,ApprovedTime,ApprovedRemarks,FinalRemarks,CancelledBy,Application")] IndentMaster indentMaster, List<IFormFile> files)
		//{

		//	int x = 0;  // Count the number of files file size with in allowed limit
		//				//[RequestSizeLimit(1000000)] // Limit to 1 MB
		//	foreach (var file in files.ToList())
		//	{
		//		if (file != null && file.Length > 1000000) // Limit to 1 MB
		//		{
		//			ModelState.AddModelError(string.Empty, "Only maximum of 1MB file size is allowed");
		//			x++;
		//		}
		//	}

		//	if (x == 0)
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			// Inside a controller action or a view
		//			var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

		//			// Retrieve the currently authenticated user
		//			var user = await _userManager.FindByEmailAsync(useremail);


		//			if (user == null)
		//			{
		//				// Handle the case where the user is not found
		//				return NotFound();
		//			}

		//			indentMaster.StoreRefno = string.Empty;
		//			indentMaster.CurrentApprovalStage = 0;
		//			indentMaster.CancelledBy = string.Empty;
		//			indentMaster.IndentDate = DateTime.Now.Date;
		//			indentMaster.Logtime = DateTime.Now;
		//			indentMaster.Loguser = user.AccountID;

		//			using (var transaction = _context.Database.BeginTransaction())
		//			{
		//				try
		//				{

		//					_context.Add(indentMaster);
		//					await _context.SaveChangesAsync();
		//					await transaction.CommitAsync();


		//					//UPDATING THE REFERENCE NO COLUMN AFTER THE TRANSACTION IS COMPLETED
		//					foreach (var entry in _context.ChangeTracker.Entries<IndentMaster>().ToList())
		//					{
		//						// Retrieve the entity from the database
		//						var indentMasterToUpdate = await _context.IndentMasters
		//							.FirstOrDefaultAsync(im => im.Sno == entry.Entity.Sno);

		//						long _Sno = entry.Entity.Sno;

		//						if (indentMasterToUpdate != null)
		//						{
		//							// Update other properties as needed
		//							indentMasterToUpdate.Refno = entry.Entity.Sno.ToString();
		//						}
		//						await _context.SaveChangesAsync();



		//						foreach (var file in files.ToList())
		//						{
		//							string _guidFileName = string.Empty;

		//							//string fileExtension = Path.GetExtension(file.FileName);
		//							//string fileWOExtension = Path.GetFileNameWithoutExtension(file.FileName);

		//							//_guidFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

		//							// Save file to storage (e.g., Azure Blob Storage) and get the BlobFileName
		//							_guidFileName = await _blobService.UploadAsync(file, _azureBlobContainer);

		//							// Create IndentMasterFile record
		//							var indentMasterFile = new IndentMasterFile
		//							{
		//								IndentMasterID = indentMaster.Sno,
		//								FileName = file.FileName,
		//								BlobFileName = _guidFileName
		//							};

		//							_context.Add(indentMasterFile);
		//							await _context.SaveChangesAsync();
		//						}


		//					}

		//				}
		//				catch (Exception ex)
		//				{
		//					await transaction.RollbackAsync();
		//					// Log or handle the exception
		//				}
		//			}

		//			return RedirectToAction(nameof(Index));
		//		}
		//	}



		//	//string sqlstr = $@"SELECT DISTINCT It.SNo, It.Code, CONCAT(TRIM(It.ItemCode),'-',It.ItemName) As [ItemCode], It.ItemName, It.GerName, It.PurName, It.Catagory, It.HSNNo, It.Unit, It.Rate, It.IndianCustomsCode, It.GermanCustomsCode, It.BOMExtraAllowance, It.Picture, Im.SupplierID, sm.Name From PriceMaster a " +
		//	//										"INNER JOIN " +
		//	//										"ItemTypeMaster it on it.code=a.itemtype " +
		//	//										"INNER JOIN " +
		//	//										"colormaster co on co.Scode=a.itemcolor " +
		//	//										"INNER JOIN " +
		//	//										"CatagorySubMaster b on b.Code=it.Catagory " +
		//	//										"INNER JOIN " +
		//	//										"CatagoryMaster c on c.Code=b.MainCatName " +
		//	//										"LEFT OUTER JOIN " +
		//	//										"Indentmaster im on im.itemtype=it.code " +
		//	//										"LEFT OUTER JOIN " +
		//	//										"SupplierMaster sm on sm.Code= im.SupplierID " +
		//	//										"WHERE C.Name IN ('Cutting Die', 'General', 'Machinery Spares', 'Shoe Last', 'Fixed Assests', 'Work Order') " +
		//	//										"";

		//	ViewData["IndentType"] = new SelectList(_context.IndentMasterMaterialDisplays.Where(x => x.ProcurementOf == "InDirect" && x.Flag == true), "ID", "Department");


		//	ViewData["ItemColor"] = new SelectList(_context.ColorMasters, "SCode", "Code", indentMaster.ItemColor);
		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters, "Code", "ItemCode", indentMaster.ItemType);
		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters.FromSqlRaw(sqlstr), "Code", "ItemCode", indentMaster.ItemType);

		//	ViewData["SupplierID"] = new SelectList(_context.SupplierMasters, "Code", "Name", indentMaster.SupplierID);
		//	ViewData["Size"] = new SelectList(_context.SizeMasters, "Name", "Name", indentMaster.Size);
		//	ViewData["UOM"] = new SelectList(_context.UnitMasters, "Name", "Name", indentMaster.UOM);
		//	ViewData["SupplierPaymentTermID"] = new SelectList(_context.SupplierPaymentTerms, "ID", "PaymentTerms", indentMaster.SupplierPaymentTermID);

		//	var gstDetails = await GetGSTDetails(indentMaster.ItemType, indentMaster.SupplierID);

		//	ViewData["CGST"] = gstDetails.Select(x => x.CGST);
		//	ViewData["SGST"] = gstDetails.Select(x => x.SGST);
		//	ViewData["IGST"] = gstDetails.Select(x => x.IGST);

		//	return View(indentMaster);
		//}


		//// GET: IndentMasters/Edit/5
		//public async Task<IActionResult> Edit(long? id)
		//{

		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return NotFound();
		//	}

		//	//var indentMaster = await _context.IndentMasters.FindAsync(id);
		//	var indentMaster = await _context.IndentMasters
		//									.Include(x => x.IndentMasterFiles) // Include the related IndentMasterFiles
		//									.FirstOrDefaultAsync(im => im.Sno == id);
		//	if (indentMaster == null)
		//	{
		//		return NotFound();
		//	}


		//	//string sqlstr = $@"SELECT DISTINCT It.SNo, It.Code, CONCAT(TRIM(It.ItemCode),'-',It.ItemName) As [ItemCode], It.ItemName, It.GerName, It.PurName, It.Catagory, It.HSNNo, It.Unit, It.Rate, It.IndianCustomsCode, It.GermanCustomsCode, It.BOMExtraAllowance, It.Picture, Im.SupplierID, sm.Name From PriceMaster a " +
		//	//										"INNER JOIN " +
		//	//										"ItemTypeMaster it on it.code=a.itemtype " +
		//	//										"INNER JOIN " +
		//	//										"colormaster co on co.Scode=a.itemcolor " +
		//	//										"INNER JOIN " +
		//	//										"CatagorySubMaster b on b.Code=it.Catagory " +
		//	//										"INNER JOIN " +
		//	//										"CatagoryMaster c on c.Code=b.MainCatName " +
		//	//										"LEFT OUTER JOIN " +
		//	//										"Indentmaster im on im.itemtype=it.code " +
		//	//										"LEFT OUTER JOIN " +
		//	//										"SupplierMaster sm on sm.Code= im.SupplierID " +
		//	//										"WHERE C.Name IN ('Cutting Die', 'General', 'Machinery Spares', 'Shoe Last', 'Fixed Assests', 'Work Order') " +
		//	//										"";



		//	ViewData["IndentType"] = new SelectList(_context.IndentMasterMaterialDisplays.Where(x => x.ProcurementOf == "InDirect" && x.Flag == true), "ID", "Department", indentMaster.IndentType);

		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters, "Code", "ItemCode", indentMaster.ItemType);
		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters, "Code", "ItemCode", indentMaster.ItemType);


		//	// Extract SQL query from the entity
		//	var sqlQuery = await _context.IndentMasterMaterialDisplays.Where(x => x.ProcurementOf == "InDirect" && x.Flag == true && x.Department == indentMaster.IndentType).Select(q => q.Query).ToListAsync();


		//	var itemTypes = await _context.ItemTypeMasters.FromSqlRaw(sqlQuery[0])
		//					.OrderBy(x => x.ItemCode)
		//					.Select(n => new SelectListItem
		//					{
		//						Value = n.Code,
		//						Text = $"{n.ItemCode}-{n.ItemName}"
		//					}).ToListAsync();

		//	ViewData["ItemType"] = new SelectList(itemTypes, "Value", "Text", indentMaster.ItemType);


		//	ViewData["ItemColor"] = new SelectList(_context.ColorMasters, "SCode", "Code", indentMaster.ItemColor);
		//	ViewData["SupplierID"] = new SelectList(_context.SupplierMasters, "Code", "Name", indentMaster.SupplierID);
		//	ViewData["Size"] = new SelectList(_context.SizeMasters, "Name", "Name", indentMaster.Size);
		//	ViewData["UOM"] = new SelectList(_context.UnitMasters.Include(i => i.ItemTypeMasters), "Name", "Name", indentMaster.UOM);
		//	ViewData["SupplierPaymentTermID"] = new SelectList(_context.SupplierPaymentTerms, "ID", "PaymentTerms", indentMaster.SupplierPaymentTermID);
		//	return View(indentMaster);
		//}


		//// POST: IndentMasters/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(long id, [Bind("Sno,IndentType,Refno,ItemType,ItemColor,UOM,Size,IndentQty,Remarks,Price,SupplierID,SupplierPaymentTermID,DeliveryPeriod,LogUser")] IndentMaster indentMaster, List<IFormFile> files)
		//{

		//	// Inside a controller action or a view
		//	var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

		//	// Retrieve the currently authenticated user
		//	var user = await _userManager.FindByEmailAsync(useremail);

		//	indentMaster = await _context.IndentMasters
		//						.Include(x => x.IndentMasterFiles) // Include the related IndentMasterFiles
		//						.FirstOrDefaultAsync(im => im.Sno == id);

		//	if (user == null)
		//	{
		//		// Handle the case where the user is not found
		//		return NotFound();
		//	}

		//	// Retrieve the entity from the database
		//	var entityToUpdate = await _context.IndentMasters.FirstOrDefaultAsync(e => e.Sno == id); // Replace with your actual condition


		//	if (id != indentMaster.Sno && user.AccountID == entityToUpdate.Loguser)
		//	{
		//		return NotFound();
		//	}


		//	//[RequestSizeLimit(1000000)] // Limit to 1 MB
		//	foreach (var file in files.ToList())
		//	{
		//		if (file != null && file.Length > 1000000) // Limit to 1 MB
		//		{
		//			ModelState.AddModelError("", "The file is too large.");
		//		}
		//	}




		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			if (entityToUpdate != null)
		//			{
		//				// Update other properties as needed
		//				entityToUpdate.IndentType = indentMaster.IndentType;
		//				entityToUpdate.ItemType = indentMaster.ItemType;
		//				entityToUpdate.ItemColor = indentMaster.ItemColor;
		//				entityToUpdate.UOM = indentMaster.UOM;
		//				entityToUpdate.Size = indentMaster.Size;
		//				entityToUpdate.IndentQty = indentMaster.IndentQty;
		//				entityToUpdate.Remarks = indentMaster.Remarks;
		//				entityToUpdate.Price = indentMaster.Price;
		//				entityToUpdate.SupplierID = indentMaster.SupplierID;
		//				entityToUpdate.SupplierPaymentTermID = indentMaster.SupplierPaymentTermID;
		//				entityToUpdate.DeliveryPeriod = indentMaster.DeliveryPeriod;
		//				entityToUpdate.Loguser = user.AccountID;

		//				await _context.SaveChangesAsync();



		//				foreach (var file in files.ToList())
		//				{
		//					string _guidFileName = string.Empty;

		//					// Save file to storage (e.g., Azure Blob Storage) and get the BlobFileName
		//					_guidFileName = await _blobService.UploadAsync(file, _azureBlobContainer);

		//					// Create IndentMasterFile record
		//					var indentMasterFile = new IndentMasterFile
		//					{
		//						IndentMasterID = indentMaster.Sno,
		//						FileName = file.FileName,
		//						BlobFileName = _guidFileName
		//					};

		//					_context.Add(indentMasterFile);
		//					await _context.SaveChangesAsync();
		//				}


		//			}
		//			//_context.Update(indentMaster);
		//			//await _context.SaveChangesAsync();
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!IndentMasterExists(indentMaster.Sno))
		//			{
		//				return NotFound();
		//			}
		//			else
		//			{
		//				throw;
		//			}
		//		}
		//		return RedirectToAction(nameof(Index));
		//	}

		//	//string sqlstr = $@"SELECT it.SNo,it.Code, CONCAT(TRIM(it.ItemCode),'-',it.ItemName) As [ItemCode] ,it.ItemName,it.GerName,it.PurName,it.Catagory,it.HSNNo,it.Unit,it.Rate,it.IndianCustomsCode,it.GermanCustomsCode,it.BOMExtraAllowance,it.Picture From PriceMaster a " +
		//	//										"INNER JOIN " +
		//	//										"ItemTypeMaster it on it.code=a.itemtype " +
		//	//										"INNER JOIN " +
		//	//										"colormaster co on co.Scode=a.itemcolor " +
		//	//										"INNER JOIN " +
		//	//										"CatagorySubMaster b on b.Code=it.Catagory " +
		//	//										"INNER JOIN " +
		//	//										"CatagoryMaster c on c.Code=b.MainCatName " +
		//	//										"WHERE C.Name IN ('Cutting Die', 'General', 'Machinery Spares', 'Shoe Last', 'Fixed Assests', 'Work Order') " +
		//	//										"ORDER BY it.ItemCode";

		//	//List<string> indentTypes = new List<string> { "General", "HR", "IT", "Maintenance" };
		//	//ViewData["IndentType"] = new SelectList(indentTypes);

		//	ViewData["IndentType"] = new SelectList(_context.IndentMasterMaterialDisplays.Where(x => x.ProcurementOf == "InDirect" && x.Flag == true), "ID", "Department");


		//	ViewData["ItemColor"] = new SelectList(_context.ColorMasters, "SCode", "Code", indentMaster.ItemColor);
		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters, "Code", "ItemCode", indentMaster.ItemType);
		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters.FromSqlRaw(sqlstr), "Code", "ItemCode", indentMaster.ItemType);
		//	ViewData["SupplierID"] = new SelectList(_context.SupplierMasters, "Code", "Name", indentMaster.SupplierID);
		//	ViewData["Size"] = new SelectList(_context.SizeMasters, "Name", "Name", indentMaster.Size);
		//	ViewData["UOM"] = new SelectList(_context.UnitMasters, "Name", "Name", indentMaster.UOM);
		//	ViewData["SupplierPaymentTermID"] = new SelectList(_context.SupplierPaymentTerms, "ID", "PaymentTerms", indentMaster.SupplierPaymentTermID);
		//	return View(indentMaster);
		//}


		////// GET: IndentMasters/GetColorCode/IT001101
		////public async Task<IActionResult> GetColorCode(string? id)
		////{
		////    if (id == null || _context.IndentMasters == null)
		////    {
		////        return NotFound();
		////    }

		////    var itemColorMergeMaster = await _context.ItemColorMergeMasters
		////        .FirstOrDefaultAsync(x => x.ItemType == id);

		////    if (itemColorMergeMaster == null)
		////    {
		////        return NotFound();
		////    }
		////    ViewData["ItemColor"] = new SelectList(_context.ColorMasters, "SCode", "Code", itemColorMergeMaster.ItemColor);

		////    return View(itemColorMergeMaster);
		////}


		//[HttpGet]
		//public async Task<JsonResult> GetItemCodesByDepartment(Int32? id)
		//{

		//	// Assuming _context.IndentMasterMaterialDisplays.FindAsync(id) returns a single record
		//	var indentMasterMaterialDisplay = await _context.IndentMasterMaterialDisplays.FindAsync(id);

		//	if (indentMasterMaterialDisplay == null)
		//	{
		//		return Json(null);
		//	}
		//	//var indentMaster = await _context.IndentMasters.FirstOrDefaultAsync();

		//	//ViewData["ItemType"] = new SelectList(_context.ItemTypeMasters.FromSqlRaw(sqlstr), "Code", "ItemCode");

		//	// Extract SQL query from the entity
		//	var sqlQuery = indentMasterMaterialDisplay.Query;


		//	List<SelectListItem> list;
		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return Json(null);
		//	}

		//	// Execute the SQL query directly
		//	var itemTypes = await _context.ItemTypeMasters
		//								.FromSqlRaw(sqlQuery)
		//								.OrderBy(x => x.ItemCode)
		//								.Select(n =>
		//								new SelectListItem
		//								{
		//									Value = n.Code,
		//									Text = n.ItemCode
		//								}).ToListAsync();

		//	return Json(itemTypes);
		//}


		//[HttpGet]
		//public JsonResult GetColorCodesByItemCode(string? id)
		//{

		//	List<SelectListItem> list;

		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return Json(null);
		//	}

		//	list = _context.ItemColorMergeMasters
		//			.Where(x => x.ItemType == id)
		//			.OrderBy(x => x.ColorMaster.Code)
		//			.Select(n =>
		//			new SelectListItem
		//			{
		//				Value = n.ColorMaster.SCode,
		//				Text = n.ColorMaster.Code + " - " + n.ColorMaster.Name
		//			}).ToList();

		//	var defItem = new SelectListItem()
		//	{
		//		Value = "",
		//		Text = "----Select Code----"
		//	};

		//	list.Insert(0, defItem);
		//	return Json(list);
		//}

		//[HttpGet]
		//public JsonResult GetUomByItemCode(string? id)
		//{

		//	List<SelectListItem> list;
		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return Json(null);
		//	}

		//	list = _context.ItemTypeMasters
		//			.Where(x => x.Code == id)
		//			.OrderBy(x => x.UnitMaster.Name)
		//			.Select(n =>
		//			new SelectListItem
		//			{
		//				Value = n.UnitMaster.Name,
		//				Text = n.UnitMaster.Name
		//			}).ToList();

		//	var defItem = new SelectListItem()
		//	{
		//		Value = "",
		//		Text = "----Select Code----"
		//	};

		//	list.Insert(0, defItem);
		//	return Json(list);
		//}


		//[HttpGet]
		//public async Task<JsonResult> GetGSTDetailsByItemCode(string? id, string? vendorid)
		//{

		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return Json(null);
		//	}

		//	var gstDetails = await GetGSTDetails(id, vendorid);

		//	return Json(gstDetails);
		//}

		//private async Task<List<TaxMaster>> GetGSTDetails(string id, string vendorid)
		//{
		//	var list = new List<TaxMaster>();

		//	var supplierOrigin = await _context.SupplierMasters
		//		.Where(s => s.Code == vendorid)
		//		.Select(s => s.Origin)
		//		.FirstOrDefaultAsync();

		//	var HSNID = await _context.ItemTypeMasters
		//		.Where(s => s.Code == id)
		//		.Select(x => x.HSNNo)
		//		.FirstOrDefaultAsync();

		//	list = await _context.TaxMasters
		//		.Where(t => t.ID == HSNID && t.IsActive == true)
		//		.Select(n => new TaxMaster
		//		{
		//			HSN = Convert.ToString(n.HSN),
		//			CGST = supplierOrigin.ToLower().Trim() == "local" ? n.CGST : 0,
		//			SGST = supplierOrigin.ToLower().Trim() == "local" ? n.SGST : 0,
		//			IGST = supplierOrigin.ToLower().Trim() == "local" ? 0 : n.IGST
		//		})
		//		.ToListAsync();

		//	return list;
		//}


		//[HttpGet]
		//public async Task<JsonResult> GetGSTDetailsByIndentNo(string? id)
		//{

		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return Json(null);
		//	}

		//	var gstDetails = await GetGSTDetails(id);

		//	return Json(gstDetails);
		//}

		//private async Task<List<TaxMaster>> GetGSTDetails(string id)
		//{
		//	var list = new List<TaxMaster>();

		//	var vendorid = await _context.IndentMasters
		//		.Where(s => s.Sno == Convert.ToInt64(id))
		//		.Select(s => s.SupplierID)
		//		.FirstOrDefaultAsync();

		//	var supplierOrigin = await _context.SupplierMasters
		//		.Where(s => s.Code == vendorid)
		//		.Select(s => s.Origin)
		//		.FirstOrDefaultAsync();

		//	var HSNID = await _context.IndentMasters
		//		 .Include(x => x.ItemTypeMaster)
		//		.Where(s => s.Sno == Convert.ToInt64(id))
		//		.Select(s => s.ItemTypeMaster.HSNNo)
		//		.FirstOrDefaultAsync();

		//	list = await _context.TaxMasters
		//		.Where(t => t.ID == HSNID && t.IsActive == true)
		//		.Select(n => new TaxMaster
		//		{
		//			HSN = Convert.ToString(n.HSN),
		//			CGST = supplierOrigin.ToLower().Trim() == "local" ? n.CGST : 0,
		//			SGST = supplierOrigin.ToLower().Trim() == "local" ? n.SGST : 0,
		//			IGST = supplierOrigin.ToLower().Trim() == "local" ? 0 : n.IGST
		//		})
		//		.ToListAsync();

		//	return list;
		//}

		//// GET: IndentMasters/Delete/5
		//public async Task<IActionResult> Delete(long? id)
		//{
		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return NotFound();
		//	}

		//	var indentMaster = await _context.IndentMasters
		//		.Include(i => i.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//		.Include(i => i.ColorMaster)
		//		.Include(i => i.SupplierMaster)
		//		.Include(i => i.SizeMaster)
		//		.Include(i => i.UnitMaster)
		//		.Include(i => i.SupplierPaymentTerm)
		//		.Include(i => i.ApplicationUser)
		//		.FirstOrDefaultAsync(m => m.Sno == id);
		//	if (indentMaster == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(indentMaster);
		//}


		//// POST: IndentMasters/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(long id)
		//{
		//	if (_context.IndentMasters == null)
		//	{
		//		return Problem("Entity set 'ApplicationDbContext.IndentMasters'  is null.");
		//	}
		//	var indentMaster = await _context.IndentMasters.FindAsync(id);
		//	if (indentMaster != null)
		//	{
		//		_context.IndentMasters.Remove(indentMaster);
		//	}

		//	await _context.SaveChangesAsync();
		//	return RedirectToAction(nameof(Index));
		//}


		//private bool IndentMasterExists(long id)
		//{
		//	return (_context.IndentMasters?.Any(e => e.Sno == id)).GetValueOrDefault();
		//}


		////[HttpPost]
		////public JsonResult GetItemName(string id)
		////{
		////    var applicationDbContext = _context.ItemTypeMasters.Where(z => z.Code == id);
		////    return Json();
		////}

		//// GET: Approve
		//public async Task<IActionResult> ApprovalIndex(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		//{

		//	// Inside a controller action or a view
		//	var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

		//	// Retrieve the currently authenticated user
		//	var user = await _userManager.FindByEmailAsync(useremail);


		//	if (user == null)
		//	{
		//		// Handle the case where the user is not found
		//		return NotFound();
		//	}


		//	var applicationDbContext = _context.IndentMasters
		//		.Where(x => x.IsCancel == false && x.IsPoGen == false && x.IsApproved == false)
		//		.Include(a => a.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//		.Include(b => b.ColorMaster)
		//		.Include(c => c.SupplierMaster)
		//		.Include(d => d.SizeMaster)
		//		.Include(e => e.UnitMaster)
		//		.Include(f => f.SupplierPaymentTerm)
		//		.Include(g => g.ApplicationUser).Where(x => x.Loguser == user.AccountID).AsQueryable();

		//	ViewData["CurrentSort"] = sortOrder;
		//	ViewData["NameSortParm"] = System.String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
		//	ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
		//	//ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;

		//	if (searchString != null)
		//	{
		//		pageNumber = 1;
		//	}
		//	else
		//	{
		//		searchString = currentFilter;
		//	}

		//	ViewData["CurrentFilter"] = searchString;

		//	if (!System.String.IsNullOrEmpty(searchString))
		//	{
		//		applicationDbContext = applicationDbContext.Where(IndentMaster => IndentMaster.ItemTypeMaster.ItemCode.Contains(searchString)
		//													|| IndentMaster.ColorMaster.Code.Contains(searchString)
		//													|| IndentMaster.SupplierMaster.Name.Contains(searchString)
		//													|| IndentMaster.Remarks.Contains(searchString));
		//	}

		//	switch (sortOrder)
		//	{
		//		case "name_desc":
		//			applicationDbContext = applicationDbContext.OrderByDescending(IndentMaster => IndentMaster.ItemTypeMaster.ItemCode);
		//			break;
		//		case "Date":
		//			applicationDbContext = applicationDbContext.OrderBy(IndentMaster => IndentMaster.IndentDate);
		//			break;
		//		case "date_desc":
		//			applicationDbContext = applicationDbContext.OrderByDescending(IndentMaster => IndentMaster.IndentDate);
		//			break;
		//		default:  // Name ascending 
		//			applicationDbContext = applicationDbContext.OrderByDescending(IndentMaster => IndentMaster.Sno);
		//			break;
		//	}

		//	if (pageSize == 0 || pageSize == null)
		//	{
		//		pageSize = _configuration.GetValue("PageSize", 10); ;
		//		ViewData["PageSize"] = pageSize;
		//	}
		//	else
		//	{
		//		ViewData["PageSize"] = ViewBag.PageSize;
		//		pageSize = ViewBag.PageSize;
		//	}
		//	//return View(await applicationDbContext.ToListAsync());

		//	return View("~/Views/IndentMasters/ApprovalIndex.cshtml", await PaginatedList<IndentMaster>.CreateAsync(applicationDbContext.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10));
		//}


		//// GET: IndentMasters/Approve/5
		//public async Task<IActionResult> Approve(long? id)
		//{
		//	if (id == null || _context.IndentMasters == null)
		//	{
		//		return NotFound();
		//	}

		//	var indentMaster = await _context.IndentMasters
		//		.Include(i => i.IndentMasterFiles)
		//		.Include(i => i.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//		.Include(i => i.ColorMaster)
		//		.Include(i => i.SupplierMaster)
		//		.Include(i => i.SizeMaster)
		//		.Include(i => i.UnitMaster)
		//		.Include(i => i.SupplierPaymentTerm)
		//		.Include(i => i.ApplicationUser)
		//		.FirstOrDefaultAsync(m => m.Sno == id);


		//	if (indentMaster == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(indentMaster);
		//}


		//public async Task<IActionResult> Approval(long id, string remarks)//, string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	// Inside a controller action or a view
		//	var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

		//	// Retrieve the currently authenticated user
		//	var user = await _userManager.FindByEmailAsync(useremail);

		//	if (user == null)
		//	{
		//		// Handle the case where the user is not found
		//		return NotFound();
		//	}

		//	// Check the user is mapped for authorization for approval
		//	var IsAuthorized = _context.ProcurementApprovalUserMappings.FirstOrDefault(a => a.UserId == user.Id);

		//	if (IsAuthorized is null)
		//	{
		//		return Unauthorized();
		//	}

		//	// Check the indent requisition is available
		//	//var indentMaster = await _context.IndentMasters.FindAsync(id);

		//	var indentMaster = await _context.IndentMasters
		//				.Include(i => i.IndentMasterFiles)
		//				.Include(i => i.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//				.Include(i => i.ColorMaster)
		//				.Include(i => i.SupplierMaster)
		//				.Include(i => i.SizeMaster)
		//				.Include(i => i.UnitMaster)
		//				.Include(i => i.SupplierPaymentTerm)
		//				.Include(i => i.ApplicationUser)
		//				.FirstOrDefaultAsync(m => m.Sno == id);


		//	if (indentMaster == null)
		//	{
		//		// Handle the indent requisition incase not available
		//		return NotFound();
		//	}

		//	// Check the user approval stage for authorization of approval
		//	var userApprovalStage = _context.ProcurementApprovalUserMappings
		//										.Include(i => i.ProcurementApprovalMaster)
		//										.FirstOrDefault(i => i.UserId == user.Id)?
		//										.ProcurementApprovalMaster?.ApprovalStage;





		//	var applicationDbContext = _context.IndentMasters
		//	.Where(x => x.IsCancel == false && x.IsPoGen == false && x.IsApproved == false)
		//		.Include(a => a.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//		.Include(b => b.ColorMaster)
		//		.Include(c => c.SupplierMaster)
		//		.Include(d => d.SizeMaster)
		//		.Include(e => e.SupplierPaymentTerm)
		//		.Include(f => f.ApplicationUser).Where(x => x.Loguser == user.AccountID).AsQueryable();


		//	//return View(await applicationDbContext.ToListAsync());


		//	if (userApprovalStage is null || userApprovalStage == 0)
		//	{
		//		return Unauthorized();
		//	}

		//	else if (userApprovalStage != (int)indentMaster.CurrentApprovalStage + 1) // <= Will not give any room for reconsideration in approval/rejection, whereas < will provide a space for reconsideration in approval/rejection.
		//	{
		//		// Set a message in ViewBag
		//		ViewBag.Message = "You've already approved this item :" + id;

		//		// Return the 'Approval.cshtml' view with the message and indentMaster
		//		return View("~/Views/IndentMasters/Approve.cshtml", indentMaster);

		//	}
		//	else if (userApprovalStage == (int)indentMaster.CurrentApprovalStage + 1) // <= Will not give any room for reconsideration in approval/rejection, whereas < will provide a space for reconsideration in approval/rejection.
		//	{

		//		// Continue with your logic for approval...

		//		//First Update the userapprovalstage to indent master
		//		using (var transactions = _context.Database.BeginTransaction())
		//		{
		//			try
		//			{
		//				// Update other properties as needed

		//				indentMaster.CurrentApprovalStage = (int)userApprovalStage;
		//				if (userApprovalStage == 4)
		//				{
		//					indentMaster.IsApproved = true;
		//				}
		//				_context.IndentMasters.Update(indentMaster);
		//				await _context.SaveChangesAsync();
		//				await transactions.CommitAsync();

		//			}
		//			catch (Exception ex)
		//			{
		//				transactions.Rollback();

		//				// Set a message in ViewBag
		//				ViewBag.Message = "Error In Commiting Transaction : " + id;

		//				// Return the 'Approval.cshtml' view with the message and indentMaster
		//				return View("~/Views/IndentMasters/Approve.cshtml", indentMaster);

		//			}
		//		}

		//		//Second Update the userapprovalstage & user details to approval history table
		//		using (var transaction = _context.Database.BeginTransaction())
		//		{
		//			try
		//			{
		//				// Update other properties as needed

		//				var indentapprovalhistory = new IndentApprovalHistory
		//				{
		//					IndentMasterId = id,
		//					ApproverAccountID = user.AccountID,
		//					ApprovalStatus = true,
		//					CurrentApprovalStage = (int)userApprovalStage,
		//					ApprovalTime = DateTime.Now,
		//					Remarks = remarks
		//				};


		//				await _context.IndentApprovalHistories.AddAsync(indentapprovalhistory);
		//				await _context.SaveChangesAsync();
		//				await transaction.CommitAsync();

		//			}
		//			catch (Exception ex)
		//			{
		//				await transaction.RollbackAsync();

		//				// Set a message in ViewBag
		//				ViewBag.Message = "Error In Commiting Transaction : " + id;

		//				// Return the 'Approval.cshtml' view with the message and indentMaster
		//				return View("~/Views/IndentMasters/Approve.cshtml", indentMaster);
		//			}
		//		}
		//	}
		//	//return RedirectToAction(nameof(Index));
		//	return View("~/Views/IndentMasters/ApprovalIndex.cshtml");
		//}


		//public async Task<IActionResult> Reject(long id, string remarks)//, string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	// Inside a controller action or a view
		//	var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

		//	// Retrieve the currently authenticated user
		//	var user = await _userManager.FindByEmailAsync(useremail);

		//	if (user == null)
		//	{
		//		// Handle the case where the user is not found
		//		return NotFound();
		//	}

		//	// Check the user is mapped for authorization for approval
		//	var IsAuthorized = _context.ProcurementApprovalUserMappings.FirstOrDefault(a => a.UserId == user.Id);

		//	if (IsAuthorized is null)
		//	{
		//		return Unauthorized();
		//	}

		//	// Check the indent requisition is available
		//	//var indentMaster = await _context.IndentMasters.FindAsync(id);
		//	var indentMaster = await _context.IndentMasters
		//						.Include(i => i.IndentMasterFiles)
		//						.Include(i => i.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//						.Include(i => i.ColorMaster)
		//						.Include(i => i.SupplierMaster)
		//						.Include(i => i.SizeMaster)
		//						.Include(i => i.UnitMaster)
		//						.Include(i => i.SupplierPaymentTerm)
		//						.Include(i => i.ApplicationUser)
		//						.FirstOrDefaultAsync(m => m.Sno == id);

		//	if (indentMaster == null)
		//	{
		//		// Handle the indent requisition incase not available
		//		return NotFound();
		//	}

		//	// Check the user approval stage for authorization of approval
		//	var userApprovalStage = _context.ProcurementApprovalUserMappings
		//										.Include(i => i.ProcurementApprovalMaster)
		//										.FirstOrDefault(i => i.UserId == user.Id)?
		//										.ProcurementApprovalMaster?.ApprovalStage;


		//	var applicationDbContext = _context.IndentMasters
		//		.Where(x => x.IsCancel == false && x.IsPoGen == false && x.IsApproved == false)
		//		.Include(a => a.ItemTypeMaster)//.ThenInclude(t => t.TaxMaster)
		//		.Include(b => b.ColorMaster)
		//		.Include(c => c.SupplierMaster)
		//		.Include(d => d.SizeMaster)
		//		.Include(e => e.SupplierPaymentTerm)
		//		.Include(f => f.ApplicationUser).Where(x => x.Loguser == user.AccountID).AsQueryable();



		//	//return View(await applicationDbContext.ToListAsync());


		//	if (userApprovalStage is null || userApprovalStage == 0)
		//	{
		//		return Unauthorized();
		//	}

		//	else if (userApprovalStage < indentMaster.CurrentApprovalStage) // <= Will not give any room for reconsideration in approval/rejection, whereas < will provide a space for reconsideration in approval/rejection.
		//	{
		//		// Set a message in ViewBag
		//		ViewBag.Message = "You've already rejected approval for this item :" + id;

		//		// Return the 'Approval.cshtml' view with the message
		//		return View("~/Views/IndentMasters/Approval.cshtml", new { id });
		//	}

		//	// Continue with your logic for approval...

		//	//First Update the userapprovalstage to indent master
		//	using (var transactions = _context.Database.BeginTransaction())
		//	{
		//		try
		//		{
		//			// Update other properties as needed

		//			indentMaster.CurrentApprovalStage = (int)userApprovalStage;
		//			if (userApprovalStage == 1)
		//			{
		//				indentMaster.IsCancel = true;
		//			}

		//			_context.IndentMasters.Update(indentMaster);
		//			await _context.SaveChangesAsync();
		//			await transactions.CommitAsync();
		//		}
		//		catch (Exception)
		//		{
		//			await transactions.RollbackAsync();

		//			// Set a message in ViewBag
		//			ViewBag.Message = "Error In Commiting Transaction : " + id;

		//			// Return the 'Approval.cshtml' view with the message
		//			return View("~/Views/IndentMasters/Approval.cshtml", new { id });
		//		}
		//	}

		//	//Second Update the userapprovalstage & user details to approval history table
		//	using (var transaction = _context.Database.BeginTransaction())
		//	{
		//		try
		//		{
		//			// Update other properties as needed

		//			var indentapprovalhistory = new IndentApprovalHistory
		//			{
		//				IndentMasterId = id,
		//				ApproverAccountID = user.AccountID,
		//				ApprovalStatus = false,
		//				CurrentApprovalStage = (int)userApprovalStage,
		//				ApprovalTime = DateTime.Now,
		//				Remarks = remarks
		//			};


		//			await _context.IndentApprovalHistories.AddAsync(indentapprovalhistory);
		//			await _context.SaveChangesAsync();
		//			await transaction.CommitAsync();

		//		}
		//		catch (Exception)
		//		{
		//			await transaction.RollbackAsync();

		//			// Set a message in ViewBag
		//			ViewBag.Message = "Error In Commiting Transaction : " + id;

		//			// Return the 'Approval.cshtml' view with the message
		//			return View("~/Views/IndentMasters/Approval.cshtml", new { id });
		//		}
		//	}

		//	//return RedirectToAction(nameof(Index));
		//	return View("~/Views/IndentMasters/ApprovalIndex.cshtml");
		//}


		//[HttpGet]
		//public async Task<IActionResult> Download(string fileName)
		//{
		//	var stream = await _blobService.DownloadAsync(fileName, _azureBlobContainer);

		//	return File(stream, "application/pdf", fileName);
		//}

		//[HttpGet]
		//public async Task<IActionResult> OpenBlobInBrowser(string fileName)
		//{
		//	try
		//	{
		//		//// Get the SAS URI for the blob file
		//		//var sasUri = await _blobService.GetSasUriAsync(fileName);

		//		//// Redirect to the SAS URI
		//		//return Redirect(sasUri);

		//		var stream = await _blobService.DownloadAsync(fileName, _azureBlobContainer);
		//		// Determine the content type of the file
		//		string contentType = GetContentType(fileName);

		//		return File(stream, contentType, fileName);

		//	}
		//	catch (Exception ex)
		//	{
		//		// Log or handle the error appropriately
		//		return Content($"An error occurred: {ex.Message}");
		//	}

		//}

		//// Method to determine the content type based on file extension
		//private string GetContentType(string fileName)
		//{
		//	string extension = Path.GetExtension(fileName).ToLowerInvariant();

		//	switch (extension)
		//	{
		//		case ".pdf":
		//			return "application/pdf";
		//		case ".png":
		//			return "image/png";
		//		case ".jpg":
		//		case ".jpeg":
		//			return "image/jpeg";
		//		case ".gif":
		//			return "image/gif";
		//		// Add more cases for other image formats if needed
		//		default:
		//			// Default to application/octet-stream for unknown types
		//			return "application/octet-stream";
		//	}
		//}


		//[HttpGet]
		//public async Task<IActionResult> ProxyPdf(string fileName)
		//{
		//	try
		//	{
		//		var stream = await _blobService.DownloadAsync(fileName, _azureBlobContainer);

		//		return File(stream, "application/pdf", fileName);
		//	}
		//	catch (Exception ex)
		//	{
		//		// Log or handle the error appropriately
		//		return Content($"An error occurred: {ex.Message}");
		//	}
		//}


		////[HttpPost]
		//public async Task<IActionResult> DeleteFile(string fileName, long ID)
		//{
		//	if (fileName == null || string.IsNullOrEmpty(fileName))
		//	{
		//		ViewBag.Message = "Invalid file information.";
		//		return StatusCode(404);
		//	}

		//	try
		//	{
		//		// Get IndentMasterFile based on ID
		//		var indentMasterFile = await _context.IndentMasterFiles.FindAsync(ID);

		//		// Check if indentMasterFile is not null
		//		if (indentMasterFile != null)
		//		{
		//			// Get IndentMaster based on IndentMasterID

		//			var indentmaster = await _context.IndentMasters.Where(x => x.Sno == indentMasterFile.IndentMasterID && x.CurrentApprovalStage == 0).FirstOrDefaultAsync();

		//			// Check if the indentmaster is found
		//			if (indentmaster != null)
		//			{
		//				// Delete the blob file in azure

		//				await _blobService.DeleteAsync(fileName, _azureBlobContainer);

		//				// Delete the blob file in azure
		//				_context.IndentMasterFiles.Remove(indentMasterFile);
		//				await _context.SaveChangesAsync();
		//				ViewBag.Message = "{fileName} removed successfully";
		//			}
		//			else
		//			{
		//				ViewBag.Message = "Some one has already verified and approved this indent";
		//			}
		//			// Redirect to the same page with the ID parameter
		//			return RedirectToAction("Edit", new { ID = indentmaster.Sno });
		//		}

		//	}
		//	catch
		//	{
		//		ViewBag.Message = "Error removing file";
		//	}
		//	return View("Index", "IndentMasters");
		//}

	}
}


