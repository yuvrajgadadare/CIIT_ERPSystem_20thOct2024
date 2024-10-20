using Microsoft.AspNetCore.Mvc;
using CIIT_NEW_WithCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
//using CIIT_NEW_WithCore.Services.Interfaces;
//using CIIT_NEW_WithCore.Services.Implementations;

namespace CIIT_NEW_WithCore.Controllers
{
    public class MainController : Controller
    {
        //IBranchService branchService;
        //IEnquiryService enquiryService;
        //public MainController(IEnquiryService enquiryService, IBranchService branchService)
        //{

        //    this.enquiryService = enquiryService;
        //    this.branchService = branchService;
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DropdownDemo()
        {
            return View();
        }
        //public List<SelectListItem> GetCenters()
        //{
        //    List<SelectListItem> lst = new List<SelectListItem>();
        //    foreach (Tblbranch b in branchService.GetBranches())
        //    {
        //        lst.Add(new SelectListItem() { Text = b.BranchName, Value = b.BranchId.ToString() });
        //    }
        //    return lst;
        //}

        ////[Route("enquiry-form")]
        //public async Task<ActionResult> EnquiryForm()
        //{
        //    EnquiryModel em = new EnquiryModel()
        //    {
        //        leadsources = await Task.Run(()=>  enquiryService.GetLeadSources()),
        //        enquiryfors =await Task.Run(()=> enquiryService.GetEnquiryFors()),
        //        topics = await Task.Run(()=> enquiryService.GetTrainingTopics())
        //    };
        //    ViewBag.qualifications = await Task.Run(()=> enquiryService.GetQualificaitons());
        //    ViewBag.branches = GetCenters();
        //    return View(em);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EnquiryForm(EnquiryModel e)
        //{
        //    await Task.Run(()=> { enquiryService.AddEnquiry(e); });

        //    ViewBag.msg = "Your enquiry is submitted successfully. Our representative will call you shortly";
        //    ModelState.Clear();

        //    EnquiryModel em = new EnquiryModel()
        //    {
        //        leadsources = await Task.Run(()=> enquiryService.GetLeadSources()),
        //        enquiryfors = await Task.Run(()=> enquiryService.GetEnquiryFors()),
        //        topics  = await Task.Run(()=> enquiryService.GetTrainingTopics())
        //    };
        //    ViewBag.qualifications = await Task.Run(()=> enquiryService.GetQualificaitons());
        //    ViewBag.branches = GetCenters();
        //    return View(em);
        //}


        ////[HttpPost]
        ////public IActionResult EnquiryForm(TblEnquiryFor enq)
        ////{
        ////    db.Add(enq);
        ////    db.SaveChanges();
        ////    return View();
        ////}

        ////public List<LeadSourceModel> GetLeadSources()
        ////{
        ////    List<LeadSourceModel> lst = new List<LeadSourceModel>();
        ////    foreach(TblleadSource s in db.TblleadSources.ToList())
        ////    {
        ////        LeadSourceModel lm = new LeadSourceModel()
        ////        {
        ////            SourceId = s.SourceId,
        ////            SourceName = s.SourceName,
        ////            IsSelected = false
        ////        };
        ////        lst.Add(lm);
        ////    }
        ////    return lst;
        ////}
        ////public List<EnquiryForModel> GetEnquiryFors()
        ////{
        ////    List<EnquiryForModel> lst = new List<EnquiryForModel>();
        ////    foreach (TblenquiryFor s in db.TblenquiryFors.ToList())
        ////    {
        ////        EnquiryForModel lm = new EnquiryForModel()
        ////        {
        ////             EnquiryForId = s.EnquiryForId ,
        ////             EnquiryFor = s.EnquiryFor,
        ////            IsSelected = false
        ////        };
        ////        lst.Add(lm);
        ////    }
        ////    return lst;
        ////}
        ////public List<TrainingTopicModel> GetTrainingTopics()
        ////{
        ////    List<TrainingTopicModel> lst = new List<TrainingTopicModel>();
        ////    foreach (TbltrainingTopic s in db.TbltrainingTopics.ToList())
        ////    {
        ////        TrainingTopicModel lm = new TrainingTopicModel()
        ////        {
        ////            TopicId = s.TopicId,
        ////            TopicName = s.TopicName,
        ////            IsSelected = false
        ////        };
        ////        lst.Add(lm);
        ////    }
        ////    return lst;
        ////}

        ////public List<SelectListItem> GetQualificaitons()
        ////{
        ////    List<SelectListItem> lst = new List<SelectListItem>();
        ////    foreach(Tblspecialization s in db.Tblspecializations.ToList())
        ////    {
        ////        Tblqualification q = db.Tblqualifications.Find(s.QualificationId);

        ////        SelectListItem st = new SelectListItem() { Text = q.Qualificaiton + "(" + s.Specialization + ")", Value = q.Qualificaiton + "(" + s.Specialization + ")" };
        ////        lst.Add(st);
        ////    }
        ////    return lst;
        ////}


        //public IActionResult OnlineTraining()
        //{
        //    return View();
        //}
    }
}
