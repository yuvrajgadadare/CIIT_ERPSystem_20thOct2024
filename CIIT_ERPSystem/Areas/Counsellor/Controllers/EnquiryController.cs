
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NuGet.Configuration;

namespace CIIT_ERPSystem.Areas.Counsellor.Controllers
{
    [Area("Counsellor")]

    public class EnquiryController : Controller
    {
        IMasterService masterService;
        IEnquiryService enquiryService;
        IExtraService extraService;
        EmailSettings _settings;
        //public ExtraService(IOptions<EmailSettings> settings)
        //{
        //    _settings = settings.Value;
        //}
        public EnquiryController(IOptions<EmailSettings> settings,IMasterService masterService, IEnquiryService enquiryService,IExtraService extraService)
        {
            this.masterService = masterService;
            this.enquiryService = enquiryService;
            this.extraService = extraService;
            _settings = settings.Value;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            List<EnquiryModel> lst = enquiryService.GetEnquiries();
            return View(lst);
        }
        public IActionResult NewEnquiry()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            ViewBag.branches = GetBranches();
            ViewBag.qualifications = GetQualifications();
            EnquiryModel em = new EnquiryModel()
            {
                 enquiry_forList=masterService.GetEnquiryFors(),
                  lead_sourceList=masterService.GetLeadSources(),
                   topicList=masterService.GetTrainingTopics(),
            };

            return View(em);
        }
        [HttpPost]
        public IActionResult NewEnquiry(EnquiryModel enquiry)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string encryptedmobile = extraService.Encrypt(enquiry.mobile_number);
            //enquiry.mobile_number = encryptedmobile;

            // enquiry.enquiry_date = DateTime.Now;
            string enquiryfordata = "";
            foreach(EnquiryForModel e in enquiry.enquiry_forList)
            {
                if (e.is_selected)
                {
                    enquiryfordata += "," + e.enquiry_for;
                }
            }
            enquiryfordata=enquiryfordata.Substring(1,enquiryfordata.Length-1);


            string leadsourcedata = "";
            foreach (LeadSourceModel e in enquiry.lead_sourceList)
            {
                if (e.is_selected)
                {
                    leadsourcedata += "," + e.source_name;
                }
            }
            leadsourcedata = leadsourcedata.Substring(1, leadsourcedata.Length - 1);



            string topicdata = "";
            foreach (TopicModel e in enquiry.topicList)
            {
                if (e.is_selected)
                {
                    topicdata += "," + e.topic_name;
                }
            }
            topicdata = topicdata.Substring(1, topicdata.Length - 1);

            enquiry.enquiry_fors = enquiryfordata;
            enquiry.lead_sources=leadsourcedata;
            enquiry.interested_topics = topicdata;
            enquiry.status = "Enquiry Submitted";
            enquiryService.AddEnquiry(enquiry);
            EmailModel emodel = new EmailModel() { EmailAddress=enquiry.email_address, Message="Enquiry Submitted Succesfully", Subject="Enquiry", UserName=enquiry.candidate_name };
            extraService.SendEmail(emodel, _settings );
            ModelState.Clear();
            ViewBag.msg = "Enquiry Added Successfully";
            ViewBag.branches = GetBranches();
            ViewBag.qualifications = GetQualifications();
            EnquiryModel em = new EnquiryModel()
            {
                enquiry_forList = masterService.GetEnquiryFors(),
                lead_sourceList = masterService.GetLeadSources(),
                topicList = masterService.GetTrainingTopics(),
            };
            return View(em);
        }
        public string AddFolloup(EnquiryFollowupModel m)
        {
            enquiryService.AddEnquiryFollowup(m);
            return "Followup Submitted Succesfully";
        }
        public string AddPromotionalMessage(PromotionalModel m)
        {
            PromotionalMessageModel pmsg = enquiryService.GetPromotionMessage(m.message_id);
            string data = m.enquiry_ids;
            data = data.Substring(1, data.Length - 1);
            string[]ids=data.Split(',');
            foreach (string d in ids)
            {
                int enquiry_id=Convert.ToInt32(d);
                EnquiryModel enquiry=enquiryService.GetEnquiry(enquiry_id);
                EnquiryPromotionModel md = new EnquiryPromotionModel()
                {
                    message_id = m.message_id,
                    enquiry_id = enquiry_id,
                    sent_time = DateTime.Now
                };
                extraService.SendTransactionalSMS(enquiry.mobile_number,pmsg.message);

                enquiryService.SendPromotionalMessage(md);

            }
           //
            return "Succesfully Posted Messages";
        }
        public JsonResult GetEnquiry(int id)
        {
            EnquiryModel e=enquiryService.GetEnquiry(id);
            return Json(e);
        }
        public JsonResult GetEnquiryWiseFollowUps(int id)
        {
            List<EnquiryFollowupModel> e = enquiryService.GetEnquiryWiseFollowups(id);
            return Json(e);
        }
        public JsonResult GetPromotionalMessages()
        {
            List<PromotionalMessageModel> e = masterService.GetPromotionalMessages();

            return Json(e);
        }
        public SelectList GetBranches()
        {
            SelectList lst=new SelectList(masterService.GetBranches(),"branch_id","branch_name");
            return lst;
           
        }

        //public List<SelectListItem> GetQualifications()
        //{
        //    List<SelectListItem> lst=new List<SelectListItem>();
        //    foreach(QualificationModel q in masterService.GetQualifications())
        //    {
        //        SelectListItem s = new SelectListItem() { Text=q.qualification, Value=q.qualification_id.ToString() };
        //        lst.Add(s);
        //    }
        //    return lst;
        //}

        public SelectList GetQualifications()
        {
            SelectList lst = new SelectList(masterService.GetQualifications(), "qualification", "qualification");
            return lst;
        }



    }
}
