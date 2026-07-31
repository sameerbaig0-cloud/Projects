using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using ServicePlatform.Services;

namespace ServicePlatform.Controllers
{
	[Authorize]
	[AutoValidateAntiforgeryToken]
    [CustomAuthorize]
	public class ProcurementApprovalMastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcurementApprovalMastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcurementApprovalMasters
        public async Task<IActionResult> Index()              
        {
            //var results = await _context.ProcurementApprovalMasters
            //       //.OrderByDescending(x => x.Id)
            //       .ToListAsync();

            //return View(results);
            return View();
        }

        //// GET: ProcurementApprovalMasters/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var procurementApprovalMaster = await _context.ProcurementApprovalMasters
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (procurementApprovalMaster == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(procurementApprovalMaster);
        //}

        //// GET: ProcurementApprovalMasters/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ProcurementApprovalMasters/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ApprovalStage,ApprovalName")] ProcurementApprovalMaster procurementApprovalMaster)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        procurementApprovalMaster.Id = Convert.ToString(Guid.NewGuid());
        //        procurementApprovalMaster.NormalizedApprovalName = procurementApprovalMaster.ApprovalName.Normalize();
        //        _context.Add(procurementApprovalMaster);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(procurementApprovalMaster);
        //}

        //// GET: ProcurementApprovalMasters/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var procurementApprovalMaster = await _context.ProcurementApprovalMasters.FindAsync(id);
        //    if (procurementApprovalMaster == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(procurementApprovalMaster);
        //}

        //// POST: ProcurementApprovalMasters/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,ApprovalStage,ApprovalName")] ProcurementApprovalMaster procurementApprovalMaster)
        //{
        //    if (id != procurementApprovalMaster.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(procurementApprovalMaster);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProcurementApprovalMasterExists(procurementApprovalMaster.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(procurementApprovalMaster);
        //}

        //// GET: ProcurementApprovalMasters/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var procurementApprovalMaster = await _context.ProcurementApprovalMasters
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (procurementApprovalMaster == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(procurementApprovalMaster);
        //}

        //// POST: ProcurementApprovalMasters/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var procurementApprovalMaster = await _context.ProcurementApprovalMasters.FindAsync(id);
        //    if (procurementApprovalMaster != null)
        //    {
        //        _context.ProcurementApprovalMasters.Remove(procurementApprovalMaster);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProcurementApprovalMasterExists(string id)
        //{
        //    return _context.ProcurementApprovalMasters.Any(e => e.Id == id);
        //}

    }
}
