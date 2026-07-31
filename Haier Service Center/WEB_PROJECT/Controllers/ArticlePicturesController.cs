using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ServicePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using ServicePlatform.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ServicePlatform.Controllers
{

	[Authorize]
	[AutoValidateAntiforgeryToken]
	[CustomAuthorize]
	public class ArticlePicturesController : Controller
	{

		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly BlobService _blobService;
		private readonly string _azureBlobContainer;
		private readonly IArticlePictureService _articlePictureService;

		public ArticlePicturesController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, BlobService blobService, IArticlePictureService articlePictureService)
		{
			_context = context;
			_configuration = configuration;
			_userManager = userManager;
			_blobService = blobService;
			_azureBlobContainer = _configuration.GetValue<string>("ProductionStylesBlobStorageContainer");
			_articlePictureService = articlePictureService;
		}

		// GET: ArticlePictures
		public async Task<IActionResult> Index()
		{
            //var applicationDbContext = _context.ArticlePictures.OrderByDescending(a => a.PictureID).Include(a => a.ApplicationUser).Include(a => a.ArticleMaster);
            //return View(await applicationDbContext.ToListAsync());

            return View();
        }

        // GET: ArticlePictures/Details/5
        public async Task<IActionResult> Details(long? id)
		{
            //if (id == null || _context.ArticlePictures == null)
            //{
            //	return NotFound();
            //}

            //var articlePicture = await _context.ArticlePictures
            //	.Include(a => a.ApplicationUser)
            //	.Include(a => a.ArticleMaster)
            //	.FirstOrDefaultAsync(m => m.PictureID == id);

            //if (articlePicture == null)
            //{
            //	return NotFound();

            //	ViewBag.OutFlag = false;
            //	ViewBag.OutRemarks = "Picture not found.";
            //}


            //ViewData["blobUrl"] = await _blobService.GetSasUriAsync_ImageView(articlePicture.BlobFileName, _azureBlobContainer);

            //return View(articlePicture);

            return View();

        }


        // GET: ArticlePictures/Create
        public IActionResult Create()
		{
			////ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email");
			//var articles = _context.ArticleMasters
			//		.Where(x => x.Article != null && x.Article.Length >= 4)
			//		.Select(x => x.Article)
			//		.Distinct()
			//		.ToList();

			//ViewData["Article"] = new SelectList(articles, "Article");

			return View();
		}

		// POST: ArticlePictures/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("PictureID,Article,FileName,BlobFileName,UserID,CreatedOn,ModifiedOn,Flag,Files")] ArticlePicture articlePicture)
        public async Task<IActionResult> Create([Bind("PictureID,Article,FileName,BlobFileName,UserID,CreatedOn,ModifiedOn,Flag,Files")] Model articlePicture)
		{

			//// Inside a controller action or a view
			//var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

			//// Retrieve the currently authenticated user
			//var user = await _userManager.FindByEmailAsync(useremail);


			//if (user == null)
			//{
			//	// Handle the case where the user is not found
			//	return NotFound();
			//}

			//if (ModelState.IsValid)
			//{

			//	try
			//	{
			//		//UPDATING THE REFERENCE NO COLUMN AFTER THE TRANSACTION IS COMPLETED

			//		foreach (var file in articlePicture.Files)
			//		{
			//			string _guidFileName = string.Empty;

			//			// Save file to storage (e.g., Azure Blob Storage) and get the BlobFileName
			//			_guidFileName = await _blobService.UploadAsync(file, _azureBlobContainer);

			//			// Create Article Pictures record

			//			// Initialize a new instance for each file
			//			var newArticlePicture = new ArticlePicture
			//			{
			//				Article = articlePicture.Article,
			//				FileName = file.FileName,
			//				BlobFileName = _guidFileName,
			//				UserID = user.Id,
			//				CreatedOn = DateTime.Now,
			//				Flag = true
			//			};

			//			_context.Add(newArticlePicture);
			//			await _context.SaveChangesAsync();


			//			ViewBag.OutFlag = true;
			//			ViewBag.OutRemarks = "Picture updated successfull.";
			//		}

			//		// If all operations succeed, redirect to the Index action
			//		return RedirectToAction(nameof(Index));
			//	}
			//	catch (Exception ex)
			//	{
			//		// Log or handle the exception
			//		ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");

			//	}
			//}
			//else
			//{

			//	// Retrieve the list of errors
			//	var errors = new List<KeyValuePair<string, string>>();
			//	foreach (var state in ModelState)
			//	{
			//		foreach (var error in state.Value.Errors)
			//		{
			//			errors.Add(new KeyValuePair<string, string>(state.Key, error.ErrorMessage));
			//		}
			//	}

			//	// Clear existing errors if needed and add formatted errors
			//	foreach (var error in errors)
			//	{
			//		ModelState.AddModelError(string.Empty, $"{error.Key}: {error.Value}");
			//	}


			//}


			////var articles = _context.ArticleMasters
			////.Where(x => x.Article != null && x.Article.Length >= 4)
			////.Select(x => x.Article)
			////.Distinct()
			////.ToList();

			////ViewData["Article"] = new SelectList(articles, "Article");

			////return View(articlePicture);
			return View();
		}




		// GET: ArticlePictures/Edit/5
		public async Task<IActionResult> Edit(long? id)
		{
            //if (id == null || _context.ArticlePictures == null)
            //{
            //	return NotFound();
            //}

            //var articlePicture = await _context.ArticlePictures.FindAsync(id);
            //if (articlePicture == null)
            //{
            //	return NotFound();
            //}

            //ViewData["blobUrl"] = await _blobService.GetSasUriAsync_ImageView(articlePicture.BlobFileName, _azureBlobContainer);


            ////ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", articlePicture.UserID);
            //var articles = _context.ArticleMasters
            //.Where(x => x.Article != null && x.Article.Length >= 4)
            //.Select(x => x.Article)
            //.Distinct()
            //.ToList();

            //ViewData["Article"] = new SelectList(articles, "Article");

            //return View(articlePicture);

            return View();

        }

        // POST: ArticlePictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
		[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("PictureID,Article,FileName,BlobFileName,UserID,CreatedOn,ModifiedOn,Flag")] ArticlePicture articlePicture)
        public async Task<IActionResult> Edit(long id, [Bind("PictureID,Article,FileName,BlobFileName,UserID,CreatedOn,ModifiedOn,Flag")] Model articlePicture)
		{
            //if (id != articlePicture.PictureID)
            //{
            //	return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //	try
            //	{
            //		_context.Update(articlePicture);
            //		await _context.SaveChangesAsync();
            //	}
            //	catch (DbUpdateConcurrencyException)
            //	{
            //		if (!ArticlePictureExists(articlePicture.PictureID))
            //		{
            //			return NotFound();
            //		}
            //		else
            //		{
            //			throw;
            //		}
            //	}
            //	return RedirectToAction(nameof(Index));
            //}

            ////ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", articlePicture.UserID);

            //var articles = _context.ArticleMasters
            //.Where(x => x.Article != null && x.Article.Length >= 4)
            //.Select(x => x.Article)
            //.Distinct()
            //.ToList();

            //ViewData["Article"] = new SelectList(articles, "Article");

            //return View(articlePicture);

            return View();
        }


        [HttpGet]
		public async Task<IActionResult> ProductGallery()
		{
            //// You can initialize the model if necessary
            //var model = new ArticlePictureSearchByInputModel();
            //return View(model);
        
			return View();
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ProductGallery(ArticlePictureSearchByInputModel inputModel)
        public async Task<IActionResult> ProductGallery(string inputModel)
        {
			//if (string.IsNullOrWhiteSpace(inputModel.SearchValue))
			//{
			//	ModelState.AddModelError("SearchValue", "Search term cannot be empty.");
			//	return View(inputModel);
			//}

			//// Process the selected radio button value and text box input value
			//string selectedRadioButton = inputModel.SelectedOption;
			//string textBoxValue = inputModel.SearchValue;


			//// Retrieve the ArticlePicture data
			////inputModel.ArticlePictures = await _context.ArticlePictures.Where(x => x.Article.Contains(textBoxValue, StringComparison.OrdinalIgnoreCase)).ToListAsync();

			//inputModel.ArticlePictures = await _context.ArticlePictures
			//									.Where(x => EF.Functions.Like(x.Article, $"{textBoxValue}%"))
			//									.Include(a => a.ArticleMaster)
			//									.ToListAsync();



			//// list of blob file names from ArticlePictures
			//var blobFileNames = inputModel.ArticlePictures.OrderBy(x => x.Article).Select(ap => ap.BlobFileName).ToList();

			//try
			//{
			//	// Retrieve the URLs of the ArticlePicture data from Azure Blob Storage
			//	inputModel.ArticlePictureUrls = await _articlePictureService.GetArticlePictureUrlsAsync(blobFileNames);
			//}
			//catch (Exception ex)
			//{
			//	// Log or handle the error appropriately
			//	ModelState.AddModelError("", $"Error retrieving images: {ex.Message}");
			//}

			//return View(inputModel);
			
			return View();
		}


		// GET: ArticlePictures/Delete/5
		public async Task<IActionResult> Delete(long? id)
		{
            //if (id == null || _context.ArticlePictures == null)
            //{
            //	return NotFound();
            //}

            //var articlePicture = await _context.ArticlePictures
            //	.Include(a => a.ApplicationUser)
            //	.Include(a => a.ArticleMaster)
            //	.FirstOrDefaultAsync(m => m.PictureID == id);
            //if (articlePicture == null)
            //{
            //	return NotFound();
            //}

            //return View(articlePicture);
        
			return View();
        }

        // POST: ArticlePictures/Delete/5
        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
            //if (_context.ArticlePictures == null)
            //{
            //	return Problem("Entity set 'ApplicationDbContext.ArticlePictures'  is null.");
            //}
            //var articlePicture = await _context.ArticlePictures.FindAsync(id);
            //if (articlePicture != null)
            //{
            //	_context.ArticlePictures.Remove(articlePicture);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        
			return View();
        }

        private bool ArticlePictureExists(long id)
		{
            //return (_context.ArticlePictures?.Any(e => e.PictureID == id)).GetValueOrDefault();
            return true;
        }



        [HttpGet]
		public async Task<IActionResult> Download(string fileName)
		{
			var stream = await _blobService.DownloadAsync(fileName, _azureBlobContainer);

			string contentType = GetContentType(fileName);

			return File(stream, "application/pdf", fileName);
			//return File(stream, contentType, fileName);
		}


		[HttpGet]
		public async Task<IActionResult> OpenBlobInBrowser(string fileName)
		{
			try
			{
				//// Get the SAS URI for the blob file
				//var sasUri = await _blobService.GetSasUriAsync(fileName);

				//// Redirect to the SAS URI
				//return Redirect(sasUri);

				var stream = await _blobService.DownloadAsync(fileName, _azureBlobContainer);
				// Determine the content type of the file
				string contentType = GetContentType(fileName);

				return File(stream, contentType, fileName);

			}
			catch (Exception ex)
			{
				// Log or handle the error appropriately
				return Content($"An error occurred: {ex.Message}");
			}

		}

		// Method to determine the content type based on file extension
		private string GetContentType(string fileName)
		{
			string extension = Path.GetExtension(fileName).ToLowerInvariant();

			switch (extension)
			{
				case ".pdf":
					return "application/pdf";
				case ".png":
					return "image/png";
				case ".jpg":
				case ".jpeg":
					return "image/jpeg";
				case ".gif":
					return "image/gif";
				// Add more cases for other image formats if needed
				default:
					// Default to application/octet-stream for unknown types
					return "application/octet-stream";
			}
		}


		[HttpGet]
		public async Task<IActionResult> Index(DateTime? dateTime, string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		{
			//var invoicesQuery = _context.ArticlePictures
			//	.Include(x => x.ApplicationUser)
			//	.Include(x => x.ArticleMaster)
			//	.AsQueryable();

			//ViewData["CurrentSort"] = sortOrder;
			//ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			//ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
			//ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;

			//if (searchString != null)
			//{
			//	pageNumber = 1;
			//}
			//else
			//{
			//	searchString = currentFilter;
			//}

			//ViewData["CurrentFilter"] = searchString;

			//// Apply date filter if dateTime is provided
			////if (dateTime.HasValue)
			////{
			////	var startDate = dateTime.Value.Date;
			////	var endDate = startDate.AddDays(1).AddTicks(-1); // End of the selected day
			////	invoicesQuery = invoicesQuery.Where(x => x.CreatedOn >= startDate && x.CreatedOn <= endDate);
			////	ViewData["DateTime"] = startDate.ToString("yyyy-MM-dd");
			////}
			////else
			////{
			////	// If dateTime is null, don't apply any filtering
			////	ViewData["DateTime"] = "All"; // Indicate that all data is being displayed
			////}

			//// No need to apply filtering based on InvoiceDate if dateTime is null

			//// Apply search filter
			//if (!string.IsNullOrEmpty(searchString))
			//{
			//	searchString = searchString.ToLower(); // Convert search string to lower case for case-insensitive search
			//	invoicesQuery = invoicesQuery.Where(i =>
			//		i.Article.Contains(searchString) ||
			//		i.FileName.ToLower().Contains(searchString) ||
			//		i.CreatedOn.ToString().Contains(searchString) ||
			//		i.ModifiedOn.ToString().Contains(searchString)
			//	);
			//}

			//// Apply sorting
			//switch (sortOrder)
			//{
			//	case "date_desc":
			//		invoicesQuery = invoicesQuery.OrderByDescending(x => x.CreatedOn);
			//		break;
			//	default:
			//		invoicesQuery = invoicesQuery.OrderBy(x => x.CreatedOn);
			//		break;
			//}

			//// Create a paginated list based on the query
			//var paginatedInvoices = await PaginatedList<ArticlePicture>.CreateAsync(invoicesQuery.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10);

			//return View(paginatedInvoices);
			return View();
		}
	}
}
