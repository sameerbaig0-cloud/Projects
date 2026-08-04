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
    public class POS_ItemMastersController : Controller
    {

        private readonly ILogger<POS_ItemMastersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobService _blobService;
        private readonly string _azureBlobContainer;

        public POS_ItemMastersController(ILogger<POS_ItemMastersController> logger, ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, BlobService blobService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _blobService = blobService;
        }

        // GET: POS_ItemMasters
        public async Task<IActionResult> Index()
        {


            var applicationDbContext = _context.POS_ItemMasters.Include(i => i.UnitMaster);
            return View(await applicationDbContext.ToListAsync());

        }


        // GET: POS_ItemMasters/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var POS_ItemMaster = await _context.POS_ItemMasters
                .Include(i => i.UnitMaster)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (POS_ItemMaster == null)
            {
                return NotFound();
            }

            return View(POS_ItemMaster);
        }

        // GET: POS_ItemMasters/Create
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
            //return View(new POS_ItemMaster());


            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName");
            return View();

        }

        // POST: POS_ItemMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ItemCode,ItemName,UnitID,IsActive")] POS_ItemMaster POS_ItemMaster)
        {
            if (ModelState.IsValid)
            {
                POS_ItemMaster.CreatedDateTime = DateTime.Now;

                ModelState.Remove(nameof(POS_ItemMaster.CreatedDateTime));


                _context.Add(POS_ItemMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName", POS_ItemMaster.UnitID);
            return View(POS_ItemMaster);
        }

        // GET: POS_ItemMasters/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var POS_ItemMaster = await _context.POS_ItemMasters.FindAsync(id);
            if (POS_ItemMaster == null)
            {
                return NotFound();
            }
            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName", POS_ItemMaster.UnitID);
            return View(POS_ItemMaster);
        }

        // POST: POS_ItemMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ItemID,ItemCode,ItemName,UnitID,IsActive")] POS_ItemMaster POS_ItemMaster)
        {
            if (id != POS_ItemMaster.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(POS_ItemMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!POS_ItemMasterExists(POS_ItemMaster.ItemID))
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
            ViewData["UnitID"] = new SelectList(_context.UnitMasters, "UnitID", "UnitName", POS_ItemMaster.UnitID);
            return View(POS_ItemMaster);
        }

        // GET: POS_ItemMasters/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var POS_ItemMaster = await _context.POS_ItemMasters
                .Include(i => i.UnitMaster)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (POS_ItemMaster == null)
            {
                return NotFound();
            }

            return View(POS_ItemMaster);
        }

        // POST: POS_ItemMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var POS_ItemMaster = await _context.POS_ItemMasters.FindAsync(id);
            if (POS_ItemMaster != null)
            {
                _context.POS_ItemMasters.Remove(POS_ItemMaster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool POS_ItemMasterExists(long id)
        {
            return _context.POS_ItemMasters.Any(e => e.ItemID == id);
        }
    }
}
