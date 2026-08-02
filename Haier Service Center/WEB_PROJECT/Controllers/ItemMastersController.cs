using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using ServicePlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicePlatform.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [CustomAuthorize]
    public class ItemMastersController : Controller
    {

        private readonly ILogger<ItemMastersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobService _blobService;
        private readonly string _azureBlobContainer;

        public ItemMastersController(ILogger<ItemMastersController> logger, ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, BlobService blobService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _blobService = blobService;
        }

        // GET: ItemMasters
        public async Task<IActionResult> Index()
        {


            var applicationDbContext = _context.ItemMasters.Include(i => i.UnitMaster);
            return View(await applicationDbContext.ToListAsync());

        }


        // GET: ItemMasters/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemMaster = await _context.ItemMasters
                .Include(i => i.UnitMaster)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (itemMaster == null)
            {
                return NotFound();
            }

            return View(itemMaster);
        }

        // GET: ItemMasters/Create
        public IActionResult Create()
        {
            //ViewBag.StateList = _context.UnitMasters
            //.Select(x => x.UnitName.ToUpper())
            //.Distinct()
            //.OrderBy(x => x)
            //.Select(x => new SelectListItem
            //{
            //    Value = x,
            //    Text = x
            //}).ToList();
            //return View(new ItemMaster());


            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName");
            return View();

        }

        // POST: ItemMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ItemCode,ItemName,UnitID,IsActive")] ItemMaster itemMaster)
        {
            if (ModelState.IsValid)
            {
                itemMaster.CreatedDateTime = DateTime.Now;

                ModelState.Remove(nameof(itemMaster.CreatedDateTime));


                _context.Add(itemMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName", itemMaster.UnitID);
            return View(itemMaster);
        }

        // GET: ItemMasters/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemMaster = await _context.ItemMasters.FindAsync(id);
            if (itemMaster == null)
            {
                return NotFound();
            }
            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName", itemMaster.UnitID);
            return View(itemMaster);
        }

        // POST: ItemMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ItemID,ItemCode,ItemName,UnitID,IsActive")] ItemMaster itemMaster)
        {
            if (id != itemMaster.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemMasterExists(itemMaster.ItemID))
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
            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName", itemMaster.UnitID);
            return View(itemMaster);
        }

        // GET: ItemMasters/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemMaster = await _context.ItemMasters
                .Include(i => i.UnitMaster)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (itemMaster == null)
            {
                return NotFound();
            }

            return View(itemMaster);
        }

        // POST: ItemMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var itemMaster = await _context.ItemMasters.FindAsync(id);
            if (itemMaster != null)
            {
                _context.ItemMasters.Remove(itemMaster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemMasterExists(long id)
        {
            return _context.ItemMasters.Any(e => e.ItemID == id);
        }
    }
}
