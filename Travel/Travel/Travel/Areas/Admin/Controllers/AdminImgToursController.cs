using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Travel.Helpper;
using Travel.Models;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminImgToursController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminImgToursController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminImgTours
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<ImgTour> lsImgTour = new List<ImgTour>();

            lsImgTour = _context.ImgTours
                .AsNoTracking()
                .OrderBy(x => x.ImgTourId)
                .Include(x => x.Tour)
                .ToList();

            PagedList<ImgTour> models = new PagedList<ImgTour>(lsImgTour.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsImgTour.Count / (Double)pageSize);
            return View(models);
        }


        // GET: Admin/AdminImgTours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ImgTours == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var imgTour = await _context.ImgTours
                .Include(i => i.Tour)
                .FirstOrDefaultAsync(m => m.ImgTourId == id);
            if (imgTour == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            return View(imgTour);
        }

        // GET: Admin/AdminImgTours/Create
        public IActionResult Create()
        {
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "ImgTourId");
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminImgTours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,ImgTourId,Description")] ImgTour imgTour, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imagename = Path.GetRandomFileName();
                    string image = imagename + extension;
                    imgTour.Description = await Utilities.UploadFile(fThumb, @"ImgTours", image.ToLower());
                }

                if (string.IsNullOrEmpty(imgTour.Description)) imgTour.Description = "default.jpg";

                _context.Add(imgTour);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error("Đã xảy ra lỗi");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", imgTour.TourId);
            return View(imgTour);
        }

        // GET: Admin/AdminImgTours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ImgTours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var imgTour = await _context.ImgTours.FindAsync(id);
            if (imgTour == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", imgTour.TourId);
            return View(imgTour);
        }

        // POST: Admin/AdminImgTours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,ImgTourId,Description")] ImgTour imgTour)
        {
            if (id != imgTour.ImgTourId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imgTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImgTourExists(imgTour.ImgTourId))
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", imgTour.TourId);
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(imgTour);
        }

        // GET: Admin/AdminImgTours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ImgTours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var imgTour = await _context.ImgTours
                .Include(i => i.Tour)
                .FirstOrDefaultAsync(m => m.ImgTourId == id);
            if (imgTour == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            return View(imgTour);
        }

        // POST: Admin/AdminImgTours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ImgTours == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            var imgTour = await _context.ImgTours.FindAsync(id);

           
            if (imgTour != null)
            {
                _context.ImgTours.Remove(imgTour);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImgTourExists(int id)
        {
          return _context.ImgTours.Any(e => e.ImgTourId == id);
        }
    }
}
