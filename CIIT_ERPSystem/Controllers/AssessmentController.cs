using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
namespace CIIT_ERPSystem.Controllers
{
    public class AssessmentController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        EmailSettings _settings;
        public AssessmentController(IMasterService masterService, IStudentService studentService, IOptions<EmailSettings> settings)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this._settings = settings.Value;
        }
        public IActionResult Index(int exam_id,int student_id)
        {
            StudentModel student = studentService.GetStudent(student_id);
            ExamModel exam = masterService.ViewAllScheduleExams().FirstOrDefault(e => e.exam_id.Equals(exam_id));
            HttpContext.Session.SetString("student_name", student.student_name);
            HttpContext.Session.SetInt32("student_id", student.student_id);
            HttpContext.Session.SetString("student", JsonConvert.SerializeObject(student));
            //ExamModel exam = new ExamModel()
            //{
            //    topic_id = topic_id,
            //    student_id = student_id,
            //    exam_date = DateTime.Now,
            //    start_time = DateTime.Now,
            //};
            if(exam==null)
            {
                ViewBag.msg = "Exam has expired. Please contact to the branch";
                return View();

            }
            string examtime = exam.exam_date.ToLongDateString() + " " + exam.start_time.ToLongTimeString();
            DateTime dt = Convert.ToDateTime(examtime);
            if(dt.AddHours(1)<=DateTime.Now)
            {
                masterService.RejectScheduledExam(exam_id);
                ViewBag.status = false;
                ViewBag.msg = "Exam has expired. Please contact to the branch";
                return View();
            }
            else
            {
                ViewBag.status = true;
            }
            HttpContext.Session.SetString("exam", JsonConvert.SerializeObject(exam));
            TopicModel topic = masterService.GetTrainingTopics().FirstOrDefault(e => e.topic_id.Equals(exam.topic_id));
            ViewBag.topic = topic.topic_name;
            ViewBag.question_count = exam.total_questions;
            List<ContentQuestionModel> questions = masterService.GetTopicWiseQuestions(exam.topic_id, exam.total_questions);
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questions));
            return View(exam);
        }

        public JsonResult GetCurrentExam()
        {
          string ex=  HttpContext.Session.GetString("exam");
            ExamModel exam = (ExamModel)JsonConvert.DeserializeObject<ExamModel>(ex);
            return Json(exam);

        }


        public IActionResult StartExam(int? question_id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            string questions = HttpContext.Session.GetString("questions");
            List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
            ViewBag.questions = questionlist;
            ContentQuestionModel q = null;
            if (question_id != null)
            {
                q = questionlist.FirstOrDefault(e => e.question_id.Equals(question_id));
                if (q.serial_number < questionlist.Count)
                {
                    ViewBag.next = false;
                }
                else
                {
                    ViewBag.next = true;

                }
                if (q.serial_number >1)
                {
                    ViewBag.prev = false;
                }
                else
                {
                    ViewBag.prev = true;

                }
            }
            else
            {
                q = questionlist.First();
                ViewBag.prev = true;

            }
            //ContentQuestionModel q = questionlist.First();


            return View(q);
        }
        [HttpPost]
        public IActionResult StartExam(ContentQuestionModel cm, string command)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
              return RedirectToAction("Login");
            }
            string questions = HttpContext.Session.GetString("questions");
            List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
            ViewBag.questions = questionlist;
            ContentQuestionModel qs = questionlist.FirstOrDefault(e => e.question_id.Equals(cm.question_id));
            int index = questionlist.IndexOf(qs);
            qs.submitted_option_number = cm.submitted_option_number;
            questionlist[index] = qs;
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questionlist));
            ContentQuestionModel q = null;
            if (command == null)
            {
                q = questionlist[0];
            }
            else if (command == "Next")
            {
                cm.serial_number++;
                if (cm.serial_number < questionlist.Count)
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
                    ViewBag.next = false;
                }
                else
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));

                    ViewBag.next = true;
                }
            }
            else if (command == "Prev")
            {
                cm.serial_number--;
                if (cm.serial_number > 1)
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
                    ViewBag.prev = false;
                }
                else
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
                    ViewBag.prev = true;
                }
            }
            else if (command == "Submit")
            {
                string exatdata = HttpContext.Session.GetString("exam");
                ExamModel exam = (ExamModel)JsonConvert.DeserializeObject<ExamModel>(exatdata);
                exam.end_time = DateTime.Now;
                string questiondata = HttpContext.Session.GetString("questions");
                List<ContentQuestionModel> questionlistdata = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
                List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
                foreach (ContentQuestionModel c in questionlistdata)
                {
                    ExamQuestionModel e = new ExamQuestionModel()
                    {
                        question_id = c.question_id,
                        submitted_option_number = c.submitted_option_number
                    };
                    lst.Add(e);
                }
                exam.examQuestions = lst;
                masterService.SubmitExam(exam);
                return RedirectToAction("SubmitExam");
            }
            ModelState.Clear();
            return View(q);
        }

        public IActionResult SubmitExam()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.msg = "Exam Submitted Successfully.Please check your mail for exam result.";
            return View();
        }
    }
}
