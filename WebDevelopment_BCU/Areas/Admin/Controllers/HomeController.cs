﻿using Microsoft.AspNetCore.Mvc;

namespace WebDevelopment_BCU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
