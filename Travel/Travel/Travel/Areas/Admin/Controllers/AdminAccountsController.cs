using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Travel.Areas.Admin.Models;
using Travel.Extension;
using Travel.Models;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly dbTravelAndHotelContext _context;
        public INotyfService _notyfService { get; }

        public AdminAccountsController(dbTravelAndHotelContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminAccounts
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Account> lsAccount = new List<Account>();

            lsAccount = _context.Accounts
                .AsNoTracking()
                .Include(b => b.Role)
                .OrderBy(x => x.AccountId)
                .ToList();

            PagedList<Account> models = new PagedList<Account>(lsAccount.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsAccount.Count / (Double)pageSize);
            return View(models);
        }

        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            return View(account);
        }

        // GET: Admin/AdminAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Description");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,Salt,Fullname,Phone,Avatar,RoleId")] Account account)
        {
            if (ModelState.IsValid && account.Username != null && account.Fullname != null && account.Password != null)
            {
                account.Salt = "123";
                _context.Add(account);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Description", account.RoleId);
            _notyfService.Information("Bạn đang tạo mới");
            return View(account);
        }

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Description", account.RoleId);
            _notyfService.Information("Bạn đang chỉnh sửa");
            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,Password,Salt,Fullname,Phone,Avatar,RoleId")] Account account)
        {
            if (id != account.AccountId)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid && account.Username != null && account.Fullname != null && account.Password != null)
            {
                try
                {
                    account.Salt = "123";
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        _notyfService.Warning("Vui lòng kiểm tra lại");
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Description", account.RoleId);
            _notyfService.Warning("Vui lòng kiểm tra lại");
            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }

            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                _notyfService.Warning("Vui lòng kiểm tra lại");
                return RedirectToAction(nameof(Index));
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return _context.Accounts.Any(e => e.AccountId == id);
        }

        [AllowAnonymous]
        [Route("/dang-nhap.html", Name = "Login")]
        public IActionResult Login()
        {
            var khID = HttpContext.Session.GetString("AccountId");
            if (khID != null) return RedirectToAction("Index", "Home", new { Area = "Admin" });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var kh = _context.Accounts.Include(x => x.Role)
                        .SingleOrDefault(x => x.Username.ToLower() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        _notyfService.Warning("Kiểm tra lại tài khoản");
                        return View(model);
                    }

                    //string pass = model.Password.Trim().ToMD5();
                    string pass = model.Password;

                    if (pass.Trim() != kh.Password.Trim())
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        _notyfService.Warning("Kiểm tra lại mật khẩu");
                        return View(model);
                    }

                    _context.Update(kh);
                    await _context.SaveChangesAsync();

                    var khID = HttpContext.Session.GetString("AccountId");

                    HttpContext.Session.SetString("AccountId", kh.AccountId.ToString());

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.Fullname),
                        new Claim("AccountId", kh.AccountId.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.Role1)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    _notyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("Login", "AdminAccounts", new { Area = "Admin" });
            }

            ViewBag.Error = "Vui lòng nhập thông tin đăng nhập lại";
            return View(model); ;
        }

        [AllowAnonymous]
        [Route("/dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                _notyfService.Warning("Bạn đã đăng xuất");
                return RedirectToAction("Login", "AdminAccounts", new { Area = "Admin" });
            }
            catch
            {
                _notyfService.Warning("Bạn đã đăng xuất");
                return RedirectToAction("Login", "AdminAccounts", new { Area = "Admin" });
            }
        }
    }
}
