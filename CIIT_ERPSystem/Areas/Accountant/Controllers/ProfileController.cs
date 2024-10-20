using ERPSystem_Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIIT_ERPSystem.Areas.Accountant.Controllers
{
    [Area( "Accountant")]

    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("employee")==null)
            {
                return Redirect("/Account/Login");
            }
            string e = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(e);
            return View(emp);
        }
    }
}
