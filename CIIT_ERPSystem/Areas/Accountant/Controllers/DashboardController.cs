using Microsoft.AspNetCore.Mvc;

namespace CIIT_ERPSystem.Areas.Accountant.Controllers
{
    [Area(areaName:"Accountant")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
