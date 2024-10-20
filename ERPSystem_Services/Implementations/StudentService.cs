using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPSystem_Services.Implementations
{
    public class StudentService : IStudentService
    {
        public void AddPayment(StudentPaymentModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@payment_id", p.payment_id);
                cmd.Parameters.AddWithValue("@registration_id", p.registration_id);
                cmd.Parameters.AddWithValue("@payment_date", p.payment_date);
                cmd.Parameters.AddWithValue("@payment_amount", p.payment_amount);
                cmd.Parameters.AddWithValue("@payment_mode", p.payment_mode);
                cmd.Parameters.AddWithValue("@payment_description", p.payment_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddStudentRegistration(StudentModel sm)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@student_id", sm.student_id);
                cmd.Parameters.AddWithValue("@student_name", sm.student_name);
                cmd.Parameters.AddWithValue("@last_name", sm.last_name);
                cmd.Parameters.AddWithValue("@gender", sm.gender);
                cmd.Parameters.AddWithValue("@mobile_number",sm.mobile_number);
                cmd.Parameters.AddWithValue("@whatsapp_number", sm.whatsapp_number);
                cmd.Parameters.AddWithValue("@email_address", sm.email_address);
                cmd.Parameters.AddWithValue("@local_address", sm.local_address);
                cmd.Parameters.AddWithValue("@permanent_address", sm.permanent_address);
                cmd.Parameters.AddWithValue("@password", sm.password);
                cmd.Parameters.AddWithValue("@birth_date", sm.birth_date);
                cmd.Parameters.AddWithValue("@profile_photo",sm.profile_photo);
                cmd.Parameters.AddWithValue("@qualification", sm.qualification);
                cmd.Parameters.AddWithValue("@parent_name", sm.parent_name);
                cmd.Parameters.AddWithValue("@parent_number", sm.parent_number);
                cmd.Parameters.AddWithValue("@student_code", sm.student_code);
                cmd.Parameters.AddWithValue("@permanent_identification_number", sm.permanent_identification_number);
                cmd.Parameters.AddWithValue("@aadhar_card_number", sm.aadhar_card_number);
                cmd.Parameters.AddWithValue("@aadhar_card_photo", sm.aadhar_card_photo);
                cmd.Parameters.AddWithValue("@id", 0);

                DataTable dt = new DataTable();
                dt.Columns.Add("registration_date", typeof(DateTime));
                dt.Columns.Add("discount", typeof(float));
                dt.Columns.Add("fee_id", typeof(int));
                foreach(RegistrationModel r in sm.registrations)
                {
                    dt.Rows.Add(r.registration_date,r.discount,r.fee_id);
                }

                cmd.Parameters.AddWithValue("@registration", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void UpdateStudentDetails(StudentModel sm)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@student_id", sm.student_id);
                cmd.Parameters.AddWithValue("@student_name", sm.student_name);
                cmd.Parameters.AddWithValue("@last_name", sm.last_name);
                cmd.Parameters.AddWithValue("@gender", sm.gender);
                cmd.Parameters.AddWithValue("@mobile_number", sm.mobile_number);
                cmd.Parameters.AddWithValue("@whatsapp_number", sm.whatsapp_number);
                cmd.Parameters.AddWithValue("@email_address", sm.email_address);
                cmd.Parameters.AddWithValue("@local_address", sm.local_address);
                cmd.Parameters.AddWithValue("@permanent_address", sm.permanent_address);
                cmd.Parameters.AddWithValue("@password", "");
                cmd.Parameters.AddWithValue("@birth_date", sm.birth_date);
                cmd.Parameters.AddWithValue("@profile_photo","");
                cmd.Parameters.AddWithValue("@qualification", sm.qualification);
                cmd.Parameters.AddWithValue("@parent_name", sm.parent_name);
                cmd.Parameters.AddWithValue("@parent_number", sm.parent_number);
                cmd.Parameters.AddWithValue("@student_code","");
                cmd.Parameters.AddWithValue("@permanent_identification_number", "");
                cmd.Parameters.AddWithValue("@aadhar_card_number", sm.aadhar_card_number);
                cmd.Parameters.AddWithValue("@aadhar_card_photo", "");
                cmd.Parameters.AddWithValue("@id", 0);

                DataTable dt = new DataTable();
                dt.Columns.Add("registration_date", typeof(DateTime));
                dt.Columns.Add("discount", typeof(float));
                dt.Columns.Add("fee_id", typeof(int));
     

                cmd.Parameters.AddWithValue("@registration", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<StudentPaymentModel> GetRegistrationWisePayments(int registration_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_registration_wise_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<StudentPaymentModel> GetStudentWisePayments(int student_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public StudentModel GetStudent(int id)
        {
            StudentModel sd = new StudentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_alltblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    List<RegistrationModel> registrations = GetStudentWiseRegistrations(student_id);
                    //List<StudentPaymentModel>payments=GetStudentWisePayments(student_id);
                    //float paidamount = 0, remaining_amount = 0;
                    //float total_amount = registrations[0].final_fees_amount;
                    //string status = "";
                    //if (payments.Count > 0)
                    //{
                    //    paidamount = payments.Sum(e => e.payment_amount);

                    //}
                    //remaining_amount = total_amount - paidamount;
                    //if (paidamount == 0)
                    //{
                    //    status = "Un Paid";
                    //}
                    //else if(paidamount>0 && paidamount<total_amount)
                    //{
                    //    status = "Partial Paid";

                    //}
                    //else
                    //{
                    //    status = "Paid";

                    //}
                    sd = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        registrations = registrations,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address =local_address,
                         permanent_address=permanent_address,
                        permanent_identification_number = permanent_identification_number,
                           whatsapp_number=whatsapp_number
                           
                         
                     
                    };

                }
                con.Close();
            }
            return sd;
        }
        public  StudentPaymentModel  GetStudentPayment(int payment_id)
        {
             StudentPaymentModel  st = new StudentPaymentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@payment_id", payment_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                  //  int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();

                    st = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name
                    };
                    
                }
                con.Close();
            }
            return st;
        }
        public List<StudentPaymentModel> GetStudentPayments()
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@payment_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description= dr["payment_description"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public List<StudentPaymentModel> GetStudentWisePreviousPayments(int registration_id, int payment_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_wise_previous_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                cmd.Parameters.AddWithValue("@payment_id", payment_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                //    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                //    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public List<StudentModel> GetStudents()
        {
            List<StudentModel> lst = new List<StudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    List<RegistrationModel> registrations = GetStudentWiseRegistrations(student_id);
                    
                        StudentModel e = new StudentModel()
                        {
                            student_id = student_id,
                            email_address = email_address,
                            birth_date = birth_date,
                            gender = gender,
                            mobile_number = mobile_number,
                            student_name = student_name,
                            profile_photo = profile_photo,
                            qualification = qualification,
                            parent_name = parent_name,
                            parent_number = parent_number,
                            student_code = student_code,
                            registrations = registrations,
                            aadhar_card_number = aadhar_card_number,
                            aadhar_card_photo = aadhar_card_photo,
                            last_name = last_name,
                            local_address = local_address,
                            permanent_address = permanent_address,
                            permanent_identification_number = permanent_identification_number,
                            whatsapp_number = whatsapp_number
                        };
                        lst.Add(e);
                    
                }
                con.Close();
            }
            return lst;
        }
        public List<StudentModel> GetAllStudents()
        {
            List<StudentModel> lst = new List<StudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_alltblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    List<RegistrationModel> registrations = GetStudentWiseRegistrations(student_id);

                    StudentModel e = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        registrations = registrations,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address = local_address,
                        permanent_address = permanent_address,
                        permanent_identification_number = permanent_identification_number,
                        whatsapp_number = whatsapp_number
                    };
                    lst.Add(e);

                }
                con.Close();
            }
            return lst;
        }
        public List<StudentModel> GetGuestStudents()
        {
            List<StudentModel> lst = new List<StudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_guest_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    List<RegistrationModel> registrations = GetStudentWiseRegistrations(student_id);

                    StudentModel e = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        registrations = registrations,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address = local_address,
                        permanent_address = permanent_address,
                        permanent_identification_number = permanent_identification_number,
                        whatsapp_number = whatsapp_number
                    };
                    lst.Add(e);

                }
                con.Close();
            }
            return lst;
        }

        public List<RegistrationModel> GetStudentWiseRegistrations(int student_id)
        {
            List<RegistrationModel> lst = new List<RegistrationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_studentWise_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = GetRegistrationWisePayments(registration_id);
                    float paid_amount = 0, remaining_amount = 0;
                   
                        if (payments.Count() > 0)
                        {
                            paid_amount = payments.Sum(e => e.payment_amount);
                        }
                        remaining_amount = final_fees_amount - paid_amount;
                        string status = "";

                        if (paid_amount == 0)
                        {
                            status = "Un Paid";
                        }
                        else if (paid_amount > 0 && paid_amount < final_fees_amount)
                        {
                            status = "Partial Paid";

                        }
                        else
                        {
                            status = "Paid";

                        }
                        string fees_mode = dr["fee_mode"].ToString();
                        DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                        RegistrationModel e = new RegistrationModel()
                        {
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            discount = discount,
                            fees_amount = fees_amount,
                            fees_mode = fees_mode,
                            fee_id = fee_id,
                            gst = gst,
                            registration_date = registration_date,
                            registration_id = registration_id,
                            final_fees_amount = final_fees_amount,
                            paid_amount = paid_amount,
                            remaining_amount = remaining_amount,
                            fees_status = status,
                            current_status=current_status


                        };
                        lst.Add(e);
                    
                }
                con.Close();
            }
            return lst;
        }

        public List<RegistrationModel> GetAllRegistrations()
        {
            List<RegistrationModel> lst = new List<RegistrationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = GetRegistrationWisePayments(registration_id);
                     
                        float paid_amount = 0, remaining_amount = 0;
                        if (payments.Count() > 0)
                        {
                            paid_amount = payments.Sum(e => e.payment_amount);
                        }
                        remaining_amount = final_fees_amount - paid_amount;
                        string status = "";

                        if (paid_amount == 0)
                        {
                            status = "Un Paid";
                        }
                        else if (paid_amount > 0 && paid_amount < final_fees_amount)
                        {
                            status = "Partial Paid";

                        }
                        else
                        {
                            status = "Paid";

                        }
                        string fees_mode = dr["fee_mode"].ToString();
                        DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                        RegistrationModel e = new RegistrationModel()
                        {
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            discount = discount,
                            fees_amount = fees_amount,
                            fees_mode = fees_mode,
                            fee_id = fee_id,
                            gst = gst,
                            registration_date = registration_date,
                            registration_id = registration_id,
                            final_fees_amount = final_fees_amount,
                            paid_amount = paid_amount,
                            remaining_amount = remaining_amount,
                            fees_status = status,
                            student_code = student_code,


                        };
                        lst.Add(e);
                     
                }
                con.Close();
            }
            return lst;
        }
        public List<RegistrationModel> GetAllGuestRegistrations()
        {
            List<RegistrationModel> lst = new List<RegistrationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_guest_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = GetRegistrationWisePayments(registration_id);

                    float paid_amount = 0, remaining_amount = 0;
                    if (payments.Count() > 0)
                    {
                        paid_amount = payments.Sum(e => e.payment_amount);
                    }
                    remaining_amount = final_fees_amount - paid_amount;
                    string status = "";

                    if (paid_amount == 0)
                    {
                        status = "Un Paid";
                    }
                    else if (paid_amount > 0 && paid_amount < final_fees_amount)
                    {
                        status = "Partial Paid";

                    }
                    else
                    {
                        status = "Paid";

                    }
                    string fees_mode = dr["fee_mode"].ToString();
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                    RegistrationModel e = new RegistrationModel()
                    {
                        student_id = student_id,
                        student_name = student_name,
                        course_id = course_id,
                        course_name = course_name,
                        discount = discount,
                        fees_amount = fees_amount,
                        fees_mode = fees_mode,
                        fee_id = fee_id,
                        gst = gst,
                        registration_date = registration_date,
                        registration_id = registration_id,
                        final_fees_amount = final_fees_amount,
                        paid_amount = paid_amount,
                        remaining_amount = remaining_amount,
                        fees_status = status,
                        student_code = student_code,


                    };
                    lst.Add(e);

                }
                con.Close();
            }
            return lst;
        }
        public  RegistrationModel GetRegistration(int id)
        {
          RegistrationModel st = new  RegistrationModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = GetRegistrationWisePayments(registration_id);
                    float paid_amount = 0, remaining_amount = 0;
                    
                        if (payments.Count() > 0)
                        {
                            paid_amount = payments.Sum(e => e.payment_amount);
                        }
                        remaining_amount = final_fees_amount - paid_amount;
                        string status = "";

                        if (paid_amount == 0)
                        {
                            status = "Un Paid";
                        }
                        else if (paid_amount > 0 && paid_amount < final_fees_amount)
                        {
                            status = "Partial Paid";

                        }
                        else
                        {
                            status = "Paid";

                        }
                        string fees_mode = dr["fee_mode"].ToString();
                        DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                        st = new RegistrationModel()
                        {
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            discount = discount,
                            fees_amount = fees_amount,
                            fees_mode = fees_mode,
                            fee_id = fee_id,
                            gst = gst,
                            registration_date = registration_date,
                            registration_id = registration_id,
                            final_fees_amount = final_fees_amount,
                            paid_amount = paid_amount,
                            remaining_amount = remaining_amount,
                            fees_status = status


                        };
                     
                   
                }
                con.Close();
            }
            return  st;
        }

        public RegistrationModel GetGuestRegistration(int id)
        {
            RegistrationModel st = new RegistrationModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_guest_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = GetRegistrationWisePayments(registration_id);
                    float paid_amount = 0, remaining_amount = 0;

                    if (payments.Count() > 0)
                    {
                        paid_amount = payments.Sum(e => e.payment_amount);
                    }
                    remaining_amount = final_fees_amount - paid_amount;
                    string status = "";

                    if (paid_amount == 0)
                    {
                        status = "Un Paid";
                    }
                    else if (paid_amount > 0 && paid_amount < final_fees_amount)
                    {
                        status = "Partial Paid";

                    }
                    else
                    {
                        status = "Paid";

                    }
                    string fees_mode = dr["fee_mode"].ToString();
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                    st = new RegistrationModel()
                    {
                        student_id = student_id,
                        student_name = student_name,
                        course_id = course_id,
                        course_name = course_name,
                        discount = discount,
                        fees_amount = fees_amount,
                        fees_mode = fees_mode,
                        fee_id = fee_id,
                        gst = gst,
                        registration_date = registration_date,
                        registration_id = registration_id,
                        final_fees_amount = final_fees_amount,
                        paid_amount = paid_amount,
                        remaining_amount = remaining_amount,
                        fees_status = status


                    };


                }
                con.Close();
            }
            return st;
        }

        public string NextPINNumber()
        {
            string pinnumber = "";
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select  erpuser.fun_nextPINNumber()", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    pinnumber = dr[0].ToString();
                }
                con.Close();
            }
            return pinnumber;
        }

        public void AddQualification(StudentQualificationModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@qualification_id", p.qualification_id);
                cmd.Parameters.AddWithValue("@student_id", p.student_id);
                cmd.Parameters.AddWithValue("@qualification", p.qualification);
                cmd.Parameters.AddWithValue("@passing_year", p.passing_year);
                cmd.Parameters.AddWithValue("@university",p.university);
                cmd.Parameters.AddWithValue("@medium", p.medium);
                cmd.Parameters.AddWithValue("@percentage",p.percentage);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateQualification(StudentQualificationModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@qualification_id", p.qualification_id);
                cmd.Parameters.AddWithValue("@student_id", p.student_id);
                cmd.Parameters.AddWithValue("@qualification", p.qualification);
                cmd.Parameters.AddWithValue("@passing_year", p.passing_year);
                cmd.Parameters.AddWithValue("@university", p.university);
                cmd.Parameters.AddWithValue("@medium", p.medium);
                cmd.Parameters.AddWithValue("@percentage", p.percentage);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteQualification(int qualification_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@qualification_id", qualification_id);
                cmd.Parameters.AddWithValue("@student_id", 0);
                cmd.Parameters.AddWithValue("@qualification", "");
                cmd.Parameters.AddWithValue("@passing_year",0);
                cmd.Parameters.AddWithValue("@university", "");
                cmd.Parameters.AddWithValue("@medium", "");
                cmd.Parameters.AddWithValue("@percentage",0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void RestoreQualification(int qualification_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.Parameters.AddWithValue("@type", "Restore");
                cmd.Parameters.AddWithValue("@qualification_id", qualification_id);
                cmd.Parameters.AddWithValue("@student_id", 0);
                cmd.Parameters.AddWithValue("@qualification", "");
                cmd.Parameters.AddWithValue("@passing_year", 0);
                cmd.Parameters.AddWithValue("@university", "");
                cmd.Parameters.AddWithValue("@medium", "");
                cmd.Parameters.AddWithValue("@percentage", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<StudentQualificationModel> GetStudentWiseQualifications(int student_id)
        {
            List<StudentQualificationModel> lst=new List<StudentQualificationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Student_Wise_qualifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                     int qualification_id = Convert.ToInt32(dr["qualification_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string university = dr["university"].ToString();
                    string medium = dr["medium"].ToString();
                    int passing_year =Convert.ToInt32( dr["passing_year"].ToString());
                    float percentage = (float)Convert.ToDouble(dr["percentage"].ToString());
                    StudentQualificationModel e = new StudentQualificationModel()
                    {
                        qualification_id = qualification_id,
                        percentage = percentage,
                        medium = medium,
                        passing_year = passing_year,
                        student_name = student_name,
                        university = university,
                        student_id = student_id,
                        qualification = qualification,
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public void ChangeStudentProfilePhoto(int student_id, string imgname)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_photo", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id",  student_id);
                cmd.Parameters.AddWithValue("@profile_photo", imgname);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void ChangeStudentAadharPhoto(int student_id, string imgname)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_aadharcardphoto", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                cmd.Parameters.AddWithValue("@aadhar_card_photo", imgname);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void ChangeStudentPassword(int student_id,string password)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_password", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                cmd.Parameters.AddWithValue("@new_password", password);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
