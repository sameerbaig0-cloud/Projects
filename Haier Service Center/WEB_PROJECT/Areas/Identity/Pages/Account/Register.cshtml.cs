using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ServicePlatform.Models;
using ServicePlatform.Interface;
using ServicePlatform.Enums;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;

namespace ServicePlatform.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<RegisterModel> logger,
            IEmailService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string ActivationCode { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, string activationCode = null)
        {

            if (activationCode == null || !Guid.TryParse(activationCode, out var activationGuid))
            {
                // Activation code not found in the database, handle the error
                return RedirectToAction("Index", "Home");
            }

            // Get the current time
            var currentTime = DateTime.Now;

            // Calculate the time 48 hours ago
            var thresholdTime = currentTime.AddHours(-48);


            // Find user link by activation code in the database
            var userLink = await _context.UserCreationLinks.Where(x => x.IsActive == true && x.CreatedTime >= thresholdTime).FirstOrDefaultAsync(x => x.ActivationCode == activationGuid);

            if (userLink == null)
            {
                //// Activation code not found in the database, handle the error
                //NotFound();
                //return;

                // Activation code not found in the database, return AccessDenied
                return Redirect("~/");
            }

            ActivationCode = activationCode; // Pass the activation code to the form
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                MailAddress address = new(Input.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);


                    await _emailSender.SendEmailAsync(Input.Email, "pmc@baerindia.com,system.administrator@baerindia.com", "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    // Deactivate the activation code
                    if (Guid.TryParse(ActivationCode, out var activationGuid))
                    {
                        var userLink = await _context.UserCreationLinks.FirstOrDefaultAsync(x => x.ActivationCode == activationGuid);
                        if (userLink != null)
                        {
                            userLink.IsActive = false;
                            _context.UserCreationLinks.Update(userLink);
                            await _context.SaveChangesAsync();
                        }
                    }


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
