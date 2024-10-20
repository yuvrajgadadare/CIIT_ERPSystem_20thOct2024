using Microsoft.AspNetCore.Mvc;

namespace CIIT_ERPSystem.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
