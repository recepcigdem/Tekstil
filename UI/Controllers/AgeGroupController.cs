using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Controllers
{
    public class AgeGroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
