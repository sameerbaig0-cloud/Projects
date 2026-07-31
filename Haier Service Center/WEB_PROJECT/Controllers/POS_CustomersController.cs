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

namespace ServicePlatform.Controllers
{

    [Authorize]
    [AutoValidateAntiforgeryToken]
    [CustomAuthorize]

    public class POS_CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public POS_CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: POS_Customers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
        {
            int currentPageSize = pageSize ?? 10;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["PageSize"] = pageSize == 0 ? 10 : pageSize;
            ViewData["PageSize"] = currentPageSize;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var applicationDbContext = _context.POS_Customers.Include(p => p.POS_Invoices).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(x =>
                    x.FirstName.Contains(searchString) ||
                    x.LastName.Contains(searchString) ||
                    x.Email.Contains(searchString) ||
                    x.Phone.Contains(searchString) ||
                    x.Address.Contains(searchString) ||
                    x.City.Contains(searchString) ||
                    x.State.Contains(searchString) ||
                    x.ZipCode.Contains(searchString) ||
                    x.Country.Contains(searchString)
                );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(x => x.LastName);
                    break;
                // Add more sorting cases if needed
                default:
                    applicationDbContext = applicationDbContext.OrderBy(x => x.LastName);
                    break;
            }

            var paginatedCustomers = await PaginatedList<POS_Customers>.CreateAsync(applicationDbContext.AsNoTracking(), pageNumber ?? 1, currentPageSize);

            return View(paginatedCustomers);
        }

        // GET: POS_Customers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.POS_Customers == null)
            {
                return NotFound();
            }

            var pOS_Customers = await _context.POS_Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (pOS_Customers == null)
            {
                return NotFound();
            }

            return View(pOS_Customers);
        }

        // GET: POS_Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: POS_Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,Country,IsActive")] POS_Customers pOS_Customers)
        {
            if (ModelState.IsValid)
            {
                if (await _context.POS_Customers.AnyAsync(x => x.Email == pOS_Customers.Email))
                {
                    ModelState.AddModelError("Email", "This email already exists.");
                }

                if (await _context.POS_Customers.AnyAsync(x => x.Phone == pOS_Customers.Phone))
                {
                    ModelState.AddModelError("Phone", "This phone number already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return View(pOS_Customers);
                }

                _context.Add(pOS_Customers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pOS_Customers);
        }

        // GET: POS_Customers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.POS_Customers == null)
            {
                return NotFound();
            }

            var pOS_Customers = await _context.POS_Customers.FindAsync(id);
            if (pOS_Customers == null)
            {
                return NotFound();
            }
            return View(pOS_Customers);
        }

        // POST: POS_Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerID,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,Country,IsActive")] POS_Customers pOS_Customers)
        {
            if (id != pOS_Customers.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pOS_Customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!POS_CustomersExists(pOS_Customers.CustomerID))
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
            return View(pOS_Customers);
        }

        // GET: POS_Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.POS_Customers == null)
            {
                return NotFound();
            }

            var pOS_Customers = await _context.POS_Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (pOS_Customers == null)
            {
                return NotFound();
            }

            return View(pOS_Customers);
        }

        // POST: POS_Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.POS_Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.POS_Customers'  is null.");
            }
            var pOS_Customers = await _context.POS_Customers.FindAsync(id);
            if (pOS_Customers != null)
            {
                _context.POS_Customers.Remove(pOS_Customers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool POS_CustomersExists(long id)
        {
          return (_context.POS_Customers?.Any(e => e.CustomerID == id)).GetValueOrDefault();
        }
    }
}
