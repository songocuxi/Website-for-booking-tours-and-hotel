using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Travel.Models;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminPlacesController : Controller
    {
        private readonly dbTravelAndHotelContext _context;

        public INotyfService _notyfService { get; }

        public AdminPlacesController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminPlaces
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Place> lsPlaces = new List<Place>();

            lsPlaces = _context.Places
                .AsNoTracking()
                .OrderBy(x => x.PlaceId)
                .Include(x => x.Location)
                .ToList();

            PagedList<Place> models = new PagedList<Place>(lsPlaces.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsPlaces.Count / (Double)pageSize);
            return View(models);
        }

        // GET: Admin/AdminPlaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Places == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var place = await _context.Places
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.PlaceId == id);
            if (place == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            return View(place);
        }

        // GET: Admin/AdminPlaces/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Location1s, "LocationId", "Name");
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminPlaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaceId,Name,Address,Image,LocationId")] Place place)
        {
            if (ModelState.IsValid && place.Name != null && place.LocationId != null)
            {
                _context.Add(place);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location1s, "LocationId", "Name", place.LocationId);
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(place);
        }

        // GET: Admin/AdminPlaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Places == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var place = await _context.Places.FindAsync(id);
            if (place == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location1s, "LocationId", "Name", place.LocationId);
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(place);
        }

        // POST: Admin/AdminPlaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaceId,Name,Address,Image,LocationId")] Place place)
        {
            if (id != place.PlaceId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid && place.Name != null && place.LocationId != null)
            {
                try
                {
                    _context.Update(place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(place.PlaceId))
                    {
                        _notyfService.Error("Đã xảy ra lỗi");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location1s, "LocationId", "Name", place.LocationId);
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(place);
        }

        // GET: Admin/AdminPlaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Places == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var place = await _context.Places
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.PlaceId == id);
            if (place == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Success("Thành công");
            return View(place);
        }

        // POST: Admin/AdminPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Places == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            var place = await _context.Places.FindAsync(id);



            if (place != null)
            {
                if (await _context.Tours.FirstOrDefaultAsync(x => x.PlaceId == place.PlaceId) != null)
                {
                    _notyfService.Error("Đã xảy ra lỗi");
                    return RedirectToAction(nameof(Index));
                }
                _context.Places.Remove(place);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceExists(int id)
        {
          return _context.Places.Any(e => e.PlaceId == id);
        }
    }
}

