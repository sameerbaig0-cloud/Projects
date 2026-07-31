using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ServicePlatform.Data;
using ServicePlatform.Enums;
using ServicePlatform.Models;
using System.Security.Claims;

namespace ServicePlatform.ViewComponents
{
	public class MyMenuViewComponent : ViewComponent
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public MyMenuViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		//Synchronous method
		// GET: MenuItems

		public async Task<IViewComponentResult> InvokeAsync()
		{

			// Inside a controller action or a view
			var useremail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

			//HttpContext.Response.Redirect("/AccessDenied");

			// Retrieve the currently authenticated user
			var user = await _userManager.FindByEmailAsync(useremail);


			// Get UserMenuAccesses for the current user
			var userMenuAccesses = _context.UserMenuAccesses.Where(x => x.UserID == user.Id);

			// Extract ApplicationIds from UserMenuAccesses
			List<long> applicationIds = userMenuAccesses.Select(x => x.ApplicationNameId).ToList();

			long applicationId = userMenuAccesses.FirstOrDefault()?.ApplicationNameId ?? 0;


			// Filter MenuItemRoleMappings based on ApplicationIds 
			var menuItemRoleMappings = _context.MenuItemRoleMappings
				.Where(m => m.ApplicationNameId == applicationId)
				.Select(m => m.MenuItemId)
				.Distinct()
				.ToList();


            // Filter MenuItems based on the filtered MenuItemRoleMappings
            //        var menulist = _context.MenuItems
            // .Where(mi => menuItemRoleMappings.Contains(mi.Id)).OrderBy(mi => mi.MenuSorting)
            //.ToList();

            var menulist = await (
						from mi in _context.MenuItems
						join map in _context.MenuItemRoleMappings
							on mi.Id equals map.MenuItemId
						orderby mi.MenuSorting
						select mi
					)
					.Distinct()
					.OrderBy(x => x.MenuSorting)
					.ToListAsync();







            return View(menulist);
		}

		////Asynchronous method
		//public async Task<IViewComponentResult> InvokeAsync()
		//{
		//    return View();
		//}


	}
}
