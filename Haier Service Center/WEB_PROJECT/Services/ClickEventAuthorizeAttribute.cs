using ServicePlatform.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ServicePlatform.Services
{

    public class ClickEventAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public ClickEventAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        // Example of storing authorization data in middleware
        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var currentUser = await userManager.GetUserAsync(context.User);

			// If there is no logged-in user, skip the middleware logic
			if (currentUser == null)
			{
				await _next(context); // Continue processing the request pipeline
				return;
			}

			// If there is a logged-in user, proceed with the middleware logic
			var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

			// Extract controller and action name from the request
			var controllerName = context.Request.RouteValues["controller"]?.ToString();
			var actionName = context.Request.RouteValues["action"]?.ToString();


			// Check if the request is for areas/identity/pages/account
			if (controllerName?.StartsWith("Areas/Identity/Pages/Account", StringComparison.OrdinalIgnoreCase) == true 				
				||
				context.Request.Path.Value.Contains("/Identity/Account/Manage", StringComparison.OrdinalIgnoreCase) == true
				||
				context.Request.Path.Value.Contains("/Identity/Account/Logout", StringComparison.OrdinalIgnoreCase) == true
				||
				context.Request.Path.Value.Contains("AccessDenied", StringComparison.OrdinalIgnoreCase) == true
				)
			{
				// Skip further processing in the middleware
				await _next(context);
				return;
			}


			//if (currentUser == null)
			//{
			//	//// Handle the case where the user is not found
			//	//context.Response.StatusCode = 404;

			//	// Redirect the user to the "404" page
			//	context.Response.Redirect("/NotFound"); // Adjust the path as needed
			//	return;
			//}


			// Check if there is an authorization record for the current controller and action
			var authorizationRecord = await dbContext.ClickEventAuthorizations.Where(x => x.UserId == currentUser.Id &&
											x.ControllerName == controllerName &&
											x.ActionName == actionName).FirstOrDefaultAsync();


			// Perform authorization based on the record
			if (authorizationRecord == null && !context.Request.Path.Value.Contains("/AccessDenied", StringComparison.OrdinalIgnoreCase))
			{
				// Check if the current user is authorized
				// You can use context.User.Identity.Name to get the current user's ID or username
				// Perform additional authorization checks as needed

				//// If not authorized, return a 403 Forbidden response
				context.Response.StatusCode = 403;

				// If not authorized, redirect to the AccessDenied page
				//context.Response.Redirect("/AccessDenied");
				return;
				//await _next(context);
			}



			// If authorization is successful, continue with the request
			await _next(context);


        }


    }
}
