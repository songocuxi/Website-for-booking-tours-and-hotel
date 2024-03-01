using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
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
    public class AdminToursController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminToursController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminTours
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Tour> lsTours = new List<Tour>();

            lsTours = _context.Tours
                .AsNoTracking()
                .OrderBy(x => x.TourId)
                .Include(x => x.Place)
                .ToList();

            PagedList<Tour> models = new PagedList<Tour>(lsTours.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsTours.Count / (Double)pageSize);
            return View(models);
        }

        // GET: Admin/AdminTours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var tour = await _context.Tours
                .Include(t => t.Place)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Information("Chi tiết " + tour.TourId.ToString());

            int sluong = _context.Bookings
                .AsNoTracking()
                .Where(x => x.TourId == id)
                .ToList().Count;

            ViewBag.sluong = sluong;
            return View(tour);
        }

        // GET: Admin/AdminTours/Create
        public IActionResult Create()
        {
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name");
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminTours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,StartTime,EndTime,Cost,Description,PlaceId")] Tour tour)
        {
            if (ModelState.IsValid && tour.Cost != null && tour.StartTime <= tour.EndTime)
            {
                _context.Add(tour);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name", tour.TourId);
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(tour);
        }

        // GET: Admin/AdminTours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name", tour.TourId);
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(tour);
        }

        // POST: Admin/AdminTours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,StartTime,EndTime,Cost,Description,PlaceId")] Tour tour)
        {
            if (id != tour.TourId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid && tour.Cost != null && tour.StartTime <= tour.EndTime)
            {
                try
                {
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.TourId))
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
            ViewData["PlaceId"] = new SelectList(_context.Places, "PlaceId", "Name", tour.TourId);
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(tour);
        }

        // GET: Admin/AdminTours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var tour = await _context.Tours
                .Include(t => t.Place)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(tour);
        }

        // POST: Admin/AdminTours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                if (await _context.Tours.FirstOrDefaultAsync(x => x.PlaceId == tour.TourId) != null)
                {
                    _notyfService.Error("Đã xảy ra lỗi");
                    return RedirectToAction(nameof(Index));
                }
                _context.Tours.Remove(tour);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.TourId == id);
        }
    }
}
