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
    public class AdminLocation1Controller : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminLocation1Controller(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminLocation1
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Location1> lsLocations = new List<Location1>();

            lsLocations = _context.Location1s
                .AsNoTracking()
                .OrderBy(x => x.LocationId)
                .ToList();

            PagedList<Location1> models = new PagedList<Location1>(lsLocations.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsLocations.Count / (Double)pageSize);
            return View(models);
        }

        // GET: Admin/AdminLocation1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Location1s == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var location1 = await _context.Location1s
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location1 == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(location1);
        }

        // GET: Admin/AdminLocation1/Create
        public IActionResult Create()
        {
            ViewData["LocatiobID"] = new SelectList(_context.Location1s, "Levels", "Type","Name","Parent");
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminLocation1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,Levels,Type,Name,Parent")] Location1 location1)
        {
            if (ModelState.IsValid && location1.Type != null && location1.Name != null && location1.Parent != null)
            {
                _context.Add(location1);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(location1);
        }

        // GET: Admin/AdminLocation1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Location1s == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var location1 = await _context.Location1s.FindAsync(id);
            if (location1 == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            return View(location1);
        }

        // POST: Admin/AdminLocation1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,Levels,Type,Name,Parent")] Location1 location1)
        {
            if (id != location1.LocationId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid  && location1.Type != null && location1.Parent != null && location1.Name != null)
            {
                try
                {
                    _context.Update(location1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Location1Exists(location1.LocationId))
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
            return View(location1);
        }

        // GET: Admin/AdminLocation1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Location1s == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var location1 = await _context.Location1s
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location1 == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Success("Thành công");
            return View(location1);
        }

        // POST: Admin/AdminLocation1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Location1s == null)
            {
                return Problem("Entity set 'dbTravelAndHotelContext.Location1s'  is null.");
            }
            var location1 = await _context.Location1s.FindAsync(id);
            if (location1 != null)
            {
                _context.Location1s.Remove(location1);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool Location1Exists(int id)
        {
          return _context.Location1s.Any(e => e.LocationId == id);
        }
    }
}
