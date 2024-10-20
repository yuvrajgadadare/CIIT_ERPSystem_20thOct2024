using Microsoft.AspNetCore.Mvc;

namespace CIIT_NEW_WithCore.Controllers
{
    public class AboutCIITController : Controller
    {
        [Route("about-ciit")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
