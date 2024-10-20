using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem_Models
{
    public class StudentPaymentModel
    {
        public int payment_id { get; set; }
        public int student_id { get; set; }
        public string student_code { get; set; }
        public int registration_id { get; set; }
        public int course_id { get; set; }
        public DateTime payment_date { get; set; }
        public float payment_amount { get; set; }
        public string student_name { get; set; }
        public string course_name { get; set; }
        public string payment_mode { get; set; }
        public string payment_description { get; set; }
    }
}
