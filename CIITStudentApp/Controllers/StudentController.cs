
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
namespace CIITStudentApp.Controllers
{
    public class StudentController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IBatchService batchService;
        public StudentController(IMasterService masterService,IStudentService studentService,IBatchService batchService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.batchService = batchService;
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id =(int) HttpContext.Session.GetInt32("student_id");
            StudentModel student = studentService.GetStudent(student_id);

            return View(student);
        }
        
        [HttpPost]
        public IActionResult Profile(StudentModel sm)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            studentService.UpdateStudentDetails(sm);
            StudentModel student = studentService.GetStudent(sm.student_id);
            return View(student);
        }
        public IActionResult Enrollment()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = studentService.GetStudent(student_id);
            return View(student);
        }
        public IActionResult CourseSyllabus(int id)
        {
            CourseModel c=masterService.GetTrainingCourse(id);
            return View(c);

        }
        public IActionResult BatchDetails()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
           List<BatchStudentModel>batches=batchService.GetStudentWiseBatches(student_id);

            return View(batches);
        }
        public IActionResult Login()
        {
            StudentLoginModel sm=new StudentLoginModel ();
            return View(sm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(StudentLoginModel st)
        {
            if (!ModelState.IsValid)
            {
                return View(st);
            }
            StudentModel student=masterService.CheckStudentLogin(st.email_address, st.password);
            if(student == null)
            {
                ViewBag.msg = "Invalid email address or password";
                StudentLoginModel sm = new StudentLoginModel();
                return View(sm);
            }
            else
            {
                HttpContext.Session.SetString("student_name", student.student_name);
                HttpContext.Session.SetInt32("student_id", student.student_id);
                HttpContext.Session.SetString("student", JsonConvert.SerializeObject(student));
                return RedirectToAction("Profile");
            }
        }
        public IActionResult ViewTopics()
        {
            if (HttpContext.Session.GetString("student_name") == null)
            {
                return RedirectToAction("Login");

            }
            ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");
            return View();
        }

        public IActionResult InitiateExam(int topic_id)
        {
            HttpContext.Session.SetInt32("topic_id", topic_id);
            int student_id =(int) (HttpContext.Session.GetInt32("student_id"));

            ExamModel exam = new ExamModel() 
            { 
                topic_id=topic_id, 
                student_id=student_id,  
                 exam_date=DateTime.Now,
                  start_time=DateTime.Now,
            
            };
            HttpContext.Session.SetString("exam", JsonConvert.SerializeObject(exam));

            TopicModel topic =masterService.GetTrainingTopics().FirstOrDefault(e=>e.topic_id.Equals(topic_id));
            ViewBag.topic = topic.topic_name;
            ViewBag.question_count = 20;
            List<ContentQuestionModel> questions = masterService.GetTopicWiseQuestions(topic_id, 5);
            HttpContext.Session.SetString("questions",JsonConvert.SerializeObject(questions));
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("student_name");
            HttpContext.Session.Remove("student");
            return RedirectToAction("Login");

        }

        public IActionResult StartExam()
        {
            string questions = HttpContext.Session.GetString("questions");
            List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
            ViewBag.questions = questionlist;
            ContentQuestionModel q = questionlist.First();

            ViewBag.prev = true;

            return View(q);
        }
        [HttpPost]
        public IActionResult StartExam(ContentQuestionModel cm, string command)
        {
            string questions = HttpContext.Session.GetString("questions");
            List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
            ViewBag.questions = questionlist;

            ContentQuestionModel qs = questionlist.FirstOrDefault(e => e.question_id.Equals(cm.question_id));
            int index=questionlist.IndexOf(qs);
            qs.submitted_option_number = cm.submitted_option_number;
            questionlist[index] = qs;
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questionlist));
            ContentQuestionModel q=null;
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
                ExamModel exam=(ExamModel)JsonConvert.DeserializeObject<ExamModel>(exatdata);
                exam.end_time = DateTime.Now;
                string questiondata = HttpContext.Session.GetString("questions");
                List<ContentQuestionModel> questionlistdata = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
                List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
                foreach(ContentQuestionModel c in questionlistdata)
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
            ViewBag.msg = "Exam Submitted Successfully.Please check your mail for exam result.";
            return View();
        }

        public IActionResult  Exams()
        {
            int student_id =(int) HttpContext.Session.GetInt32("student_id");
            List<ExamModel>exams=masterService.GetStudentWiseExams(student_id);
            return View(exams);

        }
        public IActionResult ViewExamDetails(int id)
        {
            ExamModel em=masterService.GetExam(id);
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
            return View(em);

        }
    }
}
