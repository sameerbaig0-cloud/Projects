using ServicePlatform.Data;
using ServicePlatform.Interface;
using ServicePlatform.Models;
using ServicePlatform.Services;
using Humanizer.Localisation.NumberToWords;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServicePlatform.Controllers
{
	[Authorize]
	[AutoValidateAntiforgeryToken]
	[CustomAuthorize]
	public class POSSalesController : Controller
	{

		private readonly ILogger<POSSalesController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;


		public POSSalesController(ILogger<POSSalesController> logger, ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_context = context;
			_configuration = configuration;
			_userManager = userManager;
		}


		// GET: POSSales/Index
		public async Task<IActionResult> Index(DateTime? dateTime, string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
		{
			var invoicesQuery = _context.POS_Invoices
				.Include(x => x.POS_Customer)
				.Include(x => x.POS_PaymentType)
				.Include(x => x.POS_InvoiceItems)
				.AsQueryable();

			ViewData["CurrentSort"] = sortOrder;
			ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

			// Apply date filter if dateTime is provided
			if (dateTime.HasValue)
			{
				invoicesQuery = invoicesQuery.Where(x => x.InvoiceDate.Date == dateTime.Value.Date);
				ViewData["DateTime"] = dateTime.Value.ToString("yyyy-MM-dd");
			}
			else
			{
				// If dateTime is null, don't apply any filtering
				ViewData["DateTime"] = "All"; // Indicate that all data is being displayed
			}

			// No need to apply filtering based on InvoiceDate if dateTime is null


			// Apply search filter
			if (!string.IsNullOrEmpty(currentFilter))
			{
				invoicesQuery = invoicesQuery.Where(i =>
					i.POS_Customer.FirstName.Contains(currentFilter) ||
					i.POS_Customer.Phone.Contains(currentFilter) ||
					i.InvoiceID.ToString().Contains(currentFilter) ||
					i.POS_PaymentType.Name.Contains(currentFilter) ||
					i.InvoiceDate.ToString().Contains(currentFilter)
				);
			}

			// Apply sorting
			switch (sortOrder)
			{
				case "date_desc":
					invoicesQuery = invoicesQuery.OrderByDescending(x => x.InvoiceDate);
					break;
				default:
					invoicesQuery = invoicesQuery.OrderBy(x => x.InvoiceDate);
					break;
			}

			// Create a paginated list based on the query
			var paginatedInvoices = await PaginatedList<POS_Invoice>.CreateAsync(invoicesQuery.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10);

			return View(paginatedInvoices);
		}



		// GET: POSSales/CreateAll
		public async Task<IActionResult> Create()
		{
			// Fetch the list of payment types from your data source
			ViewData["PaymentType"] = new SelectList(await _context.POS_PaymentTypes.Where(e => e.IsActive == true).AsQueryable().ToListAsync(), "PaymentTypeID", "Name");  // This should return a list of payment types

			var viewModel = new POS_Sales
			{
				Customer = new POS_Customers(),
				Invoice = new POS_Invoice(),
				InvoiceItems = new List<POS_InvoiceItem>(),
				SubTotal = 0.0m,
				DiscountPercentage = 0.0m, // Default value
										   //TaxPercentage = 0.0m, // Default value
				NetTotal = 0.0m
			};

			return View(viewModel);
		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> CreateAsync(POS_Customers viewModel)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var customer = await _context.POS_Customers.FirstOrDefaultAsync(c => c.Phone == viewModel.Phone);

		//		if (customer == null)
		//		{
		//			// Save customer
		//			_context.POS_Customers.Add(viewModel);
		//			_context.SaveChanges();
		//		}
		//		else
		//		{
		//			// Phone number is available
		//			// Proceed with your logic, such as saving the customer information

		//		}

		//		customer = await _context.POS_Customers.FirstOrDefaultAsync(c => c.Phone == viewModel.Customer.Phone);

		//		// Save invoice
		//		viewModel.Invoice.CustomerID = viewModel.Customer.CustomerID;
		//		viewModel.Invoice.PaymentTypeID = viewModel.PaymentType.PaymentTypeID;
		//		_context.POS_Invoices.Add(viewModel.Invoice);
		//		_context.SaveChanges();

		//		// Save invoice items
		//		foreach (var item in viewModel.InvoiceItems)
		//		{
		//			item.InvoiceID = viewModel.Invoice.InvoiceID;
		//			_context.POS_InvoiceItems.Add(item);
		//		}
		//		_context.SaveChanges();

		//		return RedirectToAction(nameof(Index));
		//	}
		//	return View(viewModel);
		//}


		public async Task<List<POS_Customers>> GetCustomerInformation(string mobileNumber)
		{
			var list = await _context.POS_Customers
				.Where(c => c.Phone == mobileNumber)
				.Select(n => new POS_Customers
				{
					CustomerID = n.CustomerID,
					FirstName = n.FirstName,
					LastName = n.LastName,
					Phone = n.Phone,
					Email = n.Email,
					Address = n.Address,
					City = n.City,
					Country = n.Country,
					State = n.State,
					ZipCode = n.ZipCode,
					IsActive = n.IsActive

				}).ToListAsync();


			if (list.Count == 0)
			{
				// No customer details found, add model error
				ModelState.AddModelError(string.Empty, "No customer details found for the provided phone number.");
			}

			await GetSalesScannedDetails(mobileNumber);

			return list;
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Insertcustomerinformation(string mobileNumber, string firstName, string lastName, string address, string mailAddress)
		{
			var existingCustomer = await _context.POS_Customers.FirstOrDefaultAsync(c => c.Phone == mobileNumber);
			if (existingCustomer == null)
			{
				var customer = new POS_Customers
				{
					Phone = mobileNumber,
					Address = address,
					FirstName = firstName,
					LastName = lastName,
					Email = mailAddress,
					Country = "India",
					State = "Tamil Nadu",
					ZipCode = "",
					City = "",
					IsActive = true
				};
				// Add the new customer to the database
				_context.POS_Customers.Add(customer);
				await _context.SaveChangesAsync();

				return Ok(); // Return a success response

				// Return a success message or redirect to another action
				//return RedirectToAction("Index", "Home"); // Adjust the action and controller name as needed
			}
			else
			{
				// Customer with the given mobile number already exists, add a model error
				ModelState.AddModelError(string.Empty, "Customer with this mobile number already exists.");
				//return BadRequest(ModelState); // Return a bad request response with model errors

				return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
			}
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> POSSalesScanSP(string mobileNumber, string data)
		{

            var existingCustomer = await _context.POS_Customers.FirstOrDefaultAsync(c => c.Phone == mobileNumber);
			if (existingCustomer == null)
			{
				ModelState.AddModelError(string.Empty, "Customer with this mobile number does not exists.");
				//return BadRequest(ModelState); // Return a bad request response with model errors

				return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
			}

			// Count the number of semicolons in the data
			int semicolonCount = data.Count(c => c == ';');

			// Check if the last digit is 'M', 'F', or 'U'
			char lastChar = data.LastOrDefault();

			if (semicolonCount != 7)
			{
				if (lastChar != 'M' || lastChar != 'F' || lastChar != 'U')
				{

					ModelState.AddModelError(string.Empty, "Customer with this mobile number does not exists.");
					//return BadRequest(ModelState); // Return a bad request response with model errors

					return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
				}
			}

			// Split the string by semicolons
			string[] parts = data.Split(';');
			long trackingID = Convert.ToInt64(parts[4].Substring(5, 11));

			// Parse the string to a suitable type (e.g., long) before comparison
			if (trackingID > 0)
			{
				//var viewData_checkTrackingID = await _context.POS_SalesScanningSummary.Where(f => f.TrackingID == trackingID).ToListAsync();
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Unable to Parse Tracking ID");
				//return BadRequest(ModelState); // Return a bad request response with model errors

				return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
			}

			// Create output parameter
			var resultParameter = new SqlParameter
			{
				ParameterName = "@Result",
				SqlDbType = SqlDbType.Int,
				Direction = ParameterDirection.Output
			};

			string param = string.Concat(existingCustomer.CustomerID, ';', data);
            // Call the stored procedure with @SCANNEDDATA parameter
            await _context.Database.ExecuteSqlRawAsync("EXEC POS_SalesScanningSP @Result OUTPUT, @p0", new object[] { resultParameter, param });

			// Check the result
			int result = (int)resultParameter.Value;
			if (result == 1)
			{
				// Insertion was successful
				// Your success logic here
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Unable to Process the Scanned Data");
				//return BadRequest(ModelState); // Return a bad request response with model errors

				return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

				// Insertion failed
				// Your failure logic here
			}

			return Ok();

			//return Ok(viewData);
		}



		public async Task<IActionResult> GetSalesScannedDetails(string mobileNumber)
		{
			var existingCustomer = await _context.POS_Customers.FirstOrDefaultAsync(c => c.Phone == mobileNumber);
			if (existingCustomer == null)
			{
				ModelState.AddModelError(string.Empty, "Customer with this mobile number does not exist.");
				return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
			}

			//var viewData = await _context.POS_SalesScanningSummary
			//							  .FromSqlRaw("SELECT * FROM POS_SalesScanningVw Where CustomerID='" + existingCustomer.CustomerID + "'")
			//							  .ToListAsync();
			var viewData = "";

			// Return the partial view with the data
			return PartialView("_POS_SalesScanningSummaryTable", viewData);
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteSalesItem(long id)
		{
			var itemToDelete = await _context.POS_SalesScannings.FindAsync(id);
			if (itemToDelete == null)
			{
				return NotFound();
			}

			_context.POS_SalesScannings.Remove(itemToDelete);
			await _context.SaveChangesAsync();

			return Ok();
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GenerateInvoice(string mobileNumber, string paymentType, decimal discountPercentage, decimal taxPercentage)
		{

			// Inside a controller action or a view
			var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			//HttpContext.Response.Redirect("/AccessDenied");

			// Retrieve the currently authenticated user
			var user = await _userManager.FindByEmailAsync(useremail);


			if (user == null)
			{   // Handle the case where the user is not found
				return Json(new { success = false, message = "User not found" });
			}


			try
			{
				await _context.Database.ExecuteSqlRawAsync("EXEC POS_GenerateInvoice @p0, @p1, @p2, @p3, @p4", user.Id, mobileNumber, paymentType, discountPercentage, taxPercentage);

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error generating invoice for user {UserId}", user.Id);
				return Json(new { success = false, message = "Internal server error" });
			}


		}



		// GET: POSSales/PrintInvoice
		public async Task<IActionResult> PrintInvoice(long id)
		{
			////var posInvoice = await _context.POS_Invoices.Where(x => x.InvoiceID == id).FirstOrDefaultAsync();

			//// Instantiate NumberToWordsConverter
			//var _numberToWordsConverter = new NumberToWordsConverter();


			//var posInvoice = await _context.POS_Invoices
			//	.Where(x => x.InvoiceID == id)
			//	.Include(p => p.POS_PaymentType)
			//	.Include(c => c.POS_Customer)
			//	.Include(s => s.POS_RetailStore)
			//	.Include(a => a.ApplicationUser)
			//	.Include(i => i.POS_InvoiceItems)
			//		.ThenInclude(tx => tx.POS_Taxation)
			//	.Include(i => i.POS_InvoiceItems)
			//			.ThenInclude(am => am.ArticleMaster)
			//			.ThenInclude(ab => ab.ArticleBrandModel)
			//		.FirstOrDefaultAsync();


			//if (posInvoice == null)
			//{
			//	return NotFound();
			//}


			//var companyInfo = await _context.CompanyInfos.FirstOrDefaultAsync();
			//var posRetailStore = posInvoice.POS_RetailStore;
			//var posPaymentType = posInvoice.POS_PaymentType;
			//var posCustomer = posInvoice.POS_Customer;
			//var posUserInfo = posInvoice.ApplicationUser;
			//var posInvoiceItems = posInvoice.POS_InvoiceItems; //await _context.POS_InvoiceItems.Where(x => x.InvoiceID == id).ToListAsync();

			//// Get the first ArticleMaster from the POS_InvoiceItems
			//var articleMaster = posInvoiceItems.Select(ii => ii.ArticleMaster).FirstOrDefault();
			//var articleBrand = articleMaster?.ArticleBrandModel;

			//var posTaxation = posInvoiceItems.Select(tx => tx.POS_Taxation).FirstOrDefault();
			//var subTotalValue = posInvoiceItems.Sum(s => s.UnitPrice * s.Quantity);
			//var discountValue = Math.Round(posInvoiceItems.Sum(s => (s.UnitPrice)/100 * s.DiscountPercentage), 2);
			//var amountInWords = _numberToWordsConverter.ConvertAmountToWords(posInvoice.TotalAmount);



			//// Summarize taxation information
			//var taxationSummary = posInvoiceItems.Select(item => new TaxationSummaryViewModel
			//{
			//	HSNCode = item.POS_Taxation.HSNCode,
			//	HSNDesc = item.POS_Taxation.HSNDescription,
			//	TaxableValue = Math.Round(item.TaxableValue, 2),
			//	CGSTPercent = item.POS_Taxation.CGSTPercentage,
			//	CGSTValue = Math.Round((item.TaxableValue * item.POS_Taxation.CGSTPercentage) / 100, 2),
			//	SGSTPercent = item.POS_Taxation.SGSTPercentage,
			//	SGSTValue = Math.Round((item.TaxableValue * item.POS_Taxation.SGSTPercentage) / 100, 2)
			//}).ToList();



			//var taxationSummaryViewModel = taxationSummary
			//	.GroupBy(item => new { item.HSNCode, item.HSNDesc, item.CGSTPercent, item.SGSTPercent })
			//	.Select(group => new TaxationSummaryViewModel
			//	{
			//		HSNCode = group.Key.HSNCode,
			//		HSNDesc = group.Key.HSNDesc,
			//		TaxableValue = Math.Round(group.Sum(item => item.TaxableValue), 0),
			//		CGSTPercent = group.Key.CGSTPercent,
			//		CGSTValue = Math.Round(group.Sum(item => item.CGSTValue),0),
			//		SGSTPercent = group.Key.SGSTPercent,
			//		SGSTValue = Math.Round(group.Sum(item => item.SGSTValue),0)
			//	})
			//	.ToList();



			//var viewModel = new POS_InvoiceModel
			//{
			//	POS_InvoiceID = id,
			//	CompanyInfo = companyInfo,
			//	POS_RetailStore = (POS_RetailStore)posRetailStore,
			//	POS_Customer = posCustomer,
			//	POS_Invoice = posInvoice,
			//	ApplicationUser = posUserInfo,
			//	POS_InvoiceItem = (List<POS_InvoiceItem>)posInvoiceItems,
			//	POS_PaymentType = posPaymentType,
			//	TaxationSummaryViewModels = taxationSummaryViewModel,
			//	SubTotalValue = subTotalValue,
			//	DiscountValue = discountValue,
			//	ArticleMaster = articleMaster,
			//	ArticleBrand = articleBrand,
			//	AmountInWords = amountInWords,
			//	AdditionalInformation = new List<string> {
			//	"MRP - Maximum Retail Price (Inclusive of all taxes)",
			//	"No Cash Refund",
			//	"All exchange/Complaints must be supported by Original Invoice & Product Tags",
			//	"Exchange allowed within 3 days from Date of purchase",
			//	"Exchange allowed Only for Size related issues",
			//	"No Exchange for article purchased at discounted price",
			//	"All disputes subject to exclusive jurisdiction of Ambur Courts only",
			//	"Purchase of carry bag is optional and not necessary/mandatory.",
			//	"UOM = Unit of Measurement" }, // Add actual additional info here

			//	Acknowledgements = new List<string> {
			//		"** Thank you for shopping with us **",
			//		"Save environment - Save trees - Say no to plastic/ paper carry bag",
			//		"*Kindly preserve the tax invoice for future reference",
			//		"*For any query, please get in touch with our customer service department",
			//		"Email: customercare@baerindia.com, Ph: (+91) 8489911200"} // Add actual acknowledgements here
			//};

			//return View(viewModel);

			return View();
		}




	}



}


