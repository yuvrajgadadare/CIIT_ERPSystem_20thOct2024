using CIIT_NEW_WithCore.Models;
//using CIIT_NEW_WithCore.Services.Implementations;
//using CIIT_NEW_WithCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CIIT_NEW_WithCore.Controllers
{
    public class RegistrationController : Controller
    {
    //    IStudentService studentService;
    //    ITrainingCourseService trainingCourseService;
    //    IExtraService extraService;
    //    ITrainingCourseService courseService;
    //    IEmailService emailService;


    //    private IWebHostEnvironment environment;

    //    public RegistrationController(IStudentService studentService, ITrainingCourseService trainingCourseService, IWebHostEnvironment environment, IExtraService extraService, ITrainingCourseService courseService, IEmailService emailService)
    //    {
    //        this.studentService = studentService;
    //        this.trainingCourseService = trainingCourseService;
    //        this.environment = environment;
    //        this.extraService = extraService;
    //        this.courseService = courseService;
    //        this.emailService = emailService;
    //    }

    //    public async Task<List<SelectListItem>> GetCourses()
    //    {
    //        List<SelectListItem> lst = new List<SelectListItem>();
    //        foreach (CourseModel r in await Task.Run(() => courseService.GetCourses()))
    //        {
    //            SelectListItem s = new SelectListItem()
    //            {
    //                Text = r.CourseName,
    //                //Text = r.CourseName + " With " + r.FeeMode + " Fees",
    //                Value = r.FeeId.ToString()
    //            };
    //            lst.Add(s);
    //        }
    //        return lst;
    //    }
    //    public async Task<IActionResult> Index()
    //    {
    //        ViewBag.courses = await Task.Run(() => GetCourses());
    //        ViewBag.students = await Task.Run(() => studentService.GetStudentRegistrations());
    //        return View();
    //    }
    //    [HttpPost]
    //    public async Task<IActionResult> Index(RegistrationModel r, IFormFile photo)
    //    {

    //        //TblstudentDetail sdmob = studentService.GetStudentDetails().FirstOrDefault(e => e.MobileNumber.Equals(r.MobileNumber) );
    //        //TblstudentDetail sdemail = studentService.GetStudentDetails().FirstOrDefault(e =>  e.EmailAddress.Equals(r.EmailAddress));
    //        if ( await studentService.IsMobileExist(r.MobileNumber))
    //        {
    //            ViewBag.courses = GetCourses();
    //            ViewBag.students = studentService.GetStudentRegistrations();
    //            ViewBag.msg = "Mobile Number  is already exist";

    //            return View();
    //        }
    //        else if (await studentService.IsEmailExist(r.EmailAddress))
    //        {

    //            ViewBag.courses = GetCourses();
    //            ViewBag.students = studentService.GetStudentRegistrations();
    //            ViewBag.msg = "Email address is already exist";
    //            return View();


    //        }
    //        else
    //        {
    //            if (photo != null)
    //            {

    //                string imgname = r.StudentName + Path.GetExtension(photo.FileName);
    //                string imgpath = environment.WebRootPath + "/images/Students/" + imgname;
    //                FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
    //                photo.CopyTo(fs);
    //                r.ProfilePhoto = imgname;
    //            }
    //            string password = await Task.Run(()=> extraService.GetRandomPassword(10));
    //            r.Password = password;
    //            HttpContext.Session.SetString("registration", JsonConvert.SerializeObject(r));
    //            //ViewBag.courses = GetCourses();
    //            //ViewBag.students = studentService.GetStudentRegistrations();


    //            Random randomObj = new Random();
    //            string transactionId = randomObj.Next(10000000, 100000000).ToString();

    //            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_live_2YNZNBy4uflUsd", "HgPEeHOX2u1cChdZMPRHVdM4");
    //            Dictionary<string, object> options = new Dictionary<string, object>();
    //            options.Add("amount", r.RegistrationAmount * 100);  // Amount will in paise
    //            options.Add("receipt", transactionId);
    //            options.Add("currency", "INR");
    //            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
    //            Razorpay.Api.Order orderResponse = client.Order.Create(options);
    //            string orderId = orderResponse["id"].ToString();
    //            MerchantOrder orderModel = new MerchantOrder
    //            {
    //                OrderId = orderResponse.Attributes["id"],
    //                RazorpayKey = "rzp_live_2YNZNBy4uflUsd",
    //                Amount = (float)r.RegistrationAmount * 100,
    //                Currency = "INR",
    //                Name = r.StudentName,
    //                Email = r.EmailAddress,
    //                PhoneNumber = r.MobileNumber,
    //                Address = r.Address,
    //                Description = "Registration For CIIT Training Institute"
    //            };

    //            //db.tblcollege_student_details.Add(st);
    //            //db.SaveChanges();
    //            //ViewBag.msg = "Thank you for submitting your information successfully.Our representative will connect you by call or email.";
    //            //ViewBag.topics = GetTrainings();
    //            //return RedirectToAction("Payment");
    //            return View("PaymentPage", orderModel);
    //        }
    //    }


    //    [HttpPost]
    //    public ActionResult Complete(string rzp_paymentid, string rzp_orderid)
    //    {
    //        // Payment data comes in url so we have to get it from url

    //        // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
    //        //string paymentId = Request.Params["rzp_paymentid"];

    //        //// This is orderId
    //        //string orderId = Request.Params["rzp_orderid"];

    //        string paymentId = rzp_paymentid;

    //        // This is orderId
    //        string orderId = rzp_orderid;

    //        //            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_umbrFAbVJ3slyJ", "su9eXFaihGucMmKECVRcRk0Q");
    //        Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_live_2YNZNBy4uflUsd", "HgPEeHOX2u1cChdZMPRHVdM4");

    //        //            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_live_WG2hMQTKiy6lLZ", "WrFjnp4HDgAEu6ri8uM7qdbB");

    //        Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

    //        // This code is for capture the payment 
    //        Dictionary<string, object> options = new Dictionary<string, object>();
    //        options.Add("amount", payment.Attributes["amount"]);
    //        Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
    //        string amt = paymentCaptured.Attributes["amount"];

    //        //// Check payment made successfully

    //        if (paymentCaptured.Attributes["status"] == "captured")
    //        {
    //            string data = HttpContext.Session.GetString("registration");
    //            RegistrationModel r = JsonConvert.DeserializeObject<RegistrationModel>(data);

    //            TblstudentPayment pay = new TblstudentPayment() { PaymentAmount = r.RegistrationAmount, PaymentDate = DateTime.Now, PaymentMode = "Razor Pay", PaymentDescription = "Payment for Registration" };
    //            List<TblstudentPayment> payments = new List<TblstudentPayment>();
    //            payments.Add(pay);
    //            TblstudentRegistration reg = new TblstudentRegistration()
    //            {
    //                Discount = r.Discount,
    //                FeeId = r.FeeId,
    //                RegistrationDate = r.RegistrationDate,
    //                TblstudentPayments = payments


    //            };
    //            List<TblstudentRegistration> reglist = new List<TblstudentRegistration>();
    //            reglist.Add(reg);
    //            TblstudentDetail sd = new TblstudentDetail()
    //            {
    //                StudentName = r.StudentName,
    //                MobileNumber = r.MobileNumber,
    //                EmailAddress = r.EmailAddress,
    //                BirthDate = (DateTime)r.BirthDate,
    //                Gender = r.Gender,
    //                Password = r.Password,
    //                ProfilePhoto = r.ProfilePhoto,
    //                Qualification = r.Qualification,
    //                TblstudentRegistrations = reglist

    //            };
    //            studentService.AddStudentDetails(sd);
    //                string message = "<h2>Dear<br/> " + r.StudentName + ",</h2><p>Your registration has been completed successfully.You can login by email address <b>" + r.EmailAddress + "</b> and password <b>" + r.Password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
    //            EmailModel em = new EmailModel()
    //            {
    //                EmailAddress = sd.EmailAddress,
    //                Message = message,
    //                Subject = "Registration Confirmation",
    //                UserName = sd.StudentName
    //            };

    //                emailService.SendEmail(em);
    //              message = "Dear " + r.StudentName + ", Welcome to CIIT Training Institute Student Portal. You can access your account from https://ciitstudent.com/User/Login, with user=" + r.EmailAddress + " and password=" + r.Password + " . Regards, CIIT Training Institute";
    //            extraService.SendTransactionalSMS(r.MobileNumber, message);


    //            return RedirectToAction("Success");
    //        }
    //        else
    //        {
    //            return RedirectToAction("Failed");
    //        }
    //    }


    //    //[HttpPost]
    //    //public async Task<IActionResult> Index(RegistrationModel r, IFormFile photo)
    //    //{

    //    //    //TblstudentDetail sdmob = studentService.GetStudentDetails().FirstOrDefault(e => e.MobileNumber.Equals(r.MobileNumber) );
    //    //    //TblstudentDetail sdemail = studentService.GetStudentDetails().FirstOrDefault(e =>  e.EmailAddress.Equals(r.EmailAddress));
    //    //    if (await Task.Run(() => studentService.IsMobileExist(r.MobileNumber).Result))
    //    //    {
    //    //        ViewBag.courses = GetCourses();
    //    //        ViewBag.students = await Task.Run(() => studentService.GetStudentRegistrations());
    //    //        ViewBag.msg = "Mobile Number  is already exist";
    //    //        return View();
    //    //    }
    //    //    else if (await Task.Run(() => studentService.IsEmailExist(r.EmailAddress).Result))
    //    //    {

    //    //        ViewBag.courses = await Task.Run(() => GetCourses());
    //    //        ViewBag.students = await Task.Run(() => studentService.GetStudentRegistrations());
    //    //        ViewBag.msg = "Email address is already exist";
    //    //        return View();


    //    //    }
    //    //    else
    //    //    {


    //    //        string imgname = r.StudentName + Path.GetExtension(photo.FileName);
    //    //        string imgpath = environment.WebRootPath + "/images/users/" + imgname;
    //    //        FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
    //    //        photo.CopyTo(fs);
    //    //        r.ProfilePhoto = imgname;
    //    //        string password = await Task.Run(() => extraService.GetRandomPassword(10));
    //    //        r.Password = password;


    //    //        //TblstudentPayment pay = new TblstudentPayment() { PaymentAmount = r.RegistrationAmount, PaymentDate = DateTime.Now, PaymentMode = "Razor Pay", PaymentDescription = "Payment for Registration" };
    //    //        //List<TblstudentPayment> payments = new List<TblstudentPayment>();
    //    //        //payments.Add(pay);
    //    //        TblstudentRegistration reg = new TblstudentRegistration()
    //    //        {
    //    //            Discount = r.Discount,
    //    //            FeeId = r.FeeId,
    //    //            RegistrationDate = r.RegistrationDate
    //    //            //    TblstudentPayments = payments


    //    //        };
    //    //        List<TblstudentRegistration> reglist = new List<TblstudentRegistration>();
    //    //        reglist.Add(reg);
    //    //        TblstudentDetail sd = new TblstudentDetail()
    //    //        {
    //    //            StudentName = r.StudentName,
    //    //            MobileNumber = r.MobileNumber,
    //    //            EmailAddress = r.EmailAddress,
    //    //            BirthDate = r.BirthDate,
    //    //            Gender = r.Gender,
    //    //            Password = r.Password,
    //    //            ProfilePhoto = r.ProfilePhoto,
    //    //            Qualification = r.Qualification,
    //    //            TblstudentRegistrations = reglist

    //    //        };
    //    //        await Task.Run(() => studentService.AddStudentDetails(sd));
    //    //        string message = "<h2>Dear<br/> " + r.StudentName + ",</h2><p>Your registration has been completed successfully.You can login by email address <b>" + r.EmailAddress + "</b> and password <b>" + r.Password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
    //    //        EmailModel em = new EmailModel()
    //    //        {
    //    //            EmailAddress = r.EmailAddress,
    //    //            Subject = "Registration Confirmation",
    //    //            Message = message,
    //    //            UserName = r.StudentName

    //    //        };
    //    //        await Task.Run(() => emailService.SendEmail(em));
    //    //        ViewBag.msg = "Your Account has been created successfully. Please check your registerd email address for further process";
    //    //        ViewBag.courses = await Task.Run(() => GetCourses());
    //    //        ViewBag.students = await Task.Run(() => studentService.GetStudentRegistrations());


    //    //        //string message2 = "Dear " + r.StudentName + ", Welcome to CIIT Training Institute Student Portal. You can access your account from https://ciitstudent.com/User/Login, with user=" + r.EmailAddress + " and password=" + r.Password + " . Regards, CIIT Training Institute";
    //    //        //  ExtraService.SendTransactionalSMS(r.MobileNumber, message2);
    //    //        //ViewBag.courses = GetCourses();
    //    //        //ViewBag.students = studentService.GetStudentRegistrations();
    //    //        //ViewBag.msg = "Your Account has been created successfully. You will get login credentials on your register mobile number.";
    //    //        return View();

    //    //    }
    //    //}

    //    public async Task<ActionResult> Success()
    //    {
    //        return View();
    //    }

    //    public async Task<ActionResult> Failed()
    //    {
    //        return View();
    //    }









    //    public async Task<JsonResult> GetFee(int fee_id)
    //    {
    //        CourseModel cm = await Task.Run(()=> courseService.GetFeeIdWiseCourse(fee_id));
    //        return Json(cm);
    //    }


    //    public async Task<IActionResult> NewRegistrationForm()
    //    {
    //        ViewBag.courses = await Task.Run(() => GetCourses());
    //        ViewBag.students = await Task.Run(() => studentService.GetStudentRegistrations());
    //        return View();
    //    }
    //    //public IActionResult NewRegistrationForm()
    //    //{
    //    //    ViewBag.courses =   GetCourses();
    //    //    ViewBag.students =  studentService.GetStudentRegistrations();
    //    //    return View();
    //    //}
    //}
}
}
