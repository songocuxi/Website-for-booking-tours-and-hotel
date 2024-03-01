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
    public class AdminTypeRoomsController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminTypeRoomsController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminTypeRooms
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<TypeRoom> lsTypeRooms = new List<TypeRoom>();

            lsTypeRooms = _context.TypeRooms
                .AsNoTracking()
                .OrderBy(x => x.TypeId)
                .Include(x => x.Hotel)
                .ToList();

            PagedList<TypeRoom> models = new PagedList<TypeRoom>(lsTypeRooms.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsTypeRooms.Count / (Double)pageSize);
            return View(models);
        }
        // GET: Admin/AdminTypeRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TypeRooms == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var typeRoom = await _context.TypeRooms
                .Include(t => t.Hotel)
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (typeRoom == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            return View(typeRoom);
        }

        // GET: Admin/AdminTypeRooms/Create
        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name");
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminTypeRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,HotelId,Description,Quantity")] TypeRoom typeRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", typeRoom.HotelId);
            return View(typeRoom);
        }

        // GET: Admin/AdminTypeRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TypeRooms == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var typeRoom = await _context.TypeRooms.FindAsync(id);
            if (typeRoom == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", typeRoom.HotelId);
            _notyfService.Information("Bạn đang thêm mới");
            return View(typeRoom);
        }

        // POST: Admin/AdminTypeRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,HotelId,Description,Quantity")] TypeRoom typeRoom)
        {
            if (id != typeRoom.TypeId)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeRoomExists(typeRoom.TypeId))
                    {
                        _notyfService.Warning("Vui lòng kiểm tra lại");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", typeRoom.HotelId);
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(typeRoom);
        }

        // GET: Admin/AdminTypeRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TypeRooms == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var typeRoom = await _context.TypeRooms
                .Include(t => t.Hotel)
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (typeRoom == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            return View(typeRoom);
        }

        // POST: Admin/AdminTypeRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TypeRooms == null)
            {
                return Problem("Entity set 'dbTravelAndHotelContext.TypeRooms'  is null.");
            }
            var typeRoom = await _context.TypeRooms.FindAsync(id);
            if (typeRoom != null)
            {
                _context.TypeRooms.Remove(typeRoom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeRoomExists(int id)
        {
          return _context.TypeRooms.Any(e => e.TypeId == id);
        }
    }
}
