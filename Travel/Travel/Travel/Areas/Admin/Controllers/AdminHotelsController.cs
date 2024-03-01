using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Travel.Models;
using Travel.Areas.Admin.Models;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminHotelsController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminHotelsController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminHotels
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Hotel> lsHotels = new List<Hotel>();

            lsHotels = _context.Hotels
                .AsNoTracking()
                .OrderBy(x => x.HotelId)
                .ToList();

            PagedList<Hotel> models = new PagedList<Hotel>(lsHotels.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsHotels.Count / (Double)pageSize);
            return View(models);
        }

        // GET: Admin/AdminHotels/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            HotelViewModel hotelVM = new HotelViewModel();

            hotelVM.lsHotel = new Hotel();
            hotelVM.lsImgHotel = new List<ImgHotel>();
            hotelVM.lsTypeRoom = new List<TypeRoom>();

            hotelVM.lsHotel = _context.Hotels
                .FirstOrDefault(m => m.HotelId == id);

            hotelVM.lsImgHotel = _context.ImgHotels
                .AsNoTracking()
                .Include(x => x.Hotel)
                .OrderBy(x=>x.ImgHotelId)
                .Where(x=>x.HotelId == id)
                .ToList();

            hotelVM.lsTypeRoom = _context.TypeRooms
                .AsNoTracking()
                .Include(x=>x.Hotel)
                .OrderBy(x=>x.TypeId)
                .Where(x=>x.HotelId == id)
                .ToList();

            if (hotelVM.lsHotel == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(hotelVM);
        }

        // GET: Admin/AdminHotels/Create
        public IActionResult Create()
        {
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminHotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelId,Name,Phone")] Hotel hotel)
        {
            if (ModelState.IsValid && hotel.Name != null && hotel.Phone.Length == 10)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(hotel);
        }

        // GET: Admin/AdminHotels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(hotel);
        }

        // POST: Admin/AdminHotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,Name,Phone")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid && hotel.Name != null && hotel.Phone.Length == 10)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
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
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(hotel);
        }

        // GET: Admin/AdminHotels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Admin/AdminHotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotels == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                if (await _context.HotelBookings.FirstOrDefaultAsync(x => x.HotelId == hotel.HotelId) != null)
                {
                    _notyfService.Error("Đã xảy ra lỗi");
                    return RedirectToAction(nameof(Index));
                }

                _context.Hotels.Remove(hotel);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
          return _context.Hotels.Any(e => e.HotelId == id);
        }
    }
}
