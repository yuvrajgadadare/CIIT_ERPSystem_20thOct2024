using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CIIT_ERPSystem.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]

    public class ExamController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IExtraService extraService;
        EmailSettings _settings;
        public ExamController(IMasterService masterService, IStudentService studentService, IExtraService extraService, IOptions<EmailSettings> settings)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.extraService = extraService;
            _settings = settings.Value;
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
            return View();
        }
        public IActionResult ScheduleExam()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            List<TopicModel> topics = masterService.GetTrainingTopics();
            //List<StudentModel> students = studentService.GetStudents();
            ViewBag.students = GetStudents();
            ViewBag.topics=new SelectList(topics,"topic_id","topic_name");
            List<ExamModel> examlist = masterService.ViewAllScheduleExams();
            foreach (ExamModel ex in examlist)
            {
                string examtime = ex.exam_date.ToLongDateString() + " " + ex.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt<= DateTime.Now)
                {
                    masterService.RejectScheduledExam(ex.exam_id);
                }
            }
            examlist = masterService.ViewAllScheduleExams();
            ViewData["scheduledExams"] = examlist;
            ViewData["rejectedExams"] = masterService.ViewAllRejectedExams();
            ViewData["submittedExams"] = masterService.ViewAllSubmittedExams();
            ExamModel em = new ExamModel();
            return View(em);
        }
        [HttpPost]
        public string ScheduleExam(ExamModel em)
        {
           int exam_id= masterService.ScheduleExamForStudent(em);
            string msg=ShareExamLink(exam_id, em.student_id);
            return  "Exam Scheduled Successfully,"+ msg;
           // string employee = HttpContext.Session.GetString("employee");
           // EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
           // ViewData["employee"] = emp;
          //  List<TopicModel> topics = masterService.GetTrainingTopics();
            ////List<StudentModel> students = studentService.GetStudents();
            //ViewBag.students = GetStudents();
            //ViewData["scheduledExams"] = masterService.ViewAllScheduleExams();
            //ViewData["rejectedExams"] = masterService.ViewAllRejectedExams();
            //ViewData["submittedExams"] = masterService.ViewAllSubmittedExams();
            //ViewBag.topics = new SelectList(topics, "topic_id", "topic_name");
            //return View();

        }
        public List<SelectListItem> GetStudents()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<StudentModel> students = studentService.GetAllStudents();
            foreach (var item in students)
            {
                lst.Add(new SelectListItem() {
                 Value=item.student_id.ToString(),
                  Text=item.student_name+" "+item.last_name
                });
            }
            return lst;
        }
        public string ShareExamLink(int exam_id, int student_id)
        {
            try
            {
                 string examlink =DomainUrl.Url+ "/Assessment/Index?exam_id=" + exam_id + "&student_id=" + student_id;
                //  string examlink = "https://ciitstudent.com/Assessment/Index?exam_id=" + exam_id + "&student_id=" + student_id;

                StudentModel student = studentService.GetStudent(student_id);

                ExamModel exam = masterService.ViewAllScheduleExams().FirstOrDefault(e => e.exam_id.Equals(exam_id));

                string examtime = exam.exam_date.ToLongDateString() + " " + exam.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt <= DateTime.Now)
                {
                    masterService.RejectScheduledExam(exam_id);
                    return "Exam has been expired. cannot share link to the student";
                }
                else
                {
                    string message = "<h2>Dear " + student.student_name + ",</h2><p>Your exam has been scheduled for <b>" + exam.topic_name + "</b> on <b>" + exam.exam_date.ToLongDateString() + "</b> at Start Time:<b> " + exam.start_time.ToLongTimeString() + " </b>, End Time:<b>"+exam.end_time.ToLongTimeString()+"</b> </p><p><h4>Below is the exam link</h4></p><p><a target='_blank' href='" + examlink + "'>Click Here</a></p><br/><br/><h2>Best of Luck</h2><br/><p><h5>Regards,CIIT Training Institute Pvt Ltd</h5></p>";
                    EmailModel email = new EmailModel()
                    {
                        UserName = student.student_name,
                        EmailAddress = student.email_address,
                        Subject = "Exam Scheduled for " + exam.topic_name,
                        Message = message
                    };
                    extraService.SendEmail(email, _settings);
                    return "Exam Link Shared Successfully to the student";
                }
            }
            catch (Exception ex)
            {
                return "Unable to send exam link.send link again";
            }
        }

        public IActionResult Exams()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            //int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
            List<ExamModel> exams = masterService.GetAllExams();
            return View(exams);

        }
        public IActionResult ViewExamDetails(int id)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            ExamModel em = masterService.GetExam(id);
           // int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
            return View(em);

        }
    }
}
