using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class TopicController : Controller
    {
        IExtraService extraService;
        IMasterService masterService;
        IWebHostEnvironment _environment;
        public TopicController(IExtraService extraService,IMasterService masterService, IWebHostEnvironment environment)
        {
            this.extraService = extraService;
            this.masterService = masterService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.topics = masterService.GetTrainingTopics();
            return View();
        }
        [HttpPost]
        public IActionResult Index(TopicModel topic)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            masterService.AddTopic(topic);
            ViewBag.msg = "Topic Added Successfully";
            ModelState.Clear();
            ViewBag.topics = masterService.GetTrainingTopics();

            return View();
        }
        public JsonResult GetTopics()
        {
            return Json(masterService.GetTrainingTopics());
        }
        public IActionResult Video()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            SelectList topics = new SelectList(masterService.GetTrainingTopics(),"topic_id","topic_name");
            ViewBag.topics = topics;
            TopicVideoModel t = new TopicVideoModel();
            ViewBag.videos = masterService.GetAllTopicVideos();
            return View(t);
        }
        [HttpPost]
        [RequestSizeLimit(1073741824)]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]

        //[RequestSizeLimit(2000L * 1024L * 1024L*1024L)]       //unit is bytes => 500Mb
        //[RequestFormLimits(MultipartBodyLengthLimit = 2000L * 1024L * 1024L * 1024L)]
        public IActionResult Video(TopicVideoModel v, IFormFile video)
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string filename = v.topic_id + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + Path.GetExtension(video.FileName);
            string folderpath = _environment.WebRootPath + "/Videos/" + v.topic_id;
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            string filepath = folderpath + "/" + filename;
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            //            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            video.CopyTo(fs);
            v.video_url = filename;
            masterService.AddTopicVideo(v);
            ViewBag.videos = masterService.GetAllTopicVideos();
            return Redirect("/Developer/Topic/Video");
        }
    }
}
