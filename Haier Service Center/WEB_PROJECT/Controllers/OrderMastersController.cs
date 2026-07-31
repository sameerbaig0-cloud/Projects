using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using QRCoder;
using System.Drawing;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using Microsoft.Data.SqlClient;
using ServicePlatform.Services;
using Microsoft.AspNetCore.Authorization;

namespace ServicePlatform.Controllers
{
	[Authorize]
	[AutoValidateAntiforgeryToken]
	[CustomAuthorize]
	public class OrderMastersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OrderMastersController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: OrderMasters
		public async Task<IActionResult> Index()
		{
			//var orderMasters = await _context.OrderMaster.ToListAsync();
			//if (orderMasters == null)
			//{
			//	return NotFound();
			//}
			//return View(orderMasters);
			return View();
		}

		//// GET: OrderMasters/Details/5
		//public async Task<IActionResult> Details(decimal? id)
		//{
  //          //if (id == null || _context.OrderMaster == null)
  //          //{
  //          //	return NotFound();
  //          //}

  //          //var orderMaster = await _context.OrderMaster.FirstOrDefaultAsync(m => m.SNo == id);
  //          //if (orderMaster == null)
  //          //{
  //          //	return NotFound();
  //          //}

  //          //return View(orderMaster);
  //          return View();
  //      }

  //      // GET: OrderMasters/Create
  //      public IActionResult Create()
		//{
		//	return View();
		//}

