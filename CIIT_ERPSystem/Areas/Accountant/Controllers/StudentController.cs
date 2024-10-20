using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System;

namespace CIIT_ERPSystem.Areas.Accountant.Controllers
{
    [Area(areaName: "Accountant")]
    public class StudentController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        public StudentController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService,IOptions<EmailSettings> settings, IBatchService batchService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
        }

        public IActionResult RegistrationForm()
        {
            // ViewData["students"] = studentService.GetStudents();
            ViewBag.courses = GetCoursesForGuest();
            string pinnumber = studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
        }

        //[HttpPost]
        //public ActionResult RegistrationForm(StudentModel d, IFormFile photo, string feeid)
        //{
        //    d.student_code = "Guest";
        //    string imgname = d.student_name + Path.GetExtension(photo.FileName);
        //    string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
        //    FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
        //    photo.CopyTo(fs);
        //    d.profile_photo = imgname;
        //    string password = extraService.GetRandomPassword(10);
        //    d.password = password;
        //    RegistrationModel rs = new RegistrationModel()
        //    {
        //        registration_date = DateTime.Now,
        //        fee_id = Convert.ToInt32(feeid),
        //        discount = 0
        //    };
        //    List<RegistrationModel> registrations = new List<RegistrationModel>();
        //    registrations.Add(rs);
        //    d.registrations = registrations;
        //    studentService.AddStudentRegistration(d);
        //    //      string message = "<h2>Dear<br/> " + d.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + d.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
        //    //    EmailModel em = new EmailModel() { UserName = d.student_name, EmailAddress = d.email_address, Message = message, Subject = "Registration Confirmation" };
        //    //    extraService.SendEmail(em, _settings);
        //    //ModelState.Clear();
        //    //ViewBag.msg = "Student Added Successfully";
        //    //ViewBag.courses = GetCourses();
        //    //ViewBag.students = studentService.GetStudentRegistrations();


        //    //ViewData["students"] = studentService.GetStudents();
        //    //ViewBag.courses = GetCourses();
        //    //StudentModel sm = new StudentModel();
        //    // ViewData["students"] = studentService.GetStudents();
        //    ModelState.Clear();
        //    ViewBag.msg = "Student Added Successfully";
        //    ViewBag.courses = GetCoursesForGuest();
        //    string pinnumber = studentService.NextPINNumber();
        //    StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
        //    return View(sm);
        //    //return View(sm);

        //}

        [HttpPost]
        public ActionResult RegistrationForm(StudentModel d, IFormFile photo, IFormFile aadharcard, string feeid)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            Random r = new Random();
            int n = r.Next(1, 1000);
            d.student_code = "Guest";
            string imgname = d.student_name + "_" + n + Path.GetExtension(photo.FileName);
            string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            photo.CopyTo(fs);
            d.profile_photo = imgname;

            string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            if (System.IO.File.Exists(aadharpath))
            {
                System.IO.File.Delete(aadharpath);
            }
            FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            aadharcard.CopyTo(fsaadhar);
            d.aadhar_card_photo = aadharname;



            string password = extraService.GetRandomPassword(10);
            d.password = password;
            RegistrationModel rs = new RegistrationModel()
            {
                registration_date = DateTime.Now,
                fee_id = Convert.ToInt32(feeid),
                discount = 0
            };
            List<RegistrationModel> registrations = new List<RegistrationModel>();
            registrations.Add(rs);
            d.registrations = registrations;
            studentService.AddStudentRegistration(d);
            //      string message = "<h2>Dear<br/> " + d.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + d.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
            //    EmailModel em = new EmailModel() { UserName = d.student_name, EmailAddress = d.email_address, Message = message, Subject = "Registration Confirmation" };
            //    extraService.SendEmail(em, _settings);
            //ModelState.Clear();
            //ViewBag.msg = "Student Added Successfully";
            //ViewBag.courses = GetCourses();
            //ViewBag.students = studentService.GetStudentRegistrations();


            //ViewData["students"] = studentService.GetStudents();
            //ViewBag.courses = GetCourses();
            //StudentModel sm = new StudentModel();
            // ViewData["students"] = studentService.GetStudents();
            ModelState.Clear();
            ViewBag.msg = "Student Added Successfully";
            ViewData["students"] = studentService.GetStudents();
            ViewData["guest"] = studentService.GetGuestStudents();
            ViewBag.courses = GetCourses();
            string pinnumber = studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
            //return View(sm);

        }
        public List<SelectListItem> GetCoursesForGuest()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (CourseFeeModel c in masterService.GetCourseFees())
            {
                SelectListItem st = lst.FirstOrDefault(e => e.Text.Equals(c.course_name));
                if (st == null)
                {
                    SelectListItem s = new SelectListItem() { Text = c.course_name, Value = c.fee_id.ToString() };
                    lst.Add(s);
                }
            }
            return lst;
        }



        public List<SelectListItem> GetCourses()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach(CourseFeeModel c in  masterService.GetCourseFees())
            {
                SelectListItem s = new SelectListItem() { Text=c.course_name+"("+c.fee_mode+")", Value=c.fee_id.ToString() };
                lst.Add(s);
            }
            return lst ;
        }
        public IActionResult Index()
        {
            ViewData["students"]=studentService.GetStudents();
            ViewData["guest"]=studentService.GetGuestStudents();
            //ViewBag.courses = GetCourses();
            //StudentModel sm = new StudentModel();
            //return View(sm);
            ViewBag.courses = GetCourses();
            string pinnumber = studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
        }
        //[HttpPost]
        //public IActionResult Index(StudentModel d, IFormFile photo, DateTime registrationdate, string feeid, string discount)
        //{
        //    d.student_code = "student";
        //    string imgname = d.student_name + Path.GetExtension(photo.FileName);
        //    string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
        //    FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
        //    photo.CopyTo(fs);
        //    d.profile_photo = imgname;
        //    string password = extraService.GetRandomPassword(10);
        //    d.password = password;
        //    RegistrationModel rs = new RegistrationModel()
        //    {
        //        registration_date = registrationdate,
        //        fee_id = Convert.ToInt32(feeid),
        //        discount = (float)Convert.ToDouble(discount)
        //    };
        //    List<RegistrationModel> registrations = new List<RegistrationModel>();
        //    registrations.Add(rs);
        //    d.registrations = registrations ;
        //    studentService.AddStudentRegistration(d);
        //    string message = "<h2>Dear<br/> " + d.student_name + ",</h2><p>Your registration has been completed successfully.You can login by email address <b>" + d.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
        //    EmailModel em = new EmailModel() { UserName=d.student_name, EmailAddress=d.email_address, Message=message, Subject="Registration Confirmation" };
        //    extraService.SendEmail(em,_settings);
        //    //ModelState.Clear();
        //    //ViewBag.msg = "Student Added Successfully";
        //    //ViewBag.courses = GetCourses();
        //    //ViewBag.students = studentService.GetStudentRegistrations();


        //    //ViewData["students"] = studentService.GetStudents();
        //    //ViewBag.courses = GetCourses();
        //    //StudentModel sm = new StudentModel();
        //    //return View(sm);
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult Index(StudentModel d, IFormFile photo, IFormFile aadharcard, string feeid)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            Random r=new Random ();
            int n = r.Next(1, 1000);
            d.student_code = "Student";
            string imgname = d.student_name + "_" + n + Path.GetExtension(photo.FileName);
            string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            photo.CopyTo(fs);
            d.profile_photo = imgname;

            string aadharname = d.student_name + "_adhr_"+r.Next(1,1000)  + Path.GetExtension(aadharcard.FileName);
            string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            if (System.IO.File.Exists(aadharpath))
            {
                System.IO.File.Delete(aadharpath);
            }
            FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            aadharcard.CopyTo(fsaadhar);
            d.aadhar_card_photo = aadharname;



            string password = extraService.GetRandomPassword(10);
            d.password = password;
            RegistrationModel rs = new RegistrationModel()
            {
                registration_date = DateTime.Now,
                fee_id = Convert.ToInt32(feeid),
                discount = 0
            };
            List<RegistrationModel> registrations = new List<RegistrationModel>();
            registrations.Add(rs);
            d.registrations = registrations;
            studentService.AddStudentRegistration(d);
            //      string message = "<h2>Dear<br/> " + d.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + d.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
            //    EmailModel em = new EmailModel() { UserName = d.student_name, EmailAddress = d.email_address, Message = message, Subject = "Registration Confirmation" };
            //    extraService.SendEmail(em, _settings);
            //ModelState.Clear();
            //ViewBag.msg = "Student Added Successfully";
            //ViewBag.courses = GetCourses();
            //ViewBag.students = studentService.GetStudentRegistrations();


            //ViewData["students"] = studentService.GetStudents();
            //ViewBag.courses = GetCourses();
            //StudentModel sm = new StudentModel();
            // ViewData["students"] = studentService.GetStudents();
            ModelState.Clear();
            ViewBag.msg = "Student Added Successfully";
            ViewData["students"] = studentService.GetStudents();
            ViewData["guest"] = studentService.GetGuestStudents();
            ViewBag.courses = GetCourses();
            string pinnumber = studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
            //return View(sm);

        }
        public IActionResult GetStudentdetails(int id)
        {
            StudentModel sm=studentService.GetStudent(id);
            List<BatchStudentModel> batches = batchService.GetStudentWiseBatches(id);
            ViewData["batches"] = batches;
            ViewData["payments"] = studentService.GetStudentWisePayments(id);
            return View(sm);
        }
        public IActionResult PayFees()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            List<RegistrationModel>lst=studentService.GetAllRegistrations();
            StudentPaymentModel sm = new StudentPaymentModel();
            ViewBag.registrations = new SelectList(lst,"registration_id","student_name");
            return View(sm);
        }
        [HttpPost]
        public IActionResult PayFees(StudentPaymentModel s)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            studentService.AddPayment(s);
            ViewBag.msg = "Payment Accepted Successfully";
            ModelState.Clear();

            List<RegistrationModel> lst = studentService.GetAllRegistrations();
            StudentPaymentModel sm = new StudentPaymentModel();
            ViewBag.registrations = new SelectList(lst, "registration_id", "student_name");
            return View(sm);
        }
        public JsonResult GetRegistrationDetails(int id)
        {
            RegistrationModel r=studentService.GetRegistration(id);
            return Json(r);
        }
        
        public IActionResult Invoices()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            List<StudentPaymentModel> invoices = studentService.GetStudentPayments();
            return View(invoices);
        }
        public IActionResult ViewInvoice(int registration_id,int payment_id)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            StudentPaymentModel p = studentService.GetStudentPayment(payment_id);
            RegistrationModel r= studentService.GetRegistration(registration_id);
            ViewData["registration"] = r;
            ViewData["payments"] = studentService.GetStudentWisePreviousPayments(registration_id,payment_id);
            ViewData["student"] = studentService.GetStudent(r.student_id);
            return View(p);
        }

    }
}
