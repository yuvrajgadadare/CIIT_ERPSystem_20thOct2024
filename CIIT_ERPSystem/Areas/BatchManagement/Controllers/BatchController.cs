
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CIIT_ERPSystem.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]
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
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            BatchModel b=new BatchModel();

            SelectList topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");
            SelectList trainers = new SelectList(batchService.GetAllTrainers(), "trainer_id", "trainer_name");
            ViewBag.topics = topics;
            ViewBag.trainers = trainers;
            ViewBag.batches = batchService.GetAllBatches();
            return View(b);
        }
        [HttpPost]
        public IActionResult Index(BatchModel batch)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            batchService.AddBatch(batch);
            ModelState.Clear();
            ViewBag.msg = "Batch Created Successfully";
            BatchModel b = new BatchModel();

            SelectList topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");
            SelectList trainers = new SelectList(batchService.GetAllTrainers(), "trainer_id", "trainer_name");
            ViewBag.topics = topics;
            ViewBag.trainers = trainers;
            ViewBag.batches = batchService.GetAllBatches();

            return View(b);
        }

        [HttpPost]
        public string GenerateBatchSchedule(int id)
            {
            BatchModel batch = batchService.GetBatch(id);
            List<ContentModel> contents = masterService.GetTopicWiseContents(batch.topic_id);
            DateTime dt =batch.start_date;
            foreach (ContentModel content in contents)
            {
               
                BatchScheduleModel bs = new BatchScheduleModel() 
                {
                 batch_id=batch.batch_id,
                 content_id=content.content_id,
                  expected_date=dt.ToShortDateString()
                };
                batchService.AddBatchSchedule(bs);

                if(dt.DayOfWeek == DayOfWeek.Friday)
                {
                    dt = dt.AddDays(3);
                }
                else 
                {
                    dt = dt.AddDays(1);
                }
                

            }
            return "Schedule Generated Successfully";
        }
        public JsonResult GetTopicWiseStudents(int id)
        {
            List<TopicStudentModel> lst = batchService.GetTopicWiseStudents(id);
            return Json(lst);
        }
        public IActionResult ViewBatchSchedule(int id)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            List<BatchScheduleModel>schedule=batchService.GetBatchWiseSchedule(id);
            List<BatchStudentModel> students = batchService.GetBatchWiseStudents(id);
            BatchModel b = batchService.GetBatch(id);

            ViewBag.batch = b;
            ViewBag.students = students;
            return View(schedule);
        }

        [HttpPost]
        public string AddBatchStudents(BatchStudents b)
        {

            string registrations = b.registration_ids.Substring(1, b.registration_ids.Length - 1);
            string[] data = registrations.Split(",");
            foreach(string s in data) { 
            int reg_id=Convert.ToInt32(s);
                BatchStudentModel bsmodel = new BatchStudentModel() 
                {
                 batch_id=b.batch_id,
                  registration_id=reg_id
                };
                batchService.AddBatchStudent(bsmodel);

            }
            return "Students Added Successfully";
        }
    }
}
