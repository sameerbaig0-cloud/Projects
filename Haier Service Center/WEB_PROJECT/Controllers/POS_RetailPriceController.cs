using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using ServicePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace ServicePlatform.Controllers
{
    
	[Authorize]
	[AutoValidateAntiforgeryToken]
	[CustomAuthorize]

	public class POS_RetailPriceController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly BlobService _blobService;
		private readonly string _azureBlobContainer;

		public POS_RetailPriceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, BlobService blobService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _blobService = blobService;
            _configuration = configuration;
        }

        // GET: POS_RetailPrice
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.POS_RetailPrices.Include(p => p.ArticleMaster).Include(p => p.POS_Taxation);
            //return View(await applicationDbContext.ToListAsync());
            return View();
        }

  //      // GET: POS_RetailPrice/Details/5
  //      public async Task<IActionResult> Details(long? id)
  //      {
  //          if (id == null || _context.POS_RetailPrices == null)
  //          {
  //              return NotFound();
  //          }

  //          var pOS_RetailPrice = await _context.POS_RetailPrices
  //              .Include(p => p.ArticleMaster)
  //              .Include(p => p.POS_Taxation)
  //              .FirstOrDefaultAsync(m => m.PriceID == id);
  //          if (pOS_RetailPrice == null)
  //          {
  //              return NotFound();
  //          }

  //          return View(pOS_RetailPrice);
  //      }

  //      // GET: POS_RetailPrice/Create
  //      public IActionResult Create()
  //      {
  //          ViewData["ArticleNo"] = new SelectList(_context.ArticleMasters, "Article", "Article");
  //          ViewData["TaxID"] = new SelectList(_context.POS_Taxations, "TaxID", "HSNCode");
  //          return View();
  //      }

  //      // POST: POS_RetailPrice/Create
  //      // To protect from overposting attacks, enable the specific properties you want to bind to.
  //      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  //      [HttpPost]
  //      [ValidateAntiForgeryToken]
  //      public async Task<IActionResult> Create([Bind("PriceID,ArticleNo,Price,TaxID,ModifiedDatetime,IsActive")] POS_RetailPrice pOS_RetailPrice)
  //      {
  //          if (ModelState.IsValid)
  //          {

		//		// Inside a controller action or a view
		//		var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
		//		//HttpContext.Response.Redirect("/AccessDenied");

		//		// Retrieve the currently authenticated user
		//		var user = await _userManager.FindByEmailAsync(useremail);

		//		if (user == null)
		//		{   // Handle the case where the user is not found
		//			return NotFound();
		//		}

		//		// Check the article picture is exists
		//		var articleExists = _context.POS_RetailPrices.Where(x => x.ArticleNo == pOS_RetailPrice.ArticleNo && x.IsActive == true);

  //              if(articleExists != null)
  //              {
  //                  // Set the article IsActive property from True to False
		//			await articleExists.ForEachAsync(article => article.IsActive = false);
  //                  // Update the changes to database
  //                  await _context.SaveChangesAsync();  
		//		}

		//		pOS_RetailPrice.UserID = user.Id; 
  //              pOS_RetailPrice.ModifiedDatetime = DateTime.Now;
  //              pOS_RetailPrice.IsActive    = true;

  //              _context.Add(pOS_RetailPrice);
  //              await _context.SaveChangesAsync();
  //              return RedirectToAction(nameof(Index));
  //          }

  //          ViewData["ArticleNo"] = new SelectList(_context.ArticleMasters, "Article", "Article", pOS_RetailPrice.ArticleNo);
  //          ViewData["TaxID"] = new SelectList(_context.POS_Taxations, "TaxID", "HSNCode", pOS_RetailPrice.TaxID);

		//	ViewBag.OutFlag = true;
		//	ViewBag.OutRemarks = "An error occurred while saving the data.";

		//	return View(pOS_RetailPrice);
  //      }



  //      // GET: POS_RetailPrice/Edit/5
  //      public async Task<IActionResult> Edit(long? id)
  //      {
  //          if (id == null || _context.POS_RetailPrices == null)
  //          {
  //              return NotFound();
  //          }

  //          var pOS_RetailPrice = await _context.POS_RetailPrices.FindAsync(id);
  //          if (pOS_RetailPrice == null)
  //          {
  //              return NotFound();
  //          }
  //          ViewData["ArticleNo"] = new SelectList(_context.ArticleMasters, "Article", "Article", pOS_RetailPrice.ArticleNo);
  //          ViewData["TaxID"] = new SelectList(_context.POS_Taxations, "TaxID", "HSNCode", pOS_RetailPrice.TaxID);
  //          return View(pOS_RetailPrice);
  //      }

  //      // POST: POS_RetailPrice/Edit/5
  //      // To protect from overposting attacks, enable the specific properties you want to bind to.
  //      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  //      [HttpPost]
  //      [ValidateAntiForgeryToken]
  //      public async Task<IActionResult> Edit(long id, [Bind("PriceID,ArticleNo,Price,TaxID,ModifiedDatetime,IsActive")] POS_RetailPrice pOS_RetailPrice)
  //      {
  //          if (id != pOS_RetailPrice.PriceID)
  //          {
  //              return NotFound();
  //          }

  //          // Retrieve the existing record
  //          var existingRecord = await _context.POS_RetailPrices
  //              .AsNoTracking()
  //              .FirstOrDefaultAsync(x => x.PriceID == id);

  //          if (existingRecord == null)
  //          {
  //              return NotFound();
  //          }

  //          // Check if the existing record's IsActive property is false
  //          if ((bool)!existingRecord.IsActive)
  //          {
  //              ModelState.AddModelError("", "Cannot edit this record because it is inactive.");
  //              ViewData["ArticleNo"] = new SelectList(_context.ArticleMasters, "Article", "Article", pOS_RetailPrice.ArticleNo);
  //              ViewData["TaxID"] = new SelectList(_context.POS_Taxations, "TaxID", "HSNCode", pOS_RetailPrice.TaxID);
  //              return View(pOS_RetailPrice);
  //          }


  //          if (ModelState.IsValid)
  //          {
  //              try
  //              {
  //                  _context.Update(pOS_RetailPrice);
  //                  await _context.SaveChangesAsync();
  //              }
  //              catch (DbUpdateConcurrencyException)
  //              {
  //                  if (!POS_RetailPriceExists(pOS_RetailPrice.PriceID))
  //                  {
  //                      return NotFound();
  //                  }
  //                  else
  //                  {
  //                      throw;
  //                  }
  //              }
  //              return RedirectToAction(nameof(Index));
  //          }
  //          ViewData["ArticleNo"] = new SelectList(_context.ArticleMasters, "Article", "Article", pOS_RetailPrice.ArticleNo);
  //          ViewData["TaxID"] = new SelectList(_context.POS_Taxations, "TaxID", "HSNCode", pOS_RetailPrice.TaxID);
  //          return View(pOS_RetailPrice);
  //      }

  //      // GET: POS_RetailPrice/Delete/5
  //      public async Task<IActionResult> Delete(long? id)
  //      {
  //          if (id == null || _context.POS_RetailPrices == null)
  //          {
  //              return NotFound();
  //          }

  //          var pOS_RetailPrice = await _context.POS_RetailPrices
  //              .Include(p => p.ArticleMaster)
  //              .Include(p => p.POS_Taxation)
  //              .FirstOrDefaultAsync(m => m.PriceID == id);
  //          if (pOS_RetailPrice == null)
  //          {
  //              return NotFound();
  //          }

  //          return View(pOS_RetailPrice);
  //      }

  //      // POST: POS_RetailPrice/Delete/5
  //      [HttpPost, ActionName("Delete")]
  //      [ValidateAntiForgeryToken]
  //      public async Task<IActionResult> DeleteConfirmed(long id)
  //      {
  //          if (_context.POS_RetailPrices == null)
  //          {
  //              return Problem("Entity set 'ApplicationDbContext.POS_RetailPrices'  is null.");
  //          }
  //          //var pOS_RetailPrice = await _context.POS_RetailPrices.FindAsync(id);
  //          //if (pOS_RetailPrice != null)
  //          //{
  //          //    _context.POS_RetailPrices.Remove(pOS_RetailPrice);
  //          //}
            
  //          //await _context.SaveChangesAsync();
  //          return RedirectToAction(nameof(Index));
  //      }

  //      private bool POS_RetailPriceExists(long id)
  //      {
  //        return (_context.POS_RetailPrices?.Any(e => e.PriceID == id)).GetValueOrDefault();
  //      }


  //      [HttpGet]
		//public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		//{
		//	ViewData["CurrentSort"] = sortOrder;
		//	ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
		//	ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;

		//	if (searchString != null)
		//	{
		//		pageNumber = 1;
		//	}
		//	else
		//	{
		//		searchString = currentFilter;
		//	}

		//	ViewData["CurrentFilter"] = searchString;

		//	var query = _context.POS_RetailPrices
		//			.Include(p => p.ArticleMaster)
		//			.Include(p => p.POS_Taxation) // Include POS_Taxation navigation property
		//			.AsQueryable();

		//	if (!string.IsNullOrEmpty(searchString))
		//	{
		//		query = query.Where(x =>
		//			x.ArticleNo.Contains(searchString) ||
  //                  x.Price.ToString().Contains(searchString) ||
  //                  x.POS_Taxation.HSNCode.Contains(searchString)||
  //                  x.ModifiedDatetime.ToString().Contains(searchString)
		//		);
		//	}

		//	switch (sortOrder)
		//	{
		//		case "name_desc":
		//			query = query.OrderByDescending(x => x.PriceID); // Sorting by LastName of associated ArticleMaster
		//			break;
		//		// Add more sorting cases if needed
		//		default:
		//			query = query.OrderBy(x => x.PriceID); // Sorting by LastName of associated ArticleMaster
		//			break;
		//	}

		//	var paginatedRetailPrices = await PaginatedList<POS_RetailPrice>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10);

		//	return View(paginatedRetailPrices);
		//}

	}
}
