using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class ProgramController : Controller
    {
        IQuestionService questionService;
        IMasterService masterService;
        public ProgramController(IQuestionService questionService, IMasterService masterService)
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
            ProgramQuestionModel im = new ProgramQuestionModel();
            return View();
        }
        public IActionResult AddProgramQuestion()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");

            ProgramQuestionModel im = new ProgramQuestionModel();

            return View(im);
        }
        [HttpPost]
        public string AddProgramQuestions(ProgramFormModel q)
        {
            foreach (ProgramQuestionModel p in q.questions)
            {
                questionService.AddProgramQuestion(p);
            }
            return "Added Successfully";
        }

        public JsonResult GetQuestions()
        {
            List<ProgramQuestionModel> lst = questionService.GetAllProgramQuestions();
            return Json(lst);
        }
        public JsonResult GetTopicWiseQuestions(int id)
        {
            List<ProgramQuestionModel> lst = questionService.GetTopicWiseProgramQuestions(id);
            return Json(lst);
        }
        public JsonResult GetContentWiseQuestions(int id)
        {
            List<ProgramQuestionModel> lst = questionService.GetContentWiseProgramQuestions(id);
            return Json(lst);
        }


        public IActionResult ProgramAnswer()
        {
            List<ProgramQuestionModel> lst = questionService.GetAllProgramQuestions();
            ViewBag.programs=new SelectList(lst, "program_question_id","question_title");
            ViewData["programsanswers"] = questionService.GetAllProgramAnswers();
            ProgramAnswerModel pm = new ProgramAnswerModel();
            return View(pm);
        }
        [HttpPost]
        public IActionResult ProgramAnswer(ProgramAnswerModel p)
        {
            questionService.AddProgramAnswer(p);
            ModelState.Clear();
            ViewBag.msg = "Program answer added successfully";
            List<ProgramQuestionModel> lst = questionService.GetAllProgramQuestions();
            ViewBag.programs = new SelectList(lst, "program_question_id", "question_title");
            ViewData["programsanswers"] = questionService.GetAllProgramAnswers();

            ProgramAnswerModel pm = new ProgramAnswerModel();
            return View(pm);
        }





    }
}
