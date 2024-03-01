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
    public class AdminGiamGiasController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminGiamGiasController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }


        // GET: Admin/AdminGiamGias
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<GiamGia> lsGiamgias = new List<GiamGia>();

            lsGiamgias = _context.GiamGia
                .AsNoTracking()
                .OrderBy(x => x.Kmid)
                .ToList();

            PagedList<GiamGia> models = new PagedList<GiamGia>(lsGiamgias.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsGiamgias.Count / (Double)pageSize);
            return View(models);
        }


        // GET: Admin/AdminGiamGias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GiamGia == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var giamGia = await _context.GiamGia
                .FirstOrDefaultAsync(m => m.Kmid == id);
            if (giamGia == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(giamGia);
        }

        // GET: Admin/AdminGiamGias/Create
        public IActionResult Create()
        {
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminGiamGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Kmid,MaGiamGia,Ngay,Thang")] GiamGia giamGia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giamGia);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(giamGia);
        }

        // GET: Admin/AdminGiamGias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GiamGia == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var giamGia = await _context.GiamGia.FindAsync(id);
            if (giamGia == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            return View(giamGia);
        }

        // POST: Admin/AdminGiamGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Kmid,MaGiamGia,Ngay,Thang")] GiamGia giamGia)
        {
            if (id != giamGia.Kmid)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giamGia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiamGiaExists(giamGia.Kmid))
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
            return View(giamGia);
        }

        // GET: Admin/AdminGiamGias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GiamGia == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var giamGia = await _context.GiamGia
                .FirstOrDefaultAsync(m => m.Kmid == id);
            if (giamGia == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(giamGia);
        }

        // POST: Admin/AdminGiamGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GiamGia == null)
            {
                return Problem("Entity set 'dbTravelAndHotelContext.GiamGia'  is null.");
            }
            var giamGia = await _context.GiamGia.FindAsync(id);
            if (giamGia != null)
            {
                _context.GiamGia.Remove(giamGia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiamGiaExists(int id)
        {
          return _context.GiamGia.Any(e => e.Kmid == id);
        }
    }
}
