using Microsoft.AspNetCore.Mvc;

namespace CIIT_NEW_WithCore.Controllers
{
    public class CareerController : Controller
    {
        [Route("great-successful-career-with-ciit")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
