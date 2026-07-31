using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace ServicePlatform.Services
{
	public class CustomAuthorizeAttribute : TypeFilterAttribute
	{
		public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
		{

		}
	}

	public class CustomAuthorizeFilter : IAuthorizationFilter
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public CustomAuthorizeFilter(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
		{
			_userManager = userManager;
			_context = context;
			_configuration = configuration;
		}

		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			if (context.HttpContext.User.Identity.IsAuthenticated)
			{

                // Inside a controller action or a view
                string? useremail = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Retrieve the currently authenticated user
                var user = await _userManager.FindByEmailAsync(useremail);

                if (user == null)
                {
                    // Handle the case where the user is not found
                    context.Result = new ChallengeResult(); // Redirect to login page or perform other authentication action
                    return;
                }

                // Get the requested controller URL
                string? requestedUrl = context.HttpContext.Request.Path;
				

                // Query the database to check if the current user's role has permission for the requested URL
                string? currentUserRole = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

				// Query the database to check if currentUserRole has permission for requestedUrl
				bool isAuthorized = false;

				//if (!isAuthorized)
				//{

				//                var userPageAuthorizations = await _context.UserPageAuthorizations
				//    .Include(u => u.ApplicationUser)
				//	.Include(m => m.MenuItem)
				//	.Where(r => r.UserID ==  user.Id && r.MenuItem.Url == requestedUrl)
				//	.ToListAsync();

				//                if (userPageAuthorizations == null || !userPageAuthorizations.Any())
				//                {
				//                    context.Result = new ForbidResult(); // or any other appropriate action
				//                    return;
				//                }

				//            }
				return;
			}
			else
			{
				context.Result = new ChallengeResult(); // Redirect to login page or perform other authentication action
			}

		}


        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            // Synchronous wrapper method
            OnAuthorizationAsync(context).GetAwaiter().GetResult();
        }

    }


}
