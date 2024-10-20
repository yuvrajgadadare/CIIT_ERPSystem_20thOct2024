using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CIIT_NEW_WithCore.Controllers
{
    public class ContactController : Controller
    {
      
        [Route("contact-us")]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
