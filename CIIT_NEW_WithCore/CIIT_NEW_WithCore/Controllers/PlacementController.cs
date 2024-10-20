using Microsoft.AspNetCore.Mvc;

namespace CIIT_NEW_WithCore.Controllers
{
    public class PlacementController : Controller
    {
        [Route("out-students-placed-in-top-companies")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
