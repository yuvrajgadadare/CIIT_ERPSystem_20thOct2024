 
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace ERPSystem_Services.Implementations
{
    public class MasterService : IMasterService
    {
        public void AddTopicVideo(TopicVideoModel vm)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltopic_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@video_id",vm.video_id);
                cmd.Parameters.AddWithValue("@topic_id",vm.topic_id);
                cmd.Parameters.AddWithValue("@video_title", vm.video_title);
                cmd.Parameters.AddWithValue("@video_url", vm.video_url);
                cmd.Parameters.AddWithValue("@video_description", vm.video_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddTrainingCourse(CourseModel course)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@course_id", course.course_id);
                cmd.Parameters.AddWithValue("@course_name",course.course_name);
                cmd.Parameters.AddWithValue("@cid", 0);

                DataTable dt = new DataTable();
                dt.Columns.Add("fees_amount", typeof(float));
                dt.Columns.Add("gst", typeof(float));
                dt.Columns.Add("fee_mode", typeof(string));
                foreach(CourseFeeModel f in course.courseFees)
                {
                    dt.Rows.Add(f.fees_amount, f.gst, f.fee_mode);
                }
                cmd.Parameters.AddWithValue("@fee", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public StudentModel CheckStudentLogin(string email_address, string password)
        {
            StudentModel student = null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_checkStudentLogin", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email_Address", email_address);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime birth_date =Convert.ToDateTime( dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();

                    student = new StudentModel()
                    {
                        student_id = student_id,
                        student_name = student_name,
                        email_address = email_address,
                        gender = gender,
                        birth_date = birth_date,
                        mobile_number = mobile_number,
                        profile_photo = profile_photo,
                        qualification = qualification
                    };
                     
                }
                else
                {
                    student = null;
                }
                con.Close();
            }
            return student;
        }

        public List<ExamModel> GetAllExams()
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Submitted_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    //int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    //string student_name = dr["student_name"].ToString();
                    //string email_address = dr["email_address"].ToString();
                    //string mobile_number = dr["mobile_number"].ToString();
                    //DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    //DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    //DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    //int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    //int total_Questions = Convert.ToInt32(dr["total_Questions"].ToString());
                    //string topic_name = dr["topic_name"].ToString();
                    //string status = dr["status"].ToString();

                    //ExamModel e = new ExamModel()
                    //{
                    //    student_id = student_id,
                    //    email_address = email_address,
                    //    end_time = end_time,
                    //    exam_date = exam_date,
                    //    exam_id = eid,
                    //    mobile_number = mobile_number,
                    //    topic_id = topic_id,
                    //    start_time = start_time,
                    //    student_name = student_name,
                    //    topic_name = topic_name,
                    //     total_questions=total_Questions,
                    //    status= status
                    //};
                    //lst.Add(e);
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());

                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();

                    string topic_name = dr["topic_name"].ToString();

                    ExamModel st = null;
                    List<ExamQuestionModel> questions = GetExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status

                    };
                    lst.Add(st);
                }
                con.Close();
            }
            return lst;
        }

        public List<TopicVideoModel> GetAllTopicVideos()
        {
            List<TopicVideoModel> lst = new List<TopicVideoModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@video_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int video_id = Convert.ToInt32(dr["video_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string video_title = dr["video_title"].ToString();
                    string video_url = dr["video_url"].ToString();
                    string video_description = dr["video_description"].ToString();
                    string topic_name = dr["topic_name"].ToString();

                    TopicVideoModel e = new TopicVideoModel()
                    {
                        topic_id = topic_id,
                        video_description = video_description,
                        video_url = video_url,
                        video_title = video_title,
                        video_id = video_id,
                        topic_name = topic_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<BranchModel> GetBranches()
        {
            List<BranchModel> lst = new List<BranchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbranches",con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["branch_id"].ToString());
                    string name = dr["branch_name"].ToString();
                    BranchModel e = new BranchModel() {  branch_id = id,  branch_name = name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<CourseFeeModel> GetCourseWiseFees(int course_id)
        {
            List<CourseFeeModel> lst = new List<CourseFeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_coursewise_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    string fee_mode = dr["fee_mode"].ToString();
                    CourseFeeModel e = new CourseFeeModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        fees_amount = fees_amount,
                        fee_id = fee_id,
                        fee_mode = fee_mode,
                        gst = gst
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<CourseFeeModel> GetCourseFees()
        {
            List<CourseFeeModel> lst = new List<CourseFeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_course_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fee_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    string fee_mode = dr["fee_mode"].ToString();
                    CourseFeeModel e = new CourseFeeModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        fees_amount = fees_amount,
                        fee_id = fee_id,
                        fee_mode = fee_mode,
                        gst = gst
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<EnquiryForModel> GetEnquiryFors()
        {
            List <EnquiryForModel> lst=new List<EnquiryForModel>();
            using (SqlConnection con=new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquiry_fors", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_for_id"].ToString());
                    string name = dr["enquiry_for"].ToString();
                    EnquiryForModel e = new EnquiryForModel() { enquiry_for_id = id, enquiry_for = name ,is_selected=false };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public ExamModel GetExam(int exam_id)
        {
            ExamModel st = null;
            List<ExamQuestionModel> questions = GetExamWiseQuestionResult(exam_id);
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();


                    int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                    };

                }
                con.Close();
            }
            return st;
        }

        public List<ExamQuestionModel> GetExamWiseQuestionResult(int exam_id)
        {
            List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_exam_wise_result", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    int exam_question_id = Convert.ToInt32(dr["exam_question_id"].ToString());
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());
                    int submitted_option_number = Convert.ToInt32(dr["submitted_option_number"].ToString());

                    ExamQuestionModel e = new ExamQuestionModel()
                    {
                        correct_option_number = correct_option_number,
                        exam_id = eid,
                        exam_question_id = exam_question_id,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        question = question,
                        question_id = question_id,
                         submitted_option_number=submitted_option_number,
                          
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<LeadSourceModel> GetLeadSources()
        {
            List<LeadSourceModel> lst = new List<LeadSourceModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbllead_sources", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["source_id"].ToString());
                    string name = dr["source_name"].ToString();
                    LeadSourceModel e = new LeadSourceModel() {  source_id = id,  source_name = name, is_selected = false };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<PromotionalMessageModel> GetPromotionalMessages()
        {
            List<PromotionalMessageModel> lst = new List<PromotionalMessageModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblpromotional_messages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@message_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["message_id"].ToString());
                    string title = dr["message_title"].ToString();
                    string message = dr["message"].ToString();
                    PromotionalMessageModel e = new PromotionalMessageModel() {  message_id = id,  message_title = title, message=message };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<QualificationModel> GetQualifications()
        {
            List<QualificationModel> lst = new List<QualificationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblqualifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["qualification_id"].ToString());
                    string name = dr["qualification"].ToString();
                    QualificationModel e = new QualificationModel() {  qualification_id = id,  qualification = name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<ExamModel> GetStudentWiseExams(int student_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();

                    string topic_name = dr["topic_name"].ToString();

                    ExamModel st = null;
                    List<ExamQuestionModel> questions = GetExamWiseQuestionResult(exam_id);

                  //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status= status

                    };
                    lst.Add(st);
                }
                con.Close();
            }
            return lst;
        }

        public TopicVideoModel GetTopicVideo(int video_id)
        { 
            TopicVideoModel st = new TopicVideoModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@video_id", video_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                     
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string video_title = dr["video_title"].ToString();
                    string video_url = dr["video_url"].ToString();
                    string video_description = dr["video_description"].ToString();
                    string topic_name = dr["topic_name"].ToString();

                      st = new TopicVideoModel()
                    {
                        topic_id = topic_id,
                        video_description = video_description,
                        video_url = video_url,
                        video_title = video_title,
                        video_id = video_id,
                        topic_name = topic_name
                    };
                    
                }
                con.Close();
            }
            return st;
        }

        public List<ContentModel> GetTopicWiseContents(int topic_id)
        {
            List<ContentModel> lst = new List<ContentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_wise_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    ContentModel e = new ContentModel() { topic_id = t_id, topic_name = topic_name, content_id=content_id, content_name=content_name};
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<ContentModel> GetAllTopicContents()
        {
            List<ContentModel> lst = new List<ContentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    ContentModel e = new ContentModel() { topic_id = t_id, topic_name = topic_name, content_id = content_id, content_name = content_name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<ContentQuestionModel> GetTopicWiseQuestions(int topic_id, int count)
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topicwise_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                cmd.Parameters.AddWithValue("@size", count);
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());


                    ContentQuestionModel e = new ContentQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        correct_option_number = correct_option_number,
                        submitted_option_number = 0,
                        serial_number = i
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<ContentQuestionModel> GetTopicWiseQuestions(int topic_id)
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_content_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());


                    ContentQuestionModel e = new ContentQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        correct_option_number = correct_option_number,
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<ContentQuestionModel> GetContentWiseQuestions(int content_id)
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);

                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                //    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());


                    ContentQuestionModel e = new ContentQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        correct_option_number = correct_option_number,
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<ContentQuestionModel> GetAllContentQuestions()
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", 0);

                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());


                    ContentQuestionModel e = new ContentQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        correct_option_number = correct_option_number,
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<TopicVideoModel> GetTopicWiseVideos(int topic_id)
        {
            List<TopicVideoModel> lst = new List<TopicVideoModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@video_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int video_id = Convert.ToInt32(dr["video_id"].ToString());
                    string video_title = dr["video_title"].ToString();
                    string video_url = dr["video_url"].ToString();
                    string video_description = dr["video_description"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    TopicVideoModel e = new TopicVideoModel()
                    {
                        topic_id = topic_id,
                        video_description = video_description,
                        video_url = video_url,
                        video_title = video_title,
                        video_id = video_id,
                        topic_name = topic_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<CourseModel> GetTrainingCourses()
        {
            List<CourseModel> lst = new List<CourseModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    List<TopicModel> topics=GetCourseWiseTopics(course_id);
                    List<CourseFeeModel> fees=GetCourseWiseFees(course_id);
                    CourseModel e = new CourseModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        courseFees = fees,
                        topics = topics
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<TopicModel> GetTrainingTopics()
        {
            List<TopicModel> lst = new List<TopicModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("topic_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["topic_id"].ToString());
                    string name = dr["topic_name"].ToString();
                    TopicModel e = new TopicModel() {  topic_id = id,  topic_name = name, is_selected = false };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public void SubmitExam(ExamModel exam)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                //DataTable examtable = new DataTable();
                //examtable.Columns.Add("student_id", typeof(int));
                //examtable.Columns.Add("topic_id", typeof(int));
                //examtable.Columns.Add("exam_date", typeof(DateTime));
                //examtable.Columns.Add("start_time", typeof(DateTime));
                //examtable.Columns.Add("end_time", typeof(DateTime));
                //examtable.Rows.Add(exam.student_id, exam.topic_id, exam.exam_date, exam.start_time, exam.end_time);
                DataTable questionstable = new DataTable();
                questionstable.Columns.Add("question_id",typeof(int));
                questionstable.Columns.Add("submitted_option_number", typeof(int));
                foreach(ExamQuestionModel e in exam.examQuestions)
                {
                    questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@exam", examtable);
                cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                cmd.Parameters.AddWithValue("@question", questionstable);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public CourseModel GetTrainingCourse(int course_id)
        {
            CourseModel   st = new CourseModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    //int cid = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    List<CourseFeeModel>coursefees=GetCourseWiseFees(course_id);
                    List<TopicModel>topics=GetCourseWiseTopics(course_id);
                    st = new CourseModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                         courseFees = coursefees,
                          topics = topics
                    };
                    
                }
                con.Close();
            }
            return st;
        }

        public List<TopicModel> GetCourseWiseTopics(int course_id)
        {
            List<TopicModel> lst = new List<TopicModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_course_wise_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    List<ContentModel>contents=GetTopicWiseContents(t_id);
                    TopicModel e = new TopicModel() { topic_id = t_id, topic_name = topic_name,contents=contents};
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public void AddTopic(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                cmd.Parameters.AddWithValue("@topic_name", topic.topic_name);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddTopicContent(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_addtopic_wise_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                DataTable dt = new DataTable();
                dt.Columns.Add("content_name", typeof(string));
                foreach(ContentModel c in topic.contents)
                {
                    dt.Rows.Add(c.content_name);
                }
                cmd.Parameters.AddWithValue("@contents", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateTopic(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                cmd.Parameters.AddWithValue("@topic_name", topic.topic_name);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateTopicContent(ContentModel content)
        {

            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@content_id", content.content_id);
                cmd.Parameters.AddWithValue("@content_name", content.content_name);
                cmd.Parameters.AddWithValue("@topic_id", content.topic_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteTopic(int topic_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                cmd.Parameters.AddWithValue("@topic_name", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddContentQuestion(int content_id,List<ContentQuestionModel> questions)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);
              DataTable  dt=new DataTable();
                dt.Columns.Add("question",typeof(string));
                dt.Columns.Add("option1", typeof(string));
                dt.Columns.Add("option2", typeof(string));
                dt.Columns.Add("option3", typeof(string));
                dt.Columns.Add("option4", typeof(string));
                dt.Columns.Add("correct_option_number", typeof(int));
                foreach (ContentQuestionModel c in questions)
                {
                    dt.Rows.Add(c.question, c.option1, c.option2, c.option3, c.option4, c.correct_option_number);
                }
                cmd.Parameters.AddWithValue("@questions", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteTopicContent(int content_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@content_id", content_id);
                cmd.Parameters.AddWithValue("@content_name", "");
                cmd.Parameters.AddWithValue("@topic_id",0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<ContentQuestionModel> GetContentWiseQuestion(int content_id)
        {
            throw new NotImplementedException();
        }

        //public void AddContentQuestion(ContentQuestionModel question)
        //{
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_tbltopic_contents", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@type", "Delete");
        //        cmd.Parameters.AddWithValue("@content_id", content_id);
        //        cmd.Parameters.AddWithValue("@content_name", "");
        //        cmd.Parameters.AddWithValue("@topic_id", 0);
        //        int cnt = cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public void UpdateContentQuestion(ContentQuestionModel question)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_modify_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@question_id", question.question_id);
                cmd.Parameters.AddWithValue("@content_id", question.content_id);
                cmd.Parameters.AddWithValue("@question", question.question);
                cmd.Parameters.AddWithValue("@option1", question.option1);
                cmd.Parameters.AddWithValue("@option2", question.option2);
                cmd.Parameters.AddWithValue("@option3", question.option3);
                cmd.Parameters.AddWithValue("@option4", question.option4);
                cmd.Parameters.AddWithValue("@correct_option_number", question.correct_option_number);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteContentQuestion(int question_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_modify_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@question_id", question_id);
                cmd.Parameters.AddWithValue("@content_id",0);
                cmd.Parameters.AddWithValue("@question", "");
                cmd.Parameters.AddWithValue("@option1","");
                cmd.Parameters.AddWithValue("@option2", "");
                cmd.Parameters.AddWithValue("@option3", "");
                cmd.Parameters.AddWithValue("@option4", "");
                cmd.Parameters.AddWithValue("@correct_option_number", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void RestoreContentQuestion(int question_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_modify_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Restore");
                cmd.Parameters.AddWithValue("@question_id", question_id);
                cmd.Parameters.AddWithValue("@content_id", 0);
                cmd.Parameters.AddWithValue("@question", "");
                cmd.Parameters.AddWithValue("@option1", "");
                cmd.Parameters.AddWithValue("@option2", "");
                cmd.Parameters.AddWithValue("@option3", "");
                cmd.Parameters.AddWithValue("@option4", "");
                cmd.Parameters.AddWithValue("@correct_option_number", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddCourseTopics(CourseModel course)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_course_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course.course_id);
                DataTable dt = new DataTable();
                dt.Columns.Add("topic_id", typeof(int));
                foreach(TopicModel t in course.topics)
                {
                    dt.Rows.Add(t.topic_id);
                }
                cmd.Parameters.AddWithValue("@topics", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int ScheduleExamForStudent(ExamModel exam)
        {
            int exam_id = 0;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_schedule_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.Add("@exam_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@student_id",exam.student_id);
                cmd.Parameters.AddWithValue("@topic_id", exam.topic_id);
                cmd.Parameters.AddWithValue("@total_questions", exam.total_questions);
                cmd.Parameters.AddWithValue("@exam_date", exam.exam_date);
                cmd.Parameters.AddWithValue("@start_time", exam.start_time);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                //cmd.Parameters.AddWithValue("@exam_id", exam_id);
                object st = cmd.ExecuteScalar();
                exam_id = Convert.ToInt32(st);
                con.Close();
            }
            return exam_id;
        }

        public void SubmitScheduledExam(ExamModel exam)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_submit_scheduled_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                cmd.Parameters.AddWithValue("@exam_date", exam.exam_date);


                DataTable questionstable = new DataTable();
                questionstable.Columns.Add("question_id", typeof(int));
                questionstable.Columns.Add("submitted_option_number", typeof(int));
                foreach (ExamQuestionModel e in exam.examQuestions)
                {
                    questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                }
                cmd.Parameters.AddWithValue("@question", questionstable);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void RejectScheduledExam(int exam_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_reject_scheduled_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id",exam_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<ExamModel> ViewAllScheduleExams()
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_scheduled_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    //DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string status = dr["status"].ToString();
                    ExamModel e = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        end_time=end_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        status=status,
                        total_questions= total_questions
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<ExamModel> ViewAllSubmittedExams()
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Submitted_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());

                    string topic_name = dr["topic_name"].ToString();
                    string status = dr["status"].ToString();
                    ExamModel e = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        status = status,
                        total_questions=total_questions
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<ExamModel> ViewAllRejectedExams()
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Rejected_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    //DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());

                    string topic_name = dr["topic_name"].ToString();
                    string status = dr["status"].ToString();
                    ExamModel e = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        status = status,
                        total_questions=total_questions
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
    }
}
