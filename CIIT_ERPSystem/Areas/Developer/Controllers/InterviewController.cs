using Microsoft.AspNetCore.Mvc;
using ERPSystem_Models;
using ERPSystem_Services;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class InterviewController : Controller
    {
        IQuestionService questionService;
        IMasterService masterService;
        public InterviewController(IQuestionService questionService, IMasterService masterService)
        {
            this.questionService = questionService;
            this.masterService = masterService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            InterviewQuestionModel im = new InterviewQuestionModel();
            return View();
        }
        public IActionResult AddInterviewQuestion()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");

            InterviewQuestionModel im = new InterviewQuestionModel();
            return View(im);
        }
        [HttpPost]
        public string AddQuestions(InterviewQuestionFormModel q)
        {
            questionService.AddInterviewQuestions(q.content_id, q.questions);
            return "Added Successfully";
        }

        public JsonResult GetQuestions()
        {
            List<InterviewQuestionModel> lst = questionService.GetAllInterviewQuestions();
            return Json(lst);
        }
        public JsonResult GetTopicWiseQuestions(int id)
        {
            List<InterviewQuestionModel> lst = questionService.GetTopicWiseInterviewQuestions(id);
            return Json(lst);
        }
        public JsonResult GetContentWiseQuestions(int id)
        {
            List<InterviewQuestionModel> lst = questionService.GetContentWiseInterviewQuestions(id);
            return Json(lst);
        }
    }
}
