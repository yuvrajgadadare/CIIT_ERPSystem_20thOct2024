using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CIIT_ERPSystem.Controllers
{
    public class BatchController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        public BatchController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
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
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<BatchStudentModel> batches = batchService.GetStudentWiseBatches(student_id);

            return View(batches);
        }
    }
}
