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
    public class AdminRolesController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminRolesController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminRoles
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Role> lsRoles = new List<Role>();

            lsRoles = _context.Roles
                .AsNoTracking()
                .OrderBy(x => x.RoleId)
                .ToList();

            PagedList<Role> models = new PagedList<Role>(lsRoles.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsRoles.Count / (Double)pageSize);
            return View(models);
        }

        // GET: Admin/AdminRoles/Create
        public IActionResult Create()
        {
            _notyfService.Information("Bạn đang tạo mới");
            return View();
        }

        // POST: Admin/AdminRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,Role1,Description")] Role role)
        {
            if (ModelState.IsValid && role.Role1 != null && role.Description != null)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(role);
        }

        // GET: Admin/AdminRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(role);
        }

        // POST: Admin/AdminRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,Role1,Description")] Role role)
        {
            if (id != role.RoleId)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid && role.Role1 != null && role.Description != null)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
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
            return View(role);
        }

        // GET: Admin/AdminRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Success("Thành công");

            return View(role);
        }

        // POST: Admin/AdminRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                _notyfService.Error("Đã xảy ra lỗi");
                return RedirectToAction(nameof(Index));
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                if (await _context.Accounts.FirstOrDefaultAsync(x => x.RoleId == role.RoleId) != null)
                {
                    _notyfService.Error("Đã xảy ra lỗi");
                    return RedirectToAction(nameof(Index));
                }

                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
