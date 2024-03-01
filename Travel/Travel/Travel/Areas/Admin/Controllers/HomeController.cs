using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]

    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }

        public HomeController(INotyfService notyfService)
        {
            _notyfService = notyfService;
        }

        [Route("admin.dulich", Name = "Index")]
        public IActionResult Index()
        {
            var khID = HttpContext.Session.GetString("AccountId");

            if (khID == null)
            {
                _notyfService.Warning("Vui lòng đăng nhập lại");
                return RedirectToAction("Logout", "AdminAccounts", new { Area = "Admin" });
            }

            return View();
        }

        [AllowAnonymous]
        [Route("admin.loi", Name = "Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
