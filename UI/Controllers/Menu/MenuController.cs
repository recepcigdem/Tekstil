using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class MenuController : BaseController
    {
        public MenuController(IStringLocalizerFactory factory, IStringLocalizer<MenuController> localizer, ILogger<MenuController> logger, IWebHostEnvironment env) : base(factory, env)
        {
            _localizer = localizer;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
