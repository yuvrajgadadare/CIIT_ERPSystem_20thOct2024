namespace ERPSystem_Models
{
    public class EmployeeModel
    {
      public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_code { get; set; }
        public string email_address { get; set; }
        public string mobile_number { get; set; }
        public string profile_photo { get; set; }
        public DateTime birth_date { get; set; }
        public DateTime joining_date{ get; set; }
        public int  role_id{ get; set; }
        public string role_name { get; set; }
        public string password { get; set; }
        public float salary { get; set; }
    }
}
