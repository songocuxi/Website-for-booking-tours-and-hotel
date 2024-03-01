using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Travel.Helpper;
using Travel.Models;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminImgHotelsController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminImgHotelsController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // GET: Admin/AdminImgHotels

        public IActionResult Index(int? id, int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<ImgHotel> lsImgHotel = new List<ImgHotel>();

            //if (hotelid == null)
            //{
            //    lsImgHotel = _context.ImgHotels
            //    .AsNoTracking()
            //    .OrderBy(x => x.ImgHotelId)
            //    .Include(x => x.Hotel)
            //    .ToList();
            //}
            //else

            if (id == null)
            {
                _notyfService.Error("Không được xem tất cả hình ảnh");
                return RedirectToAction("Index", "AdminHotels");
            }


            lsImgHotel = _context.ImgHotels
            .AsNoTracking()
            .OrderBy(x => x.ImgHotelId)
            .Include(x => x.Hotel)
            .Where(x => x.HotelId == id)
            .ToList();

            PagedList<ImgHotel> models = new PagedList<ImgHotel>(lsImgHotel.AsQueryable(), pageNumber, pageSize);

            ViewBag.Hotelid = id;

            Hotel hotel = _context.Hotels
               .FirstOrDefault(m => m.HotelId == id);

            ViewBag.NameHotel = hotel.Name;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsImgHotel.Count / (Double)pageSize);
            return View(models);
        }



        // GET: Admin/AdminImgHotels/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return Redirect("/admin/AdminImgHotels?id=" + id);
            }

            ViewBag.Hotelid = id;
            Hotel hotel = _context.Hotels
               .FirstOrDefault(m => m.HotelId == id);

            ViewBag.NameHotel = hotel.Name;
            return View();
        }

        // POST: Admin/AdminImgHotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            ImgHotel imgHotel = new ImgHotel();
            imgHotel.HotelId = id;

            if (fThumb != null)
            {
                string extension = Path.GetExtension(fThumb.FileName);
                string imagename = Path.GetRandomFileName();
                string image = imagename + extension;
                imgHotel.Description = await Utilities.UploadFile(fThumb, @"ImgHotels", image.ToLower());


                if (string.IsNullOrEmpty(imgHotel.Description)) imgHotel.Description = "default.jpg";

                _context.Add(imgHotel);
                await _context.SaveChangesAsync();
                return Redirect("/admin/AdminImgHotels?id=" + id);
            }

            _notyfService.Error("Đã xảy ra lỗi");
            ViewBag.Hotelid = id;
            Hotel hotel = _context.Hotels
               .FirstOrDefault(m => m.HotelId == id);

            ViewBag.NameHotel = hotel.Name;
            return View();
        }


        // POST: Admin/AdminImgHotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ImgHotels == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return Redirect("/admin/");
            }
            var imgHotel = await _context.ImgHotels.FindAsync(id);

            int hotelid = imgHotel.HotelId;

            if (imgHotel != null)
            {
                _context.ImgHotels.Remove(imgHotel);
            }

            await _context.SaveChangesAsync();
            return Redirect("/admin/AdminImgHotels?id=" + hotelid);
        }

        private bool ImgHotelExists(int id)
        {
            return _context.ImgHotels.Any(e => e.ImgHotelId == id);
        }
    }
}
