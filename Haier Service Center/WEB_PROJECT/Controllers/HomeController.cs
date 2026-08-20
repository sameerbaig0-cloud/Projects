using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Interface;
using ServicePlatform.Models;
using ServicePlatform.Services;
using System.Diagnostics;

namespace ServicePlatform.Controllers
{

    [Authorize]
    [AutoValidateAntiforgeryToken]
    [CustomAuthorize]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _emailService;


        public HomeController(ILogger<HomeController> logger, IEmailService emailService, ApplicationDbContext context)
        {
            _logger = logger;
            _emailService = emailService;
            _context = context;
        }

        public IActionResult Index()
        {
            var companyName = _context.CompanyInfos
                                     .Select(c => c.CompanyName) // replace "Name" with your column
                                     .FirstOrDefault();

            ViewBag.CompanyName = companyName; 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> SendEmail(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the form data (e.g., send an email)
                var subject = "New Contact Form Submission";
                var body = $"Name: {model.Name}<br>Email: {model.Email}<br>Message: {model.Message}";
                var toEmail = "system.administrator@baerindia.com"; // Replace with your recipient's email
                var ccEmail = "pmc@baerindia.com,system.administrator@baerindia.com";

               await _emailService.SendEmailAsync(toEmail, ccEmail, subject, body);

                // Redirect to a thank-you or confirmation page                return RedirectToAction("Thank You");
            }

            // If model validation fails, return to the contact form with error messages
            return View("Contact", model);
        }

    }
}