		//// POST: OrderMasters/Create
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("SNo,OrderNo,OrderDate,DelDate,Article,Size,Pair,Origin,Position,OrderType,ProdStartDate,GDelDate,BSDETotalPair")] OrderMaster orderMaster)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_context.Add(orderMaster);
		//		await _context.SaveChangesAsync();
		//		return RedirectToAction(nameof(Index));
		//	}
		//	return View(orderMaster);
		//}

		//// GET: OrderMasters/Edit/5
		//public async Task<IActionResult> Edit(decimal? id)
		//{
  //          //if (id == null || _context.OrderMaster == null)
  //          //{
  //          //	return NotFound();
  //          //}

  //          //var orderMaster = await _context.OrderMaster.FindAsync(id);
  //          //if (orderMaster == null)
  //          //{
  //          //	return NotFound();
  //          //}
  //          //return View(orderMaster);
  //          return View();

  //      }

  //      // POST: OrderMasters/Edit/5
  //      [HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(decimal id, [Bind("SNo,OrderNo,OrderDate,DelDate,Article,Size,Pair,Origin,Position,OrderType,ProdStartDate,GDelDate,BSDETotalPair")] OrderMaster orderMaster)
		//{
		//	if (id != orderMaster.SNo)
		//	{
		//		return NotFound();
		//	}

		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			_context.Update(orderMaster);
		//			await _context.SaveChangesAsync();
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!OrderMasterExists(orderMaster.SNo))
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
		//	return View(orderMaster);
		//}

		//// GET: OrderMasters/Delete/5
		//public async Task<IActionResult> Delete(decimal? id)
		//{
  //          //if (id == null || _context.OrderMaster == null)
  //          //{
  //          //	return NotFound();
  //          //}

  //          //var orderMaster = await _context.OrderMaster.FirstOrDefaultAsync(m => m.SNo == id);
  //          //if (orderMaster == null)
  //          //{
  //          //	return NotFound();
  //          //}

  //          //return View(orderMaster);
  //          return View();

  //      }

  //      // POST: OrderMasters/Delete/5
  //      [HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(decimal id)
		//{
		//	//if (_context.OrderMaster == null)
		//	//{
		//	//	return Problem("Entity set 'ApplicationDbContext.OrderMaster'  is null.");
		//	//}
		//	//var orderMaster = await _context.OrderMaster.FindAsync(id);
		//	//if (orderMaster != null)
		//	//{
		//	//	_context.OrderMaster.Remove(orderMaster);
		//	//}

		//	//await _context.SaveChangesAsync();
		//	return RedirectToAction(nameof(Index));
		//}

		//private bool OrderMasterExists(decimal id)
		//{
		//	//return (_context.OrderMaster?.Any(e => e.SNo == id)).GetValueOrDefault();
		//	return true;

  //      }

  //      // GET: OrderMasters/OrderTrackingCodeGenerate
  //      public IActionResult OrderTrackingCodeGenerate()
		//{
		//	return View();
		//}

		//[HttpGet]
		//public async Task<IActionResult> GetOrderInformation(string orderNo)
		//{
			
		//	if (string.IsNullOrEmpty(orderNo))
		//	{
		//		return Json(null);
		//	}

		//	//var orders = await _context.OrderMaster
		//	//	.Where(c => c.OrderNo == orderNo)
		//	//	.Select(n => new { n.Article })
		//	//	.Distinct()
		//	//	.ToListAsync();

		//	var orders = "";
		//	return Json(orders);
		//}

		//[HttpGet]
		//public async Task<IActionResult> GetSizeInformation(string orderNo, string article)
		//{
		//	if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(article))
		//	{
		//		return Json(null);
		//	}

		//	//var sizes = await _context.OrderMaster
		//	//	.Where(c => c.OrderNo == orderNo && c.Article == article)
		//	//	.Select(n => new { n.Size })
		//	//	.Distinct()
		//	//	.ToListAsync();

		//	var sizes = "";
		//	return Json(sizes);
		//}


		//public async Task<IActionResult> GenerateQRCodes(string orderNo, string article, string size, string issize)
		//{
		//	try
		//	{
		//		bool issize_1 = bool.Parse(issize);
		//		// Validate inputs
		//		if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(article) || string.IsNullOrEmpty(size))
		//		{
		//			ViewBag.Error = "Invalid input parameters.";
		//			return View("Error");
		//		}


		//		// Call the stored procedure with parameters
		//		var trackingResults = _context.OrderMasterTrackingCodeQRCodes
		//.FromSqlRaw("EXEC OrderMasterTrackingCodeGen_SP @OrderNo, @Article, @Size, @IsSizeWise",
		//			new SqlParameter("@OrderNo", orderNo),
		//			new SqlParameter("@Article", article),
		//			new SqlParameter("@Size", size),
		//			new SqlParameter("@IsSizeWise", issize))
		//.AsEnumerable()  // Switch to client-side processing
		//.Select(r => new OrderMasterTrackingCodeQRCode
		//{
		//	OrderNo = r.OrderNo,  // Adjust these property mappings as per your stored procedure result
		//	Article = r.Article,
		//	ArticleNo = r.ArticleNo,
		//	ArticleColor = r.ArticleColor,
		//	Size = r.Size,
		//	StampingNo = r.StampingNo,
		//	Gender = r.Gender,
		//	QRCodeData = r.QRCodeData
		//})
		//.ToList();





		//		//           // Fetch the relevant tracking codes from your database
		//		//           var trackingCodes = await _context.OrderMaster
		//		//.Where(tc => tc.OrderNo == orderNo && tc.Article == article && tc.Size == size)
		//		//.ToListAsync();


		//		// Log the number of records found
		//		//Console.WriteLine($"Records Found: {trackingCodes.Count}");

		//		if (trackingResults == null || !trackingResults.Any())
		//		{
		//			ViewBag.Error = $"No tracking codes found for OrderNo: {orderNo}, Article: {article}, Size: {size}.";
		//			return View("Error");
		//		}

		//		// Prepare a list to hold the view model data
		//		List<OrderMasterTrackingCodeQRCode> qrCodeViewModels = new List<OrderMasterTrackingCodeQRCode>();

		//		// Generate QR codes for each tracking code entry
		//		List<string> qrCodeUrls = new List<string>();

		//		foreach (var trackingCode in trackingResults)
		//		{
		//			// Construct data string for QR code
		//			string data = trackingCode.QRCodeData.ToString();


  //                  // Generate QR code image
  //                  using (MemoryStream ms = new MemoryStream())
		//			{
		//				QRCodeGenerator qrGenerator = new QRCodeGenerator();
		//				QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.M);
		//				QRCode qrCode = new QRCode(qrCodeData);

		//				// Adjust the size and margin as per your requirement
		//				Bitmap qrCodeImage = qrCode.GetGraphic(20);

		//				// Save or convert QR code image
		//				qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
		//				byte[] qrCodeBytes = ms.ToArray();

		//				// Convert to data URL and store in list
		//				string qrCodeUrl = $"data:image/png;base64,{Convert.ToBase64String(qrCodeBytes)}";
  //                      //qrCodeUrls.Add(qrCodeUrl);

  //                      // Create view model instance and add to list
  //                      OrderMasterTrackingCodeQRCode viewModel = new OrderMasterTrackingCodeQRCode
  //                      {
		//					QRCodeData = qrCodeUrl,
		//					OrderNo = trackingCode.OrderNo,
		//					Article = trackingCode.Article,
		//					ArticleNo = trackingCode.ArticleNo,
		//					ArticleColor = trackingCode.ArticleColor,
		//					Size = trackingCode.Size,
		//					StampingNo = trackingCode.StampingNo,
  //                          Gender = trackingCode.Gender
  //                      };

		//				qrCodeViewModels.Add(viewModel);

		//			}
		//		}

		//		// Pass generated QR code URLs to the view
		//		//ViewBag.QRCodeImageUrls = qrCodeUrls;
		//		//ViewBag.OrderNumbers = trackingCodes.Select(tc => tc.OrderNo).ToList();
		//		//ViewBag.Article = trackingCodes.Select(tc => tc.Article).ToList();
		//		//ViewBag.Size = trackingCodes.Select(tc => tc.Size).ToList();

		//		//return Ok();

		//		return PartialView("_QRCodePartialView", qrCodeViewModels);  // Example if using partial view
		//	}
		//	catch (Exception ex)
		//	{
		//		ViewBag.Error = "Error generating QR codes: " + ex.Message;
		//		return View("Error");
		//	}
		//}






		////public async Task<IActionResult> GenerateQRCodes(string orderNo, string article, string size, string issize)
		////{
		////    try
		////    {
		////        bool isSizeWise = bool.Parse(issize);

		////        // Validate inputs
		////        if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(article) || string.IsNullOrEmpty(size))
		////        {
		////            ViewBag.Error = "Invalid input parameters.";
		////            return View("Error");
		////        }

		////        // Fetch the relevant tracking codes from your database
		////        var trackingCodes = await _context.OrderMaster
		////            .Where(tc => tc.OrderNo == orderNo && tc.Article == article && tc.Size == size)
		////            .ToListAsync();

		////        // Check if any tracking codes were found
		////        if (trackingCodes == null || !trackingCodes.Any())
		////        {
		////            ViewBag.Error = $"No tracking codes found for OrderNo: {orderNo}, Article: {article}, Size: {size}.";
		////            return View("Error");
		////        }

		////        // Pass generated QR code URLs to the view
		////        ViewBag.QRCodeImageUrls = trackingCodes.Select(tc => $"{tc.OrderNo};{tc.Article};{tc.Size}").ToList();
		////        return View("OrderTrackingCodeGenerate", trackingCodes);
		////    }
		////    catch (Exception ex)
		////    {
		////        ViewBag.Error = "Error generating QR codes: " + ex.Message;
		////        return View("Error");
		////    }
		////}




		////public IActionResult GenerateImage(string data)
		////{
		////    using (MemoryStream ms = new MemoryStream())
		////    {
		////        QRCodeGenerator qrGenerator = new QRCodeGenerator();
		////        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.M);
		////        QRCode qrCode = new QRCode(qrCodeData);

		////        // Adjust the margin by passing an additional parameter to GetGraphic method
		////        Bitmap qrCodeImage = qrCode.GetGraphic(20); // Adjust the size as per your requirement

		////        // Crop the image to remove excessive white space
		////        //Bitmap croppedImage = CropImage(qrCodeImage);


		////        qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

		////        return File(ms.ToArray(), "image/png"); // Returns the QR code image
		////    }
		////}



		////public IActionResult Generate(string data)
		////{

		////	// Sample list of strings
		////	List<string> dataList = new List<string>
		////	{
		////		"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520101;M",
		////		//"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520102;M",
		////		//"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520103;M",
		////		//"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520104;M",
		////		//"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520105;M",
		////		//"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520106;M",
		////		"1733;107;12.0;69752;I201900000610918;4062784003185;12V23697520107;M"
		////	};
		////	// Encode the list of strings into a single string
		////	string dataListString = string.Join(",", dataList);


		////	//ViewBag.QRCodeImageUrls = Url.Action("GenerateImage", "Home", new { data = dataListString });
		////	//return View("Index");

		////	ViewBag.QRCodeImageUrls = dataListString;
		////	return View("OrderTrackingCodeGenerate");
		////}




		////public IActionResult GenerateImage(string data)
		////{
		////	using (MemoryStream ms = new MemoryStream())
		////	{
		////		QRCodeGenerator qrGenerator = new QRCodeGenerator();
		////		QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.M);
		////		QRCode qrCode = new QRCode(qrCodeData);

		////		// Adjust the margin by passing an additional parameter to GetGraphic method
		////		Bitmap qrCodeImage = qrCode.GetGraphic(20); // Adjust the size as per your requirement

		////		// Crop the image to remove excessive white space
		////		//Bitmap croppedImage = CropImage(qrCodeImage);


		////		qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

		////		return File(ms.ToArray(), "image/png"); // Returns the QR code image
		////	}
		////}
	}
}


