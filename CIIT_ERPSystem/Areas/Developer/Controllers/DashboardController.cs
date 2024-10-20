﻿using Microsoft.AspNetCore.Mvc;

namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            return View();
        }
    }
}
