using Microsoft.AspNetCore.Mvc;

namespace CIIT_ERPSystem.Controllers
{
    public class DesignController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
