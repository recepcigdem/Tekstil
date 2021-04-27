using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UI.Models;

namespace UI.Controllers.Home
{
    public class HomeController : BaseController
    {

        public HomeController(IStringLocalizerFactory factory, IStringLocalizer<HomeController> localizer, ILogger<HomeController> logger, IWebHostEnvironment env) : base(factory, env)
        {
            _localizer = localizer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            MainPage model = new MainPage();

            var getUserSessior = Helpers.SessionHelper.GetStaff(Request);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Error()
        {
            return View();
        }
    }
}
