using Microsoft.AspNetCore.Mvc;

namespace CIITStudentApp.Controllers
{
    public class DesignController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
