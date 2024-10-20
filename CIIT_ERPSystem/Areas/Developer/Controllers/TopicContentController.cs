using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class TopicContentController : Controller
    {
        IMasterService masterService;
        public TopicContentController(IMasterService masterService)
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

        public JsonResult GetContents()
        {
            return Json(masterService.GetAllTopicContents());
        }
        public JsonResult GetTopicWiseContents(int topic_id)
        {
            return Json(masterService.GetTopicWiseContents(topic_id));
        }
        //public IActionResult AddContent()
        //{
        //    ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");

        //    return View();
        //}
        [HttpPost]
        public string AddContents(TopicModel t)
        {
            masterService.AddTopicContent(t);

            return "Contents Added Successfully";
        }
        [HttpPost]
        public string DeleteContent(int content_id)
        {
            masterService.DeleteTopicContent(content_id);
            return "Content Deleted Successfully";
        }
        //[HttpGet]
        //public JsonResult GetTopicContents(int id)
        //{
        //    return Json(GetTopicWiseContents(id));
        //}
    }
}
