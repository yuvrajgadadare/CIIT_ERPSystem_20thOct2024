using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace CIIT_NEW_WithCore.Controllers
{
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
        public EnquiryController(IOptions<EmailSettings> settings, IMasterService masterService, IEnquiryService enquiryService, IExtraService extraService)
        {
            this.masterService = masterService;
            this.enquiryService = enquiryService;
            this.extraService = extraService;
            _settings = settings.Value;
        }
        public IActionResult Index()
        {
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
        [HttpPost]
        public IActionResult Index(EnquiryModel enquiry)
        {
           
            string enquiryfordata = "";
            foreach (EnquiryForModel e in enquiry.enquiry_forList)
            {
                if (e.is_selected)
                {
                    enquiryfordata += "," + e.enquiry_for;
                }
            }
            enquiryfordata = enquiryfordata.Substring(1, enquiryfordata.Length - 1);
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
            enquiry.lead_sources = leadsourcedata;
            enquiry.interested_topics = topicdata;
            enquiry.status = "Enquiry Submitted";
            enquiryService.AddEnquiry(enquiry);
            EmailModel emodel = new EmailModel()
            {
                EmailAddress = enquiry.email_address,
                Message = "<h2>Dear "+enquiry.candidate_name+ ",</h2><p>Thank you for inquiring about <b>"+enquiry.interested_topics+ "</b>.</p><p> We will be in touch in less than an hour to answer any questions you have.</p><p>Please feel free to check out courses  here <a href='https://ciitinstitute.com/' target='_blank'>CIIT Training Institute</a></p><br/><br/> <h2>Regards,</h2><h2>CIIT Training Institute Pvt Ltd</h2>",
                Subject = "Enquiry With CIIT Training Institute",
                UserName = enquiry.candidate_name
            };
            extraService.SendEmail(emodel, _settings);
            ModelState.Clear();
            ViewBag.msg = "Your enquiry has been submitted successfully.";
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
        public SelectList GetBranches()
        {
            SelectList lst = new SelectList(masterService.GetBranches(), "branch_id", "branch_name");
            return lst;
        }
        public SelectList GetQualifications()
        {
            SelectList lst = new SelectList(masterService.GetQualifications(), "qualification", "qualification");
            return lst;
        }
    }
}
