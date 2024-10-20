using Microsoft.AspNetCore.Mvc;

namespace CIIT_NEW_WithCore.Controllers
{
    public class CourseController : Controller
    {
        [Route("latest-training-with-live-project")]
        public IActionResult Index()
        {
            return View();
        }
        //[Route("Excel-Course")]
        //public IActionResult Excel()
        //{
        //    return View();
        //}
    }
}
