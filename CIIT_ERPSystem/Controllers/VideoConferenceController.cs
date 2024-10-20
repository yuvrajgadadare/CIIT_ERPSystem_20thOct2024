using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIIT_ERPSystem.Controllers
{
    public class VideoConferenceController : Controller
    {
        IStudentService studentService;
        public VideoConferenceController(IStudentService studentService)
        {
            this.studentService = studentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StartMeeting()
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = studentService.GetStudent(student_id);
            ViewBag.name = student.student_name;
            return View();
        }
        public IActionResult JoinMeeting()
        {
            return View();
        }
  
    }
}
