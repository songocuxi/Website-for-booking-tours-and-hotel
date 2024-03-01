using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Travel.Models;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminBookingsController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminBookingsController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminBookings
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Booking> lsBookings = new List<Booking>();

            lsBookings = _context.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .OrderBy(x => x.BookingId)
                .ToList();

            PagedList<Booking> models = new PagedList<Booking>(lsBookings.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsBookings.Count / (Double)pageSize);

            ViewData["CustomerId"] = new SelectList(_context.Accounts, "AccountId", "Fullname");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View(models);
        }

        // GET: Admin/AdminBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Information("Chi tiết " + booking.BookingId.ToString());
            return View(booking);
        }

        // GET: Admin/AdminBookings/Create
        public IActionResult Create(int? id)
        {
            return View();
        }

        // POST: Admin/AdminBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id)
        {
            Booking booking = new Booking();

            booking.TourId = id;
            booking.Date = DateTime.Now;
            booking.Pay = false;
            booking.CustomerId = Int32.Parse(HttpContext.Session.GetString("AccountId")); 

            _context.Add(booking);
            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));


        }

        // GET: Admin/AdminBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Accounts, "AccountId", "Fullname", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", booking.TourId);
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(booking);
        }

        // POST: Admin/AdminBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,CustomerId,Date,Pay,BookingId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        _notyfService.Error("Đã xảy ra lỗi");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Accounts, "AccountId", "Fullname", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", booking.TourId);
            return View(booking);
        }

        // GET: Admin/AdminBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        // POST: Admin/AdminBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null) _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
