
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
namespace CIIT_ERPSystem.Controllers
{
    public class StudentController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IBatchService batchService;
        private IWebHostEnvironment environment;

        public StudentController(IMasterService masterService,IStudentService studentService,IBatchService batchService, IWebHostEnvironment environment)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.batchService = batchService;
            this.environment = environment;
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
            ViewBag.msg = "Profile Details updated successfully";
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
                StudentModel sm = studentService.GetStudent(student.student_id);
                HttpContext.Session.SetString("student_name", student.student_name);
                HttpContext.Session.SetInt32("student_id", student.student_id);
                HttpContext.Session.SetString("student", JsonConvert.SerializeObject(sm));
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
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
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
            ViewBag.question_count = exam.total_questions;
            List<ContentQuestionModel> questions = masterService.GetTopicWiseQuestions(topic_id, 5);
            HttpContext.Session.SetString("questions",JsonConvert.SerializeObject(questions));
            return View();
        }

        public string ChangeProfilePhoto(IFormFile file)
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel d = studentService.GetStudent(student_id);
            Random r = new Random();
            int n = r.Next(1, 1000);
            string imgname = d.student_name + "_" + n + Path.GetExtension(file.FileName);
            string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            file.CopyTo(fs);
           // d.profile_photo = imgname;
            studentService.ChangeStudentProfilePhoto(student_id, imgname);
            //string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            //string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            //if (System.IO.File.Exists(aadharpath))
            //{
            //    System.IO.File.Delete(aadharpath);
            //}
            //FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            //aadharcard.CopyTo(fsaadhar);
            //d.aadhar_card_photo = aadharname;
              d = studentService.GetStudent(student_id);
            HttpContext.Session.SetString("student_name", d.student_name);
            HttpContext.Session.SetInt32("student_id", d.student_id);
            HttpContext.Session.SetString("student", JsonConvert.SerializeObject(d));

            return "Profile Photo Changed Successfully";

        }

        public string ChangeAadharPhoto(IFormFile file)
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel d = studentService.GetStudent(student_id);
            Random r = new Random();
            int n = r.Next(1, 1000);
            string imgname = d.student_name + "_" + n + Path.GetExtension(file.FileName);
            string imgpath = environment.WebRootPath + "/Students/AadharCards/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            file.CopyTo(fs);
            // d.profile_photo = imgname;
            studentService.ChangeStudentAadharPhoto(student_id, imgname);
            //string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            //string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            //if (System.IO.File.Exists(aadharpath))
            //{
            //    System.IO.File.Delete(aadharpath);
            //}
            //FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            //aadharcard.CopyTo(fsaadhar);
            //d.aadhar_card_photo = aadharname;
            d = studentService.GetStudent(student_id);
            HttpContext.Session.SetString("student_name", d.student_name);
            HttpContext.Session.SetInt32("student_id", d.student_id);
            HttpContext.Session.SetString("student", JsonConvert.SerializeObject(d));

            return "Profile Photo Changed Successfully";

        }
        
        public string ChangePassword(ChangePasswordModel p)
        {

            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel d = studentService.GetStudent(student_id);
            StudentModel sm = masterService.CheckStudentLogin(d.email_address, p.current_password);
            if (sm != null)
            {
                studentService.ChangeStudentPassword(student_id, p.new_password);
                return "Password changed successfully";
            }
            else
            {
                return "current password is not matched";

            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("student_id");
            HttpContext.Session.Remove("student_name");
            HttpContext.Session.Remove("student");
            return RedirectToAction("Login");

        }

        //public IActionResult StartExam()
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    string questions = HttpContext.Session.GetString("questions");
        //    List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
        //    ViewBag.questions = questionlist;
        //    ContentQuestionModel q = questionlist.First();

        //    ViewBag.prev = true;

        //    return View(q);
        //}
        //[HttpPost]
        //public IActionResult StartExam(ContentQuestionModel cm, string command)
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    string questions = HttpContext.Session.GetString("questions");
        //    List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
        //    ViewBag.questions = questionlist;

        //    ContentQuestionModel qs = questionlist.FirstOrDefault(e => e.question_id.Equals(cm.question_id));
        //    int index=questionlist.IndexOf(qs);
        //    qs.submitted_option_number = cm.submitted_option_number;
        //    questionlist[index] = qs;
        //    HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questionlist));
        //    ContentQuestionModel q=null;
        //    if (command == null)
        //    {
        //        q = questionlist[0];
        //    }
        //    else if (command == "Next")
        //    {
        //        cm.serial_number++;
        //        if (cm.serial_number < questionlist.Count)
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
        //            ViewBag.next = false;
        //        }
        //        else
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));

        //            ViewBag.next = true;
        //        }
        //    }
        //    else if (command == "Prev")
        //    {
        //        cm.serial_number--;
        //        if (cm.serial_number > 1)
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
        //            ViewBag.prev = false;
        //        }
        //        else
        //        {
        //            q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
        //            ViewBag.prev = true;
        //        }
        //    }
        //    else if (command == "Submit")
        //    {
        //        string exatdata = HttpContext.Session.GetString("exam");
        //        ExamModel exam=(ExamModel)JsonConvert.DeserializeObject<ExamModel>(exatdata);
        //        exam.end_time = DateTime.Now;
        //        string questiondata = HttpContext.Session.GetString("questions");
        //        List<ContentQuestionModel> questionlistdata = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
        //        List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
        //        foreach(ContentQuestionModel c in questionlistdata)
        //        {
        //            ExamQuestionModel e = new ExamQuestionModel()
        //            {
        //                question_id = c.question_id,
        //                submitted_option_number = c.submitted_option_number
        //            };
        //            lst.Add(e);
        //        }
        //        exam.examQuestions = lst;
        //        masterService.SubmitExam(exam);
        //        return RedirectToAction("SubmitExam");
        //    }
        //    ModelState.Clear();
        //    return View(q);
        //}

        //public IActionResult SubmitExam()
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    ViewBag.msg = "Exam Submitted Successfully.Please check your mail for exam result.";
        //    return View();
        //}

        public IActionResult  Exams()
        {
            int student_id =(int) HttpContext.Session.GetInt32("student_id");
            List<ExamModel>exams=masterService.GetStudentWiseExams(student_id);
            return View(exams);

        }
        public IActionResult ViewExamDetails(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            ExamModel em=masterService.GetExam(id);
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
            return View(em);

        }
    }
}
