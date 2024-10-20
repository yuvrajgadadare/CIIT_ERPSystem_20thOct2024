 
using ERPSystem_Models;
using ERPSystem_Services.Interfaces;
using Microsoft.Data.SqlClient;
namespace ERPSystem_Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        public void ChangeEmployeeDetailPassword(EmployeeModel employee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_employee_password", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
                cmd.Parameters.AddWithValue("@password", employee.password);
                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public void ChangeProfilePhoto(EmployeeModel employee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_employee_photo", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
                cmd.Parameters.AddWithValue("@profile_photo", employee.profile_photo);
                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public EmployeeModel CheckEmployeeLogin(string employee_code, string password)
        {
            EmployeeModel st = new EmployeeModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_check_employee_login", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_code", employee_code);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int id = Convert.ToInt32(dr["employee_id"].ToString());

                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    int role_id = Convert.ToInt32(dr["role_id"].ToString());
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                    st = new EmployeeModel()
                    {
                        employee_id = id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                         profile_photo=profile_photo
                    };
                    con.Close();
                    return st;
                }
                else
                {
                    con.Close();
                    return null;
                }
                
            }
            
        }
        public EmployeeModel GetEmployee(int id)
        {
            EmployeeModel st = new EmployeeModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblemployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();

                    int role_id = Convert.ToInt32(dr["role_id"].ToString());
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                      st = new EmployeeModel()
                    {
                        employee_id = id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                         profile_photo=profile_photo
                    };
                     
                }
                con.Close();
            }
            return  st;
        }
        public List<EmployeeModel> GetEmployees()
        {
            List<EmployeeModel> lst = new List<EmployeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblemployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["employee_id"].ToString());
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    int role_id = Convert.ToInt32(dr["role_id"].ToString());
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                    EmployeeModel e = new EmployeeModel()
                    {
                        employee_id = id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                         profile_photo=profile_photo
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public void UpdateEmployeeDetails(EmployeeModel employee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblemployees_modify", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
                cmd.Parameters.AddWithValue("@employee_name", employee.employee_name);
                cmd.Parameters.AddWithValue("@email_address", employee.email_address);
                cmd.Parameters.AddWithValue("@mobile_number", employee.mobile_number);
                cmd.Parameters.AddWithValue("@birth_date", employee.birth_date);
                SqlDataReader dr = cmd.ExecuteReader();
            }
        }
    }
}