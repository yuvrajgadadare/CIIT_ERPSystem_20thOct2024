 
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace ERPSystem_Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        public TrainerModel CheckTrainerLogin(string email_address, string password)
        {
            TrainerModel tr = null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_checktrainer_login", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email_address", email_address);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int id = Convert.ToInt32(dr["trainer_id"].ToString());
                    string name = dr["trainer_name"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string gender = dr["gender"].ToString();
                     tr = new TrainerModel()
                    {
                        email_address = email_address,
                        gender = gender,
                        mobile_number = mobile_number,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        trainer_id = id,
                        trainer_name = name
                    };
                  
                }
                 
                con.Close();
            }
            return tr;
        }
    }
}
