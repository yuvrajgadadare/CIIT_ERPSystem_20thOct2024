using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CIIT_ERPSystem.Controllers
{
    public class PaymentController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        public PaymentController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return Redirect("/Student/Login");  
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<BatchStudentModel> batches = batchService.GetStudentWiseBatches(student_id);

            List<StudentPaymentModel> lst = studentService.GetStudentWisePayments(student_id);
            return View(lst);
        }

        public IActionResult ViewInvoice(int registration_id, int payment_id)
        {
            if (HttpContext.Session.GetString("student_id") == null)
            {
                return Redirect("/Student/Login");


            }
            StudentPaymentModel p = studentService.GetStudentPayment(payment_id);
            RegistrationModel r = studentService.GetRegistration(registration_id);
            ViewData["registration"] = r;
            ViewData["payments"] = studentService.GetStudentWisePreviousPayments(registration_id, payment_id);
            ViewData["student"] = studentService.GetStudent(r.student_id);
            return View(p);
        }
    }
}
