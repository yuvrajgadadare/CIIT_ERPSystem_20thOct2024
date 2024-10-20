using Microsoft.AspNetCore.Mvc;

namespace CIIT_NEW_WithCore.Controllers
{
    public class EventController : Controller
    {
        [Route("ciit-events")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
