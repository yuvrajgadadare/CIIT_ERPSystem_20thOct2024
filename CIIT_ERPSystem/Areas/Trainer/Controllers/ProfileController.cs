using ERPSystem_Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIIT_ERPSystem.Areas.Trainer.Controllers
{
    [Area("Trainer")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("trainer") == null)
            {
                return Redirect("/Account/Login");
            }
            string trainer = HttpContext.Session.GetString("trainer");
            TrainerModel tr = (TrainerModel)JsonConvert.DeserializeObject<TrainerModel>(trainer);
            ViewData["trainer"] = tr;
            return View();
        }
    }
}
