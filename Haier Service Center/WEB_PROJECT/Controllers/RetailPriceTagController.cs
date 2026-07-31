using AspNetCore.Reporting;
using ServicePlatform.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Use Microsoft.Data.SqlClient instead of System.Data.SqlClient
using System;
using System.Data;
using System.IO;
using QRCoder;



namespace ServicePlatform.Controllers
{
    public class RetailPriceTagController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private object writer;

        public RetailPriceTagController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrintPriceTag(string ScannedData)
        {

            //    try
            //    {
            //        // Step 1: Parse ScannedData
            //        List<string> resultList = ScannedData.Split(';').Select(t => t.Trim()).ToList();
            //        decimal STrackingNo;
            //        decimal.TryParse(resultList[4].Substring(5), out STrackingNo);

            //        // Step 2: Fetch LabelNews records
            //        var labelNews = await _context.LabelNews.Where(x => x.TrackingID == STrackingNo).ToListAsync();

            //        // Step 3: Split details and trim
            //        List<string> resultLabelNews = labelNews
            //            .SelectMany(x => x.Detail.Split(new string[] { "   " }, StringSplitOptions.None))
            //            .Select(t => t.Trim())
            //            .ToList();

            //        // Step 4: Fetch TrailingOrderVw records
            //        var trailingOrderViews = await _context.TrailingOrderVw
            //            .Where(x => x.Article == resultLabelNews[0] && x.TrailingOrderNo == resultLabelNews[2])
            //            .ToListAsync();


            //        // Step 5: Prepare DataTables
            //        DataTable dataTable1 = new DataTable("DataSet1");
            //        DataTable dataTable2 = new DataTable("DataSet2");



            //        using (var connection = _context.Database.GetDbConnection())
            //        {
            //            if (connection.State == ConnectionState.Closed)
            //            {
            //                await connection.OpenAsync();
            //            }

            //            string _orderNo = trailingOrderViews.Select(t => t.OrderNo).First();
            //            string _article = trailingOrderViews.Select(t => t.Article).First();
            //            string _size = resultList[2].ToString();
            //            int _lotNo = int.Parse(resultList[6].Substring(resultList[6].Length - 4)[..2]);
            //            int _sequenceNo = int.Parse(resultList[6].Substring(resultList[6].Length - 2));


            //            // Step 6: Execute stored procedure and load DataTable1
            //            using (var command = connection.CreateCommand())
            //            {
            //                command.CommandText = "Exec labelPrintingDetails @OrderNo, @Article, @Size, @LotNo, @SequenceNo";
            //                command.CommandType = CommandType.Text;

            //                command.Parameters.Add(new SqlParameter("@OrderNo", _orderNo));
            //                command.Parameters.Add(new SqlParameter("@Article", _article));
            //                command.Parameters.Add(new SqlParameter("@Size", _size));
            //                command.Parameters.Add(new SqlParameter("@LotNo", _lotNo));
            //                command.Parameters.Add(new SqlParameter("@SequenceNo", _sequenceNo));


            //                using (var reader = await command.ExecuteReaderAsync())
            //                {
            //                    dataTable1.Load(reader);
            //                }
            //            }

            //            // Step 7: Execute raw SQL query and load DataTable2
            //            using (var command = connection.CreateCommand())
            //            {
            //                command.CommandText = "SELECT * FROM POS_RetailPrice WHERE IsActive=1 AND ArticleNo=@ArticleNo";
            //                command.CommandType = CommandType.Text;
            //                command.Parameters.Add(new SqlParameter("@ArticleNo", resultLabelNews[0]));

            //                using (var reader = await command.ExecuteReaderAsync())
            //                {
            //                    dataTable2.Load(reader);
            //                }
            //            }
            //        }

            //        // Step 8: Validate DataTable2
            //        if (dataTable2.Rows.Count == 0)
            //        {
            //            throw new Exception("DataSet2 is empty.");
            //        }

            //        // Step 9: Load RDLC report
            //        string reportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\PrintPriceTag.rdlc";
            //        int pixelSize = 1; // Adjust as needed
            //        byte[] qrCodeImageBytes = QRCodeHelper.GenerateQRCode(dataTable1.Rows[0]["QRCodeData"].ToString(), pixelSize);
            //        string qrCodeBase64 = Convert.ToBase64String(qrCodeImageBytes);

            //    Dictionary<string, string> parameters = new Dictionary<string, string>
            //    {
            //    { "ReportParameter1", "Label_Details" },
            //    { "ReportParameter2", "Retail_Price" },
            //    { "QRCodeBase64", qrCodeBase64 }
            //};

            //        LocalReport localReport = new LocalReport(reportPath);
            //        localReport.AddDataSource("DataSet1", dataTable1);
            //        localReport.AddDataSource("DataSet2", dataTable2);

            //        var result = localReport.Execute(RenderType.Pdf, 1, parameters, "");

            //        byte[] pdfBytes = result.MainStream.ToArray();

            //        return File(pdfBytes, "application/pdf");
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.ErrorMessage = "An error occurred while generating the report: " + ex.Message;
            //        return View("Index");
            //    }

            return View();
        }


    }
}




