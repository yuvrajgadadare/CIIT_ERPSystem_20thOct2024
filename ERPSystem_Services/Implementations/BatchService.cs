
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPSystem_Services.Implementations
{
    public class BatchService : IBatchService
    {
        public void AddBatch(BatchModel batch)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@batch_id", batch.batch_id);
                cmd.Parameters.AddWithValue("@batch_name", batch.batch_name);
                cmd.Parameters.AddWithValue("@topic_id", batch.topic_id);
                cmd.Parameters.AddWithValue("@trainer_id", batch.trainer_id);
                cmd.Parameters.AddWithValue("@start_date", batch.start_date);
                cmd.Parameters.AddWithValue("@end_date", batch.end_date);
                cmd.Parameters.AddWithValue("@batch_time", batch.batch_time);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
            }
        public void AddBatchSchedule(BatchScheduleModel schedule)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@batch_schedule_id", schedule.batch_schedule_id);
                cmd.Parameters.AddWithValue("@batch_id", schedule.batch_id);
                cmd.Parameters.AddWithValue("@content_id", schedule.content_id);
                cmd.Parameters.AddWithValue("@expected_date", schedule.expected_date);
            //    cmd.Parameters.AddWithValue("@actual_date", schedule.actual_date);
        
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddBatchStudent(BatchStudentModel student)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch_student", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@batch_student_id", student.batch_student_id);
                cmd.Parameters.AddWithValue("@batch_id", student.batch_id);
                cmd.Parameters.AddWithValue("@registration_id", student.registration_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public List<BatchModel> GetAllBatches()
        {
            List<BatchModel> lst = new List<BatchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();

                    List<BatchScheduleModel> schedule = GetBatchWiseSchedule(id);
                    bool status = false;
                    if (schedule.Count() > 0)
                    {

                        status = true;
                    }

                    int cnt = 0;
                    List<BatchStudentModel> students = GetBatchWiseStudents(id);
                    if(students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    BatchModel bm = new BatchModel()
                    {
                        batch_id = id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                         is_schedule_generated=status,
                          student_count=cnt
                    };
                    lst.Add(bm);
                }
            }
                return lst;
            }
        public List<BatchScheduleModel> GetAllBatchSchedules()
        {

            List<BatchScheduleModel> lst = new List<BatchScheduleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatch_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_schedule_id = Convert.ToInt32(dr["batch_schedule_id"].ToString());
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    // DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    //  DateTime ?actual_date=null;
                    string expected_date = dr["expected_date"].ToString();

                    ////  DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    //string actual_date = dr["actual_date"].ToString();

                    if (expected_date != "")
                    {
                        expected_date = Convert.ToDateTime(expected_date).ToShortDateString();
                    }
                    //  DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    string actual_date = dr["actual_date"].ToString();
                    if (actual_date != "")
                    {
                        actual_date = Convert.ToDateTime(actual_date).ToShortDateString();
                    }

                    string status = "Not Conducted";
                    if (actual_date != "")
                    {
                        status = "Conducted";
                    }


                    string batch_time = dr["batch_time"].ToString();
                    BatchScheduleModel bm = new BatchScheduleModel()
                    {
                         batch_schedule_id=batch_schedule_id,
                        batch_id = batch_id,
                        batch_name = batch_name,
                      actual_date = actual_date,
                       content_id = content_id,
                        content_name=content_name,
                         expected_date=expected_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                         batch_time=batch_time,
                         status = status
                          
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public List<BatchStudentModel> GetAllBatchStudents()
        {

            List<BatchStudentModel> lst = new List<BatchStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatch_students", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_student_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();


                    BatchStudentModel bm = new BatchStudentModel()
                    {

                        batch_id = batch_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        batch_student_id = batch_student_id,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public List<TrainerModel> GetAllTrainers()
        {

            List<TrainerModel> lst = new List<TrainerModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltrainers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@trainer_id", 0);
                
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string gender = dr["gender"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();

                    TrainerModel bm = new TrainerModel()
                    {

                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                        email_address = email_address,
                        gender = gender,
                        mobile_number = mobile_number,
                        profile_photo = profile_photo,
                        qualification = qualification
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public BatchModel GetBatch(int id)
        {

            BatchModel  bm = new BatchModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbatches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int bid = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    int cnt = 0;
                    List<BatchStudentModel> students = GetBatchWiseStudents(id);
                    if (students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    bm = new BatchModel()
                    {
                        batch_id = bid,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                         student_count = cnt,
                    };
                     
                }
            }
            return bm;
        }
        public List<BatchScheduleModel> GetBatchWiseSchedule(int batch_id)
        {
            List<BatchScheduleModel> lst = new List<BatchScheduleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_wise_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_schedule_id = Convert.ToInt32(dr["batch_schedule_id"].ToString());
                    int b_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();

                    //DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    string expected_date =  dr["expected_date"].ToString() ;
                    if (expected_date != "")
                    {
                        expected_date=Convert.ToDateTime(expected_date).ToShortDateString();
                    }
                    //  DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                     string actual_date =  dr["actual_date"].ToString() ;
                    if (actual_date != "")
                    {
                        actual_date = Convert.ToDateTime(actual_date).ToShortDateString();
                    }
                    string status = "Not Conducted";
                    if (actual_date!="")
                    {
                        status = "Conducted";
                    }
                    string batch_time = dr["batch_time"].ToString();
                    BatchScheduleModel bm = new BatchScheduleModel()
                    {
                        batch_schedule_id = batch_schedule_id,
                        batch_id = b_id,
                        batch_name = batch_name,
                      //  actual_date = actual_date,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                         actual_date=actual_date,
                          batch_time = batch_time,
                           status = status,
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public  BatchScheduleModel  GetScheduleWiseSchedule(int batch_schedule_id)
        {
           BatchScheduleModel  st = new  BatchScheduleModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_schedule", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_schedule_id", batch_schedule_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //int batch_schedule_id = Convert.ToInt32(dr["batch_schedule_id"].ToString());
                    int b_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();

                    //DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    string expected_date = dr["expected_date"].ToString();
                    if (expected_date != "")
                    {
                        expected_date = Convert.ToDateTime(expected_date).ToShortDateString();
                    }
                    //  DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    string actual_date = dr["actual_date"].ToString();
                    if (actual_date != "")
                    {
                        actual_date = Convert.ToDateTime(actual_date).ToShortDateString();
                    }
                    string status = "Not Conducted";
                    if (actual_date != "")
                    {
                        status = "Conducted";
                    }
                    string batch_time = dr["batch_time"].ToString();
                    st = new BatchScheduleModel()
                    {
                        batch_schedule_id = batch_schedule_id,
                        batch_id = b_id,
                        batch_name = batch_name,
                        //  actual_date = actual_date,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                        actual_date = actual_date,
                        batch_time = batch_time,
                        status = status,
                    };
                     
                }
            }
            return st;
        }
        public List<BatchStudentModel> GetBatchWiseStudents(int batch_id)
        {
            List<BatchStudentModel> lst = new List<BatchStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_wise_students", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
                    int b_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    //DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); 
                    //int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string batch_time = dr["batch_time"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();


                    BatchStudentModel bm = new BatchStudentModel()
                    {

                        batch_id = b_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        start_date = start_date,
                        batch_student_id = batch_student_id,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
        public List<TopicStudentModel> GetTopicWiseStudents(int batch_id)
        {
            BatchModel bm = GetBatch(batch_id);
            List<BatchStudentModel> batchstudents = GetBatchWiseStudents(batch_id);
            List<TopicStudentModel> lst = new List<TopicStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_students", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", bm.topic_id   );
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    int tp_id = Convert.ToInt32(dr["topic_id"].ToString());

                    string student_name = dr["student_name"].ToString();
                    string course_name = dr["course_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    BatchStudentModel bst = batchstudents.FirstOrDefault(e => e.registration_id.Equals(registration_id));
                    if (bst == null)
                    {
                        TopicStudentModel ts = new TopicStudentModel()
                        {
                            registration_id = registration_id,
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            topic_id = tp_id,
                            topic_name = topic_name
                        };
                        lst.Add(ts);
                    }
                }
            }
            return lst;
        }
        public List<BatchModel> GetTrainerWiseBatches(int trainer_id)
        {
            List<BatchModel> lst = new List<BatchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_trainer_wise_batches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@trainer_id", trainer_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                  //  int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                    DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString());
                    string batch_time = dr["batch_time"].ToString();

                    List<BatchScheduleModel> schedule = GetBatchWiseSchedule(id);
                    bool status = false;
                    if (schedule.Count() > 0)
                    {

                        status = true;
                    }

                    int cnt = 0;
                    List<BatchStudentModel> students = GetBatchWiseStudents(id);
                    if (students.Count() > 0)
                    {
                        cnt = students.Count();
                    }
                    BatchModel bm = new BatchModel()
                    {
                        batch_id = id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        end_date = end_date,
                        start_date = start_date,
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                        is_schedule_generated = status,
                        student_count = cnt
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }

        public void MarkStudentScheduleAttendance(ScheduleAttendanceModel s)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("registration_id", typeof(int));
            dt.Columns.Add("is_present", typeof(int));
            dt.Columns.Add("flag", typeof(int));
            foreach(StudentAttendanceModel sam in s.students)
            {
                dt.Rows.Add(sam.registration_id, sam.is_present, 0);
            }
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblbatch_schedule_attendance", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@schedule_attendance_id", 0);
                cmd.Parameters.AddWithValue("@batch_schedule_id", s.batch_schedule_id);
                //cmd.Parameters.AddWithValue("@batch_schedule_id", s.batch_schedule_id);
                cmd.Parameters.AddWithValue("@attendance_date", s.attendance_date);
                cmd.Parameters.AddWithValue("@id", 0);
                cmd.Parameters.AddWithValue("@attendance", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
            }




        public List<StudentMarkAttendance> GetBatchWiseStudentAttendance(int batch_id, int registration_id)
        {
            List<StudentMarkAttendance> lst = new List<StudentMarkAttendance>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_and_batch_wise_attendance", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                  //  int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                   // int id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    DateTime expected_date = Convert.ToDateTime(dr["expected_date"].ToString());
                    DateTime actual_date = Convert.ToDateTime(dr["actual_date"].ToString());
                    DateTime attendance_date = Convert.ToDateTime(dr["attendance_date"].ToString());
                    int is_present = Convert.ToInt32(dr["is_present"].ToString());
                    string attendance = dr["attendance"].ToString();
                    StudentMarkAttendance bm = new StudentMarkAttendance()
                    {
                        actual_date = actual_date.ToShortDateString(),
                        attendance = attendance,
                        attendance_date = attendance_date.ToShortDateString(),
                        batch_id = batch_id,
                        batch_name = batch_name,
                        content_id = content_id,
                        content_name = content_name,
                        expected_date = expected_date.ToShortDateString(),
                        is_present = is_present,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name,
                        topic_id = topic_id,
                        topic_name = topic_name,

                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }

        public List<BatchStudentModel> GetStudentWiseBatches(int student_id)
        {
            List<BatchStudentModel> lst = new List<BatchStudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_student_wise_batches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int batch_student_id = Convert.ToInt32(dr["batch_student_id"].ToString());
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime start_date = Convert.ToDateTime(dr["start_date"].ToString());
                   // DateTime end_date = Convert.ToDateTime(dr["end_date"].ToString()); 
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int trainer_id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string trainer_name = dr["trainer_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    string batch_time = dr["batch_time"].ToString();
                    int s_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    List<BatchScheduleModel> schedule = GetBatchWiseSchedule(batch_id);
                    List<StudentMarkAttendance> attendane = GetBatchWiseStudentAttendance(batch_id, registration_id);
                    BatchStudentModel bm = new BatchStudentModel()
                    {

                        batch_id = batch_id,
                        batch_name = batch_name,
                        batch_time = batch_time,
                        start_date = start_date,
                        batch_student_id = batch_student_id,
                        registration_id = registration_id,
                        student_id = student_id,
                        student_name = student_name,
                        registration_date = registration_date,
                        status = "",
                        topic_id = topic_id,
                        topic_name = topic_name,
                        trainer_id = trainer_id,
                        trainer_name = trainer_name,
                        schedule= schedule,
                         attendance= attendane
                    };
                    lst.Add(bm);
                }
            }
            return lst;
        }
    }
}
