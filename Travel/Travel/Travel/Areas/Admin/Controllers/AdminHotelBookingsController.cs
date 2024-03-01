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
    public class AdminHotelBookingsController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminHotelBookingsController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminHotelBookings
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<HotelBooking> lsHotelBookings = new List<HotelBooking>();

            lsHotelBookings = _context.HotelBookings
                .AsNoTracking()
                .Include(b => b.Hotel)
                .Include(b => b.Tour)
                .OrderBy(x => x.Hbid)
                .ToList();

            PagedList<HotelBooking> models = new PagedList<HotelBooking>(lsHotelBookings.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsHotelBookings.Count / (Double)pageSize);

            ViewData["CustomerId"] = new SelectList(_context.Accounts, "AccountId", "Fullname");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View(models);
        }

        // GET: Admin/AdminHotelBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HotelBookings == null)
            {
                return NotFound();
            }

            var hotelBooking = await _context.HotelBookings
                .Include(h => h.Hotel)
                .Include(h => h.Tour)
                .FirstOrDefaultAsync(m => m.Hbid == id);
            if (hotelBooking == null)
            {
                return NotFound();
            }

            return View(hotelBooking);
        }

        // GET: Admin/AdminHotelBookings/Create
        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Cost");
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminHotelBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelId,TourId,Date,Hbid")] HotelBooking hotelBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotelBooking);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", hotelBooking.HotelId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Cost", hotelBooking.TourId);
            return View(hotelBooking);
        }

        // GET: Admin/AdminHotelBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HotelBookings == null)
            {
                return NotFound();
            }

            var hotelBooking = await _context.HotelBookings.FindAsync(id);
            if (hotelBooking == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", hotelBooking.HotelId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Cost", hotelBooking.TourId);
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(hotelBooking);
        }

        // POST: Admin/AdminHotelBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,TourId,Date,Hbid")] HotelBooking hotelBooking)
        {
            if (id != hotelBooking.Hbid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelBookingExists(hotelBooking.Hbid))
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
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", hotelBooking.HotelId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Cost", hotelBooking.TourId);
            return View(hotelBooking);
        }

        // GET: Admin/AdminHotelBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HotelBookings == null)
            {
                return NotFound();
            }

            var hotelBooking = await _context.HotelBookings
                .Include(h => h.Hotel)
                .Include(h => h.Tour)
                .FirstOrDefaultAsync(m => m.Hbid == id);
            if (hotelBooking == null)
            {
                return NotFound();
            }

            return View(hotelBooking);
        }

        // POST: Admin/AdminHotelBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HotelBookings == null)
            {
                return Problem("Entity set 'dbTravelAndHotelContext.HotelBookings'  is null.");
            }
            var hotelBooking = await _context.HotelBookings.FindAsync(id);
            if (hotelBooking != null)
            {
                _context.HotelBookings.Remove(hotelBooking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelBookingExists(int id)
        {
          return _context.HotelBookings.Any(e => e.Hbid == id);
        }
    }
}
