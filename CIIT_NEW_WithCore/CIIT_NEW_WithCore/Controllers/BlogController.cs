using Microsoft.AspNetCore.Mvc;

namespace CIIT_NEW_WithCore.Controllers
{
    public class BlogController : Controller
    {
        [Route("ciit-blogs")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
