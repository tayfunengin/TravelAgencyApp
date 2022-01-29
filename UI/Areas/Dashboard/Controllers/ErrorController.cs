using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class ErrorController : Controller
    {
        public IActionResult Index(Exception ex)
        {
            return View(ex);
        }
    }
}
