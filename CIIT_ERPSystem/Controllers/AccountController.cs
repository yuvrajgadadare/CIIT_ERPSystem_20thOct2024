
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace CIIT_ERPSystem.Controllers
{
    public class  AccountController : Controller
    {
        IEmployeeService employeeService;
        ITrainerService trainerService;
        public AccountController(IEmployeeService employeeService, ITrainerService trainerService)
        {
            this.employeeService = employeeService;
            this.trainerService = trainerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string employee_code, string password)
        {
            EmployeeModel em = employeeService.CheckEmployeeLogin(employee_code, password);
            if (em != null)
            {
                if (em.email_address != null)
                {
                    HttpContext.Session.SetString("employee", JsonConvert.SerializeObject(em));
                    HttpContext.Session.SetString("employee_name", em.employee_name);
                    if (em.role_name == "Counsellor")
                    {
                        return Redirect("/Counsellor/Dashboard/Index");
                    }
                    else if (em.role_name == "SuperUser")
                    {
                        return Redirect("/BatchManagement/Dashboard/Index");
                    }
                    else if (em.role_name == "Accountant")
                    {
                        return Redirect("/Accountant/Dashboard/Index");
                    }
                    else if (em.role_name == "Developer")
                    {
                        return Redirect("/Developer/Dashboard/Index");
                    }
                    
                }
            }
            else
            {
                TrainerModel tr = trainerService.CheckTrainerLogin(employee_code, password);
                if (tr != null)
                {
                    HttpContext.Session.SetString("trainer", JsonConvert.SerializeObject(tr));
                    HttpContext.Session.SetString("trainer_name", tr.trainer_name);
                    HttpContext.Session.SetInt32("trainer_id", tr.trainer_id);
                    return Redirect("/Trainer/Dashboard/Index");
                }

                ViewBag.msg = "Invalid employee code/ email address or password";
                return View();


            }
            return View();

        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("trainer");
            HttpContext.Session.Remove("trainer_name");
            HttpContext.Session.Remove("trainer_id");
            HttpContext.Session.Remove("Employee");
            HttpContext.Session.Remove("employee_name");
            return RedirectToAction("Login");
        }
    }
}