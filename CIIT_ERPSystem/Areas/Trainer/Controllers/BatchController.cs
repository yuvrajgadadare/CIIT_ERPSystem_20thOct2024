
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIIT_ERPSystem.Areas.Trainer.Controllers
{
    [Area("Trainer")]
    public class BatchController : Controller
    {
        IBatchService batchService;
        IMasterService masterService;
        public BatchController(IBatchService batchService, IMasterService masterService)
        {
            this.batchService = batchService;
            this.masterService = masterService;
        }
        public IActionResult Index()
        {
           
            if (HttpContext.Session.GetString("trainer") == null)
            {
                return Redirect("/Account/Login");
            }
            string trainer = HttpContext.Session.GetString("trainer");
            TrainerModel tr = (TrainerModel)JsonConvert.DeserializeObject<TrainerModel>(trainer);
            ViewData["trainer"] = tr;

            List<BatchModel> lst = batchService.GetTrainerWiseBatches(tr.trainer_id);
            return View(lst);
        }

        public IActionResult Details(int id)
        {
            string trainer = HttpContext.Session.GetString("trainer");
            TrainerModel tr = (TrainerModel)JsonConvert.DeserializeObject<TrainerModel>(trainer);
            ViewData["trainer"] = tr;

            BatchModel b =batchService.GetBatch(id);
            ViewBag.batch = b;

            List<BatchScheduleModel> schedule = batchService.GetBatchWiseSchedule(id);
          
            List<BatchStudentModel> students = batchService.GetBatchWiseStudents(id);
            ViewBag.schedule = schedule;
            ViewBag.students = students;
            return View();
        }

        public JsonResult GetScheduleWiseData(int id)
        {
            BatchScheduleModel bs = batchService.GetScheduleWiseSchedule(id);
            return Json(bs);    
        }

        [HttpPost]
        public string MarkAttendance(ScheduleAttendanceModel sm)
        {
            batchService.MarkStudentScheduleAttendance(sm);

            return "Success";
        }

        public JsonResult GetBatchWiseStudentAtendance(int batch_id,int registration_id)
        {
            List<StudentMarkAttendance> lst=batchService.GetBatchWiseStudentAttendance(batch_id, registration_id);
            return Json(lst);
        }
    }
}
