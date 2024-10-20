using ERPSystem_Models;

namespace ERPSystem_Services.Interfaces
{
    public interface IEnquiryService
    {
        void AddEnquiry(EnquiryModel enquiry);
        void AddEnquiryFollowup(EnquiryFollowupModel m);
        void SendPromotionalMessage(EnquiryPromotionModel m);
        List<EnquiryModel> GetEnquiries();
       EnquiryModel GetEnquiry(int enquiry_id);
        List<EnquiryFollowupModel> GetEnquiryFollowups();
        List<EnquiryFollowupModel> GetEnquiryWiseFollowups(int enquiry_id);
        List<EnquiryPromotionModel> GetEnquiryPromotions();
        List<PromotionalMessageModel> GetPromotionMessages();
         PromotionalMessageModel GetPromotionMessage(int message_id);

    }
}
