using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIIT_ERPSystem.Controllers
{
    public class QualificationController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IBatchService batchService;
        public QualificationController(IMasterService masterService, IStudentService studentService, IBatchService batchService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.batchService = batchService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = studentService.GetStudent(student_id);
            ViewBag.student = student;
            ViewBag.qualifications = studentService.GetStudentWiseQualifications(student_id);
            StudentQualificationModel q = new StudentQualificationModel() { student_id=student_id };
            return View(q);
        }
        [HttpPost]
        public IActionResult Index(StudentQualificationModel sq)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            studentService.AddQualification(sq);
            ViewBag.msg = "Qualification added successfully";
            ModelState.Clear();
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = studentService.GetStudent(student_id);
            ViewBag.student = student;
            ViewBag.qualifications = studentService.GetStudentWiseQualifications(student_id);
            StudentQualificationModel q = new StudentQualificationModel() { student_id = student_id };
            return View(q);
        }
    }
}
