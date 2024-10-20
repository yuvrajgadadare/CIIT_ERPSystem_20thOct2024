using ERPSystem_Models;

namespace ERPSystem_Services.Interfaces
{
    public interface IMasterService
    {
        void AddTrainingCourse(CourseModel course); 
        void AddCourseTopics(CourseModel course);
        List<CourseModel> GetTrainingCourses();
        void AddTopic(TopicModel topic);
        void AddTopicContent(TopicModel topic);
        void UpdateTopic(TopicModel topic);
        void UpdateTopicContent(ContentModel topic);
        void DeleteTopic(int topic_id);
        void AddContentQuestion(int content_id, List<ContentQuestionModel> questions);
        void UpdateContentQuestion(ContentQuestionModel question);
        void DeleteContentQuestion(int question_id);
        void RestoreContentQuestion(int question_id);
        void DeleteTopicContent(int content_id);
        List<CourseFeeModel> GetCourseFees();
        CourseModel GetTrainingCourse(int course_id);
        List<CourseFeeModel> GetCourseWiseFees(int course_id);
        List<TopicModel> GetCourseWiseTopics(int course_id);
        List<TopicModel> GetTrainingTopics();
        List<ContentModel> GetTopicWiseContents(int topic_id);
        List<ContentModel> GetAllTopicContents();
        List<ContentQuestionModel> GetContentWiseQuestion(int content_id);
        List<BranchModel> GetBranches();
        List<LeadSourceModel> GetLeadSources();
        List<EnquiryForModel> GetEnquiryFors();
        List<PromotionalMessageModel> GetPromotionalMessages();
        List<QualificationModel> GetQualifications();
        void AddTopicVideo(TopicVideoModel vm);
        List<TopicVideoModel> GetAllTopicVideos();
        List<TopicVideoModel> GetTopicWiseVideos(int topic_id);
        TopicVideoModel  GetTopicVideo(int video_id);
        List<ContentQuestionModel> GetTopicWiseQuestions(int topic_id,int count);
        //List<ContentQuestionModel> GetTopicWiseQuestions(int topic_id);
        List<ContentQuestionModel> GetContentWiseQuestions(int content_id);
        List<ContentQuestionModel> GetTopicWiseQuestions(int topic_id);
        List<ContentQuestionModel> GetAllContentQuestions();
        StudentModel CheckStudentLogin(string email_address,string password);  
        
        void SubmitExam(ExamModel exam);
        List<ExamModel> GetAllExams();
        List<ExamModel> GetStudentWiseExams(int student_id);
        List<ExamQuestionModel> GetExamWiseQuestionResult(int exam_id);
        ExamModel GetExam(int exam_id);


        int ScheduleExamForStudent(ExamModel exam);
        void SubmitScheduledExam(ExamModel exam);
        void RejectScheduledExam(int exam_id);
        List<ExamModel> ViewAllScheduleExams();
        List<ExamModel> ViewAllSubmittedExams();
        List<ExamModel> ViewAllRejectedExams();




    }
}
