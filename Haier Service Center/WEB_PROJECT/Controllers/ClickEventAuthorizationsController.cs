using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using System.Drawing.Printing;

namespace ServicePlatform.Controllers
{
    public class ClickEventAuthorizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClickEventAuthorizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClickEventAuthorizations
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var applicationDbContext = _context.ClickEventAuthorizations.Include(p => p.ApplicationUser).AsQueryable();


            // Applying search filter if provided
            if (!string.IsNullOrEmpty(currentFilter))
            {
                applicationDbContext = applicationDbContext.Where(p => p.ApplicationUser.UserName.Contains(currentFilter) || p.ControllerName.Contains(currentFilter) || p.ActionName.Contains(currentFilter));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(x => x.ApplicationUser.UserName);
                    break;
                // Add more sorting cases if needed
                default:
                    applicationDbContext = applicationDbContext.OrderBy(x => x.ApplicationUser.UserName);
                    break;
            }

            var paginatedQuery = await PaginatedList<ClickEventAuthorization>.CreateAsync(applicationDbContext.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10);

            ViewBag.CurrentFilter = currentFilter; // Pass current filter value back to the view
            ViewBag.PageSize = pageSize; // Pass page size value back to the view

            return View(paginatedQuery);
        }


        // GET: ClickEventAuthorizations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ClickEventAuthorizations == null)
            {
                return NotFound();
            }

            var clickEventAuthorization = await _context.ClickEventAuthorizations
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clickEventAuthorization == null)
            {
                return NotFound();
            }

            return View(clickEventAuthorization);
        }

        // GET: ClickEventAuthorizations/Create
        public IActionResult Create()
        {
            var controllerNames = ControllerDiscovery.GetControllerType().ToList();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["ControllerName"] = new SelectList(controllerNames);
            return View();
        }

        // POST: ClickEventAuthorizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ControllerName,ActionName")] ClickEventAuthorization clickEventAuthorization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clickEventAuthorization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var controllerNames = ControllerDiscovery.GetControllerType().ToList();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", clickEventAuthorization.UserId);
            ViewData["ControllerName"] = new SelectList(controllerNames);

            return View(clickEventAuthorization);
        }

        // GET: ClickEventAuthorizations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ClickEventAuthorizations == null)
            {
                return NotFound();
            }
            var controllerNames = ControllerDiscovery.GetControllerType().ToList();

            var clickEventAuthorization = await _context.ClickEventAuthorizations.FindAsync(id);
            if (clickEventAuthorization == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", clickEventAuthorization.UserId);
            ViewData["ControllerName"] = new SelectList(controllerNames);
            return View(clickEventAuthorization);
        }

        // POST: ClickEventAuthorizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,ControllerName,ActionName")] ClickEventAuthorization clickEventAuthorization)
        {
            if (id != clickEventAuthorization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRecord = await _context.ClickEventAuthorizations
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (existingRecord == null)
                    {
                        return NotFound();
                    }

                    existingRecord.UserId = clickEventAuthorization.UserId;
                    existingRecord.ControllerName = clickEventAuthorization.ControllerName;
                    existingRecord.ActionName = clickEventAuthorization.ActionName;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClickEventAuthorizationExists(clickEventAuthorization.Id))
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
            var controllerNames = ControllerDiscovery.GetControllerType().ToList();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", clickEventAuthorization.UserId);
            ViewData["ControllerName"] = new SelectList(controllerNames);
            return View(clickEventAuthorization);
        }

        // GET: ClickEventAuthorizations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ClickEventAuthorizations == null)
            {
                return NotFound();
            }

            var clickEventAuthorization = await _context.ClickEventAuthorizations
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clickEventAuthorization == null)
            {
                return NotFound();
            }

            return View(clickEventAuthorization);
        }

        // POST: ClickEventAuthorizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ClickEventAuthorizations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ClickEventAuthorizations'  is null.");
            }
            var clickEventAuthorization = await _context.ClickEventAuthorizations.FindAsync(id);
            if (clickEventAuthorization != null)
            {
                _context.ClickEventAuthorizations.Remove(clickEventAuthorization);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClickEventAuthorizationExists(long id)
        {
          return (_context.ClickEventAuthorizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }


		[HttpGet]
		public IActionResult GetActionNames(string controllerName)
		{
			var actionNames = ControllerDiscovery.GetActionMethods(controllerName);
			return Json(actionNames);
		}
	}
}
