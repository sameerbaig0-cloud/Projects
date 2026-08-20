using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServicePlatform.Data;
using ServicePlatform.Models;
using ServicePlatform.Services;
using System.Security.Claims;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServicePlatform.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [CustomAuthorize]
    public class ShowRoomsController : Controller
    {
        private readonly ILogger<ShowRoomsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobService _blobService;
        private readonly string _azureBlobContainer;

        public ShowRoomsController(ILogger<ShowRoomsController> logger, ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, BlobService blobService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _blobService = blobService;
        }

        // =========================
        // LIST
        // =========================
        public async Task<IActionResult> Index(DateTime? dateTime, string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
        {
            var dataQuery = _context.ShowRooms.AsQueryable();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
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

            // Apply date filter if dateTime is provided
            if (dateTime.HasValue)
            {
                var fromDate = dateTime.Value.Date;
                var toDate = fromDate.AddDays(1);

                dataQuery = dataQuery.Where(x =>
                    x.CreatedDate >= fromDate &&
                    x.CreatedDate < toDate);

                // dataQuery = dataQuery.Where(x => x.CreatedDate.Date == dateTime.Value.Date);
                ViewData["DateTime"] = dateTime.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                // If dateTime is null, don't apply any filtering
                ViewData["DateTime"] = "All"; // Indicate that all data is being displayed
            }

            // No need to apply filtering based on InvoiceDate if dateTime is null


            // Apply search filter
            if (!string.IsNullOrEmpty(currentFilter))
            {
                dataQuery = dataQuery.Where(i =>
                    i.Email.Contains(currentFilter) ||
                    i.Name.Contains(currentFilter) ||
                    i.Phone.ToString().Contains(currentFilter) ||
                    i.Address.Contains(currentFilter) ||
                    i.State.Contains(currentFilter) ||
                    i.GSTNumber.Contains(currentFilter) ||
                    i.AlternatePhone.Contains(currentFilter) ||
                    i.State.Contains(currentFilter) ||
                    i.CreatedDate.ToString().Contains(currentFilter)
                );
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "date_desc":
                    dataQuery = dataQuery.OrderByDescending(x => x.CreatedDate);
                    break;
                default:
                    dataQuery = dataQuery.OrderBy(x => x.CreatedDate);
                    break;
            }

            // Create a paginated list based on the query
            var paginatedInvoices = await PaginatedList<ShowRoom>.CreateAsync(dataQuery.AsNoTracking(), pageNumber ?? 1, pageSize ?? 10);

            return View(paginatedInvoices);
            //return View(data);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var showroom = await _context.ShowRooms
                .FirstOrDefaultAsync(m => m.ShowRoomId == id);

            if (showroom == null) return NotFound();

            return View(showroom);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {

            ViewBag.StateList = _context.Places
                .Select(x => x.State.ToUpper())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();

            //ViewBag.CityList = _context.Places
            //    .Select(x => x.Place)
            //    .Distinct()
            //    .OrderBy(x => x)
            //    .Select(x => new SelectListItem
            //    {
            //        Value = x,
            //        Text = x
            //    }).ToList();


            return View(new ShowRoom());
        }


        [HttpGet]
        public JsonResult GetDistrict(string state)
        {
            var district = _context.Places
                .Where(x => x.State == state)
                .Select(x => x.District.ToUpper())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();

            return Json(district);
        }


        [HttpGet]
        public JsonResult GetCity(string state, string district)
        {
            var city = _context.Places
                .Where(x => x.State == state && x.District == district)
                .Select(x => x.Place.ToUpper())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();

            return Json(city);
        }

        [HttpGet]
        public JsonResult GetZipCode(string state, string district, string city)
        {
            var zipcode = _context.Places
                .Where(x => x.State == state && x.District == district && x.Place == city)
                .Select(x => x.Zip.ToUpper())
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();

            return Json(zipcode);
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowRoom showRoom)
        {
            // Inside a controller action or a view
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Retrieve the currently authenticated user
            var user = await _userManager.FindByEmailAsync(useremail);


            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            showRoom.CreatedBy = user.AccountID.ToString();
            showRoom.CreatedDate = DateTime.Now;

            ModelState.Remove(nameof(showRoom.CreatedBy));
            ModelState.Remove(nameof(showRoom.CreatedDate));

            if (ModelState.IsValid)
            {
                // Duplicate checks
                if (await _context.ShowRooms.AnyAsync(x => x.Email == showRoom.Email))
                    ModelState.AddModelError("Email", "Email already exists");

                if (await _context.ShowRooms.AnyAsync(x => x.Phone == showRoom.Phone))
                    ModelState.AddModelError("Phone", "Phone already exists");

                if (!string.IsNullOrEmpty(showRoom.GSTNumber))
                {
                    if (await _context.ShowRooms.AnyAsync(x => x.GSTNumber == showRoom.GSTNumber))
                        ModelState.AddModelError("GSTNumber", "GST already exists");
                }

                if (ModelState.ErrorCount == 0)
                {
                    showRoom.CreatedDate = DateTime.Now;
                    showRoom.CreatedBy = user.AccountID.ToString();

                    _context.Add(showRoom);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(showRoom);
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var showroom = await _context.ShowRooms.FindAsync(id);
            if (showroom == null) return NotFound();

            return View(showroom);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShowRoom showRoom)
        {
            // Inside a controller action or a view
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Retrieve the currently authenticated user
            var user = await _userManager.FindByEmailAsync(useremail);


            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            if (id != showRoom.ShowRoomId) return NotFound();

            if (ModelState.IsValid)
            {
                // Duplicate checks (exclude current record)
                if (await _context.ShowRooms.AnyAsync(x => x.Email == showRoom.Email && x.ShowRoomId != id))
                    ModelState.AddModelError("Email", "Email already exists");

                if (await _context.ShowRooms.AnyAsync(x => x.Phone == showRoom.Phone && x.ShowRoomId != id))
                    ModelState.AddModelError("Phone", "Phone already exists");

                if (!string.IsNullOrEmpty(showRoom.GSTNumber))
                {
                    if (await _context.ShowRooms.AnyAsync(x => x.GSTNumber == showRoom.GSTNumber && x.ShowRoomId != id))
                        ModelState.AddModelError("GSTNumber", "GST already exists");
                }

                if (ModelState.ErrorCount == 0)
                {
                    try
                    {
                        showRoom.UpdatedDate = DateTime.Now;
                        showRoom.UpdatedBy = user.AccountID.ToString();

                        _context.Update(showRoom);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ShowRoomExists(showRoom.ShowRoomId))
                            return NotFound();
                        else
                            throw;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(showRoom);
        }

        // =========================
        // DELETE (GET)
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var showroom = await _context.ShowRooms
                .FirstOrDefaultAsync(m => m.ShowRoomId == id);

            if (showroom == null) return NotFound();

            return View(showroom);
        }

        // =========================
        // DELETE (POST) → SOFT DELETE
        // =========================
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Inside a controller action or a view
            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Retrieve the currently authenticated user
            var user = await _userManager.FindByEmailAsync(useremail);


            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            var showroom = await _context.ShowRooms.FindAsync(id);

            if (showroom != null)
            {
                showroom.IsDeleted = true;
                showroom.UpdatedDate = DateTime.Now;
                showroom.UpdatedBy = user.AccountID.ToString();

                _context.Update(showroom);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // HELPER
        // =========================
        private bool ShowRoomExists(int id)
        {
            return _context.ShowRooms.Any(e => e.ShowRoomId == id);
        }
    }
}
