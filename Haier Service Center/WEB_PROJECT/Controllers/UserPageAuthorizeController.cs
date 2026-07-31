using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ServicePlatform.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserPageAuthorizeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public UserPageAuthorizeController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }


        // GET: UserPageAuthorize
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.UserPageAuthorizations
            //    .Include(u => u.ApplicationUser)
            //    .Include(u => u.MenuItem);
            var applicationDbContext = await (
                from u in _context.UserPageAuthorizations.Include(u => u.ApplicationUser)
                join mi in _context.MenuItems on u.MenuItemsID equals mi.Id into gj1
                from subMenu1 in gj1.DefaultIfEmpty()
                join me2 in _context.MenuItems on subMenu1.ParentId equals me2.Id into gj2
                from subMenu2 in gj2.DefaultIfEmpty()
                select new
                {
                    Authorization = u,
                    MenuItem = new MenuItem
                    {
                        Id = subMenu1 != null ? subMenu1.Id : 0,
                        Title = string.Concat(
                            !string.IsNullOrEmpty(subMenu2.Title) ? subMenu2.Title + "\\" : "",
                            !string.IsNullOrEmpty(subMenu1.Title) ? subMenu1.Title + "\\" : "",
                            subMenu1 != null ? subMenu1.Title : ""
                        )
                    }
                }
            ).ToListAsync();

            var userPageAuthorizations = applicationDbContext
             .Select(result => new UserPageAuthorization
             {
                 // Populate properties of UserPageAuthorization from the anonymous type
                 // Note: Make sure to adjust the property names accordingly
                 // For example:
                 ApplicationUser = result.Authorization.ApplicationUser,
                 ID = result.Authorization.ID,
                 MenuItemsID = result.Authorization.MenuItemsID,
                 IsAuthorized = result.Authorization.IsAuthorized,
                 MenuItem = result.MenuItem,
                 // Include other properties of UserPageAuthorization if necessary
             });

            return View(userPageAuthorizations.ToList());


        }


        // GET: UserPageAuthorize/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UserPageAuthorizations == null)
            {
                return NotFound();
            }

            var applicationDbContext = await (
              from u in _context.UserPageAuthorizations.Where(m => m.ID == id).Include(u => u.ApplicationUser)
              join mi in _context.MenuItems on u.MenuItemsID equals mi.Id into gj1
              from subMenu1 in gj1.DefaultIfEmpty()
              join me2 in _context.MenuItems on subMenu1.ParentId equals me2.Id into gj2
              from subMenu2 in gj2.DefaultIfEmpty()
              select new
              {
                  Authorization = u,
                  MenuItem = new MenuItem
                  {
                      Id = subMenu1 != null ? subMenu1.Id : 0,
                      Title = string.Concat(
                          !string.IsNullOrEmpty(subMenu2.Title) ? subMenu2.Title + "\\" : "",
                          !string.IsNullOrEmpty(subMenu1.Title) ? subMenu1.Title + "\\" : "",
                          subMenu1 != null ? subMenu1.Title : ""
                      )
                  }
              }
          ).ToListAsync();

            var userPageAuthorizations = applicationDbContext
             .Select(result => new UserPageAuthorization
             {
                 // Populate properties of UserPageAuthorization from the anonymous type
                 // Note: Make sure to adjust the property names accordingly
                 // For example:
                 ApplicationUser = result.Authorization.ApplicationUser,
                 ID = result.Authorization.ID,
                 MenuItemsID = result.Authorization.MenuItemsID,
                 IsAuthorized = result.Authorization.IsAuthorized,
                 MenuItem = result.MenuItem,
                 // Include other properties of UserPageAuthorization if necessary
             });


            if (userPageAuthorizations == null)
            {
                return NotFound();
            }

            return View(userPageAuthorizations.ToList());

        }


        // GET: UserPageAuthorize/Create
        public IActionResult Create()
        {

            var query = from mi in _context.MenuItems
                        join me in _context.MenuItems on mi.ParentId equals me.Id into gj1
                        from subMenu1 in gj1.DefaultIfEmpty()
                        join me2 in _context.MenuItems on subMenu1.ParentId equals me2.Id into gj2
                        from subMenu2 in gj2.DefaultIfEmpty()
                        select new
                        {
                            ID = mi.Id,
                            Title = string.Concat(
                            !string.IsNullOrEmpty(subMenu2.Title) ? subMenu2.Title + "\\" : "",
                            !string.IsNullOrEmpty(subMenu1.Title) ? subMenu1.Title + "\\" : "",
                            mi.Title)
                        };

            var result = query.ToList();

            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email");
            //ViewData["MenuItemsID"] = new SelectList(_context.MenuItems, "Id", "Title");

            ViewData["MenuItemsID"] = new SelectList(result, "ID", "Title");

            return View();
        }


        // POST: UserPageAuthorize/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,MenuItemsID,IsAuthorized")] UserPageAuthorization userPageAuthorization)
        {
            var query = from mi in _context.MenuItems
                        join me in _context.MenuItems on mi.ParentId equals me.Id into gj1
                        from subMenu1 in gj1.DefaultIfEmpty()
                        join me2 in _context.MenuItems on subMenu1.ParentId equals me2.Id into gj2
                        from subMenu2 in gj2.DefaultIfEmpty()
                        select new
                        {
                            ID = mi.Id,
                            Title = string.Concat(
                            !string.IsNullOrEmpty(subMenu2.Title) ? subMenu2.Title + "\\" : "",
                            !string.IsNullOrEmpty(subMenu1.Title) ? subMenu1.Title + "\\" : "",
                            mi.Title)
                        };

            var result = query.ToList();

            if (ModelState.IsValid)
            {
                // Check if the combination of UserID and MenuItemsID already exists
                var existingAuthorization = await _context.UserPageAuthorizations
                    .FirstOrDefaultAsync(x => x.UserID == userPageAuthorization.UserID && x.MenuItemsID == userPageAuthorization.MenuItemsID);

                if (existingAuthorization != null)
                {
                    ModelState.AddModelError("", "User authorization for this menu item already exists.");

                    ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", userPageAuthorization.UserID);
                    ViewData["MenuItemsID"] = new SelectList(result, "ID", "Title", userPageAuthorization.MenuItemsID);
                    return View(userPageAuthorization);
                }

                userPageAuthorization.ID = Convert.ToString(Guid.NewGuid());
                _context.Add(userPageAuthorization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", userPageAuthorization.UserID);
            ViewData["MenuItemsID"] = new SelectList(result, "ID", "Title", userPageAuthorization.MenuItemsID);
            return View(userPageAuthorization);
        }


        // GET: UserPageAuthorize/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.UserPageAuthorizations == null)
            {
                return NotFound();
            }

            var userPageAuthorization = await _context.UserPageAuthorizations.FindAsync(id);
            if (userPageAuthorization == null)
            {
                return NotFound();
            }

            var query = from mi in _context.MenuItems
                        join me in _context.MenuItems on mi.ParentId equals me.Id into gj1
                        from subMenu1 in gj1.DefaultIfEmpty()
                        join me2 in _context.MenuItems on subMenu1.ParentId equals me2.Id into gj2
                        from subMenu2 in gj2.DefaultIfEmpty()
                        select new
                        {
                            ID = mi.Id,
                            Title = string.Concat(
                            !string.IsNullOrEmpty(subMenu2.Title) ? subMenu2.Title + "\\" : "",
                            !string.IsNullOrEmpty(subMenu1.Title) ? subMenu1.Title + "\\" : "",
                            mi.Title)
                        };

            var result = query.ToList();

            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", userPageAuthorization.UserID);
            ViewData["MenuItemsID"] = new SelectList(result, "ID", "Title", userPageAuthorization.MenuItemsID);
            return View(userPageAuthorization);
        }


        // POST: UserPageAuthorize/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,UserID,MenuItemsID,IsAuthorized")] UserPageAuthorization userPageAuthorization)
        {
            if (id != userPageAuthorization.ID)
            {
                return NotFound();
            }

            var query = from mi in _context.MenuItems
                        join me in _context.MenuItems on mi.ParentId equals me.Id into gj1
                        from subMenu1 in gj1.DefaultIfEmpty()
                        join me2 in _context.MenuItems on subMenu1.ParentId equals me2.Id into gj2
                        from subMenu2 in gj2.DefaultIfEmpty()
                        select new
                        {
                            ID = mi.Id,
                            Title = string.Concat(
                            !string.IsNullOrEmpty(subMenu2.Title) ? subMenu2.Title + "\\" : "",
                            !string.IsNullOrEmpty(subMenu1.Title) ? subMenu1.Title + "\\" : "",
                            mi.Title)
                        };

            var result = query.ToList();


            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(userPageAuthorization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageAuthorizationExists(userPageAuthorization.ID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", userPageAuthorization.UserID);
            ViewData["MenuItemsID"] = new SelectList(result, "ID", "Title", userPageAuthorization.MenuItemsID);
            return View(userPageAuthorization);
        }


        // GET: UserPageAuthorize/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UserPageAuthorizations == null)
            {
                return NotFound();
            }

            var userPageAuthorization = await _context.UserPageAuthorizations
                .Include(u => u.ApplicationUser)
                .Include(u => u.MenuItem)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userPageAuthorization == null)
            {
                return NotFound();
            }

            return View(userPageAuthorization);
        }


        // POST: UserPageAuthorize/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UserPageAuthorizations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserPageAuthorizations'  is null.");
            }
            var userPageAuthorization = await _context.UserPageAuthorizations.FindAsync(id);
            if (userPageAuthorization != null)
            {
                _context.UserPageAuthorizations.Remove(userPageAuthorization);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool UserPageAuthorizationExists(string id)
        {
            return (_context.UserPageAuthorizations?.Any(e => e.ID == id)).GetValueOrDefault();
        }


    }
}
