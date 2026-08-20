using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Data;
using ServicePlatform.Models;

namespace ServicePlatform.Controllers
{
    public class ProcurementApprovalUserMappingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcurementApprovalUserMappingsController(ApplicationDbContext context)
        {
            _context = context;
        }

		// GET: ProcurementApprovalUserMappings
		public async Task<IActionResult> Index()
		{
			//var applicationDbContext = _context.ProcurementApprovalUserMappings.Include(p => p.ApplicationUser).Include(p => p.ProcurementApprovalMaster);
			//return View(await applicationDbContext.ToListAsync());


			////var results = await _context.ProcurementApprovalUserMappings.Include(p => p.ApplicationUser).Include(p => p.ProcurementApprovalMaster)
			////      //.OrderByDescending(x => x.Id)
			////      .ToListAsync();

			////return View(results);

			return View();
		}

		//// GET: ProcurementApprovalUserMappings/Details/5
		//public async Task<IActionResult> Details(long id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	var procurementApprovalUserMapping = await _context.ProcurementApprovalUserMappings
		//		.Include(p => p.ApplicationUser)
		//		.Include(p => p.ProcurementApprovalMaster)
		//		.FirstOrDefaultAsync(m => m.Id == id);
		//	if (procurementApprovalUserMapping == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(procurementApprovalUserMapping);
		//}

		//// GET: ProcurementApprovalUserMappings/Create
		//public IActionResult Create()
		//{
		//	ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
		//	ViewData["ApprovalMasterId"] = new SelectList(_context.ProcurementApprovalMasters, "Id", "ApprovalName");
		//	return View();
		//}

		//// POST: ProcurementApprovalUserMappings/Create
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("UserId,ApprovalMasterId")] ProcurementApprovalUserMapping procurementApprovalUserMapping)
		//{

		//	if (ModelState.IsValid)
		//	{
		//		_context.Add(procurementApprovalUserMapping);
		//		await _context.SaveChangesAsync();
		//		return RedirectToAction(nameof(Index));
		//	}
		//	ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", procurementApprovalUserMapping.UserId);
		//	ViewData["ApprovalMasterId"] = new SelectList(_context.ProcurementApprovalMasters, "Id", "ApprovalName", procurementApprovalUserMapping.ApprovalMasterId);
		//	return View(procurementApprovalUserMapping);
		//}

		//// GET: ProcurementApprovalUserMappings/Edit/5
		//public async Task<IActionResult> Edit(long id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	var procurementApprovalUserMapping = await _context.ProcurementApprovalUserMappings.FindAsync(id);
		//	if (procurementApprovalUserMapping == null)
		//	{
		//		return NotFound();
		//	}
		//	ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", procurementApprovalUserMapping.UserId);
		//	ViewData["ApprovalMasterId"] = new SelectList(_context.ProcurementApprovalMasters, "Id", "ApprovalName", procurementApprovalUserMapping.ApprovalMasterId);
		//	return View(procurementApprovalUserMapping);
		//}

		//// POST: ProcurementApprovalUserMappings/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,ApprovalMasterId")] ProcurementApprovalUserMapping procurementApprovalUserMapping)
		//{
		//	if (id != procurementApprovalUserMapping.Id)
		//	{
		//		return NotFound();
		//	}

		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			_context.Update(procurementApprovalUserMapping);
		//			await _context.SaveChangesAsync();
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!ProcurementApprovalUserMappingExists(procurementApprovalUserMapping.Id))
		//			{
		//				return NotFound();
		//			}
		//			else
		//			{
		//				throw;
		//			}
		//		}
		//		return RedirectToAction(nameof(Index));
		//	}
		//	ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", procurementApprovalUserMapping.UserId);
		//	ViewData["ApprovalMasterId"] = new SelectList(_context.ProcurementApprovalMasters, "Id", "ApprovalName", procurementApprovalUserMapping.ApprovalMasterId);
		//	return View(procurementApprovalUserMapping);
		//}

		//// GET: ProcurementApprovalUserMappings/Delete/5
		//public async Task<IActionResult> Delete(long id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	var procurementApprovalUserMapping = await _context.ProcurementApprovalUserMappings
		//		.Include(p => p.ApplicationUser)
		//		.Include(p => p.ProcurementApprovalMaster)
		//		.FirstOrDefaultAsync(m => m.Id == id);
		//	if (procurementApprovalUserMapping == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(procurementApprovalUserMapping);
		//}

		//// POST: ProcurementApprovalUserMappings/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(long id)
		//{
		//	var procurementApprovalUserMapping = await _context.ProcurementApprovalUserMappings.FindAsync(id);
		//	if (procurementApprovalUserMapping != null)
		//	{
		//		_context.ProcurementApprovalUserMappings.Remove(procurementApprovalUserMapping);
		//	}

		//	await _context.SaveChangesAsync();
		//	return RedirectToAction(nameof(Index));
		//}

		//private bool ProcurementApprovalUserMappingExists(long id)
		//{
		//	return _context.ProcurementApprovalUserMappings.Any(e => e.Id == id);
		//}

	}
}
