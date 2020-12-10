using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WowAuctionnatorV2.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Form()
        {
            return View();
        }
    }
}
