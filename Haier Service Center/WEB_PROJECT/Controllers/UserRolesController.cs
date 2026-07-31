using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using ServicePlatform.Data;
using ServicePlatform.Enums;

namespace ServicePlatform.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]

    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


       
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = await GetUserRoles(user)
                };
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            //var result = await _userManager.RemoveFromRolesAsync(user, roles);
            try
            {
                //To removes roles of the user
                await RemoveFromRolesAsync(model, userId);
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions specific to database update issues
                // ex.Message contains details about the error
                // You might want to log the exception or take other appropriate actions.
                Console.WriteLine($"Error saving changes to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }


            //var result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            //if (!result.Succeeded)
            //{
            //    ModelState.AddModelError("", "Cannot add selected roles to user");
            //    return View(model);
            //}

            try
            {
                //To Add roles of the user
                await AddUserRolesAsync(model, userId);
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions specific to database update issues
                // ex.Message contains details about the error
                // You might want to log the exception or take other appropriate actions.
                Console.WriteLine($"Error saving changes to the database: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }



        private async Task RemoveFromRolesAsync(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "User Does Not Exists");
            }


            // Fetch the user roles from the context (assuming you have a context property named 'DbContext')
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            // Identify the roles to be removed
            var rolesToRemove = model.Select(vm => vm.RoleName).ToList();

            // Remove the identified roles from the user's roles
            foreach (var roleToRemove in rolesToRemove)
            {
                var roleToRemoveEntity = await _context.Roles
                    .FirstOrDefaultAsync(role => role.Name == roleToRemove);

                if (roleToRemoveEntity != null)
                {
                    var userRoleToRemove = userRoles.FirstOrDefault(ur => ur.RoleId == roleToRemoveEntity.Id);

                    if (userRoleToRemove != null)
                    {
                        _context.UserRoles.Remove(userRoleToRemove);
                    }
                }
            }

            // Save changes to the database

            await _context.SaveChangesAsync();
        }



        private async Task AddUserRolesAsync(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError("", "User Does Not Exist");
                return;
            }

            foreach (var viewModel in model.Where(vm => vm.Selected == true))
            {
                var role = await _roleManager.FindByNameAsync(viewModel.RoleName);

                if (role != null)
                {
                    // Check if the user is not already in the role
                    var userInRole = await _userManager.IsInRoleAsync(user, role.Name);

                    if (!userInRole)
                    {
                        // Manually add the user to the role by adding an entry to UserRoles
                        var userRole = new IdentityUserRole<string>
                        {
                            UserId = user.Id,
                            RoleId = role.Id
                        };

                        _context.UserRoles.Add(userRole);
                    }
                }
                else
                {
                    // Handle the case where the role doesn't exist
                    ModelState.AddModelError("", $"Role '{viewModel.RoleName}' does not exist");
                }
            }

            // Save changes to the database using DbContext
            await _context.SaveChangesAsync();

        }


    }
}