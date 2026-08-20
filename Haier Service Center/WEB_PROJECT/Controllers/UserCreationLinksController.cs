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
using ServicePlatform.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ServicePlatform.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [CustomAuthorize]
    public class UserCreationLinksController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserCreationLinksController> _logger;
        private readonly IEmailService _emailService;


        public UserCreationLinksController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<UserCreationLinksController> logger, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        // GET: UserCreationLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserCreationLinks.Include(u => u.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserCreationLinks/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.UserCreationLinks == null)
            {
                return NotFound();
            }

            var userCreationLink = await _context.UserCreationLinks
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.CreationID == id);
            if (userCreationLink == null)
            {
                return NotFound();
            }

            return View(userCreationLink);
        }

        // GET: UserCreationLinks/Create
        public IActionResult Create()
        {
            //ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: UserCreationLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreationModel userCreationModel)
        {
            if (ModelState.IsValid)
            {
                // Inside a controller action or a view
                var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                //HttpContext.Response.Redirect("/AccessDenied");

                // Retrieve the currently authenticated user
                var user = await _userManager.FindByEmailAsync(useremail);

                // Generate a new GUID
                var activationCode = Guid.NewGuid();

                var userCreationLink = new UserCreationLink
                {
                    Email = userCreationModel.Email,
                    ActivationCode = activationCode,
                    UserID = user.Id,
                    CreatedTime = DateTime.Now,
                    IsActive = true
                };
                _context.Add(userCreationLink);
                await _context.SaveChangesAsync();


                string linkforRegistration = @"http://103.76.191.220:196/Identity/Account/Register";

                var returnUrl = "/"; // Default return URL, can be set to any desired page

                // Construct the activation link with the activation code
                var callbackUrl = Url.Page(
                    "/Account/Register",
                    pageHandler: null,
                    values: new { area = "Identity", activationCode = userCreationLink.ActivationCode, returnUrl },
                    protocol: Request.Scheme);

                var subject = "Confirmation email for account activation";
                var body = $"Hi User! <br/><br/>Baer India invited you to create an account on its confidential portal PMC MIS Online which is registered to BaerIndia.<br/>Only authorized persons can access this portal.<br/><br/>Click <a href=\"{callbackUrl}\">here</a> to register your account.";
                var toEmail = userCreationLink.Email; // Replace with your recipient's email
                var ccEmail = "pmc@baerindia.com,system.administrator@baerindia.com";

                await _emailService.SendEmailAsync(toEmail, ccEmail, subject, body);


                return RedirectToAction(nameof(Index));
            }

            return View(userCreationModel);
        }

        // GET: UserCreationLinks/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.UserCreationLinks == null)
            {
                return NotFound();
            }

            var userCreationLink = await _context.UserCreationLinks.FindAsync(id);
            if (userCreationLink == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", userCreationLink.UserID);
            return View(userCreationLink);
        }

        // POST: UserCreationLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CreationID,Email,ActivationCode,UserID,CreatedTime,IsActive")] UserCreationLink userCreationLink)
        {
            if (id != userCreationLink.CreationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCreationLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCreationLinkExists(userCreationLink.CreationID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", userCreationLink.UserID);
            return View(userCreationLink);
        }

        // GET: UserCreationLinks/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.UserCreationLinks == null)
            {
                return NotFound();
            }

            var userCreationLink = await _context.UserCreationLinks
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.CreationID == id);
            if (userCreationLink == null)
            {
                return NotFound();
            }

            return View(userCreationLink);
        }

        // POST: UserCreationLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.UserCreationLinks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserCreationLinks'  is null.");
            }
            var userCreationLink = await _context.UserCreationLinks.FindAsync(id);
            if (userCreationLink != null)
            {
                _context.UserCreationLinks.Remove(userCreationLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCreationLinkExists(long id)
        {
            return (_context.UserCreationLinks?.Any(e => e.CreationID == id)).GetValueOrDefault();
        }
    }
}
