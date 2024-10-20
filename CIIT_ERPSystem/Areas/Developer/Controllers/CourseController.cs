using Microsoft.AspNetCore.Mvc;
using ERPSystem_Models;
using ERPSystem_Services;
using ERPSystem_Services.Interfaces;
namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class CourseController : Controller
    {
        IMasterService masterService;
        public CourseController(IMasterService masterService)
        {
            this.masterService = masterService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            List<CourseModel> courses = masterService.GetTrainingCourses();
            return View(courses);
        }

        public JsonResult GetTopics()
        {
            return Json(masterService.GetTrainingTopics());
        }
        public JsonResult GetCourseTopics(int id)
        {
            return Json(masterService.GetCourseWiseTopics(id));
        }
        public JsonResult GetCourses()
        {
            return Json(masterService.GetTrainingCourses());
        }
        [HttpPost]
        public string AddCourseDetails(CourseModel cm)
        {
            masterService.AddTrainingCourse(cm);
            return "Course Added Successfully";
        }
        [HttpPost]
        public string AddCourseTopics(CourseModel course)
        {
            masterService.AddCourseTopics(course);
            return "Course Topics Added Successfully";
        }
        public IActionResult CourseSyllabus(int course_id)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            CourseModel cm=masterService.GetTrainingCourse(course_id);
            return View(cm);
        }
    }
}
