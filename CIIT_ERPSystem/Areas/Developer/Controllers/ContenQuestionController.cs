using Microsoft.AspNetCore.Mvc;
using ERPSystem_Models;
using ERPSystem_Services;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class ContentQuestionController : Controller
    {
        IMasterService masterService;
        public ContentQuestionController(IMasterService masterService)
        {
            this.masterService = masterService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");

            return View();
        }
        public IActionResult AddQuestions()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");

            return View();
        }
        public JsonResult GetTopicWiseContents(int id)
        {
            return Json(masterService.GetTopicWiseContents(id));
        }
        public JsonResult GetTopicWiseQuestions(int id)
        {
            return Json(masterService.GetTopicWiseQuestions(id)); 
        
        }
        public JsonResult GetContentWiseQuestions(int id)
        {
            return Json(masterService.GetContentWiseQuestions(id));
        }

        public string AddContentQuestions(ContentModel c)
        {
            masterService.AddContentQuestion(c.content_id,c.contentQuestions);
            return "Questions Added Successfully";
        }
    }
}
