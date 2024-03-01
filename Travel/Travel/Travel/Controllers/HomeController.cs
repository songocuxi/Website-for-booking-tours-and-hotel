using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Travel.Models;

namespace Travel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		public INotyfService _notyfService { get; }

		public HomeController(ILogger<HomeController> logger, INotyfService notyfService)
        {
            _logger = logger;
			_notyfService = notyfService;
		}

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Tour()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
