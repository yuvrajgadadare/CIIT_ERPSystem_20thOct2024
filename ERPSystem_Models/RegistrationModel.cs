﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem_Models
{
    public class  RegistrationModel
    {
         public int registration_id {  get; set; }
         public int student_id {  get; set; }
         public  string student_code{  get; set; }
         public  string student_name{  get; set; }
        public DateTime registration_date {  get; set; }
        public string current_status { get; set; }
        public float discount {  get; set; }
        public int fee_id {  get; set; }
         public int course_id {  get; set; }
         public string course_name{  get; set; }
         public float fees_amount {  get; set; }
         public float final_fees_amount {  get; set; }
        public float paid_amount { get; set; }
        public float remaining_amount { get; set; }
        public string fees_status { get; set; }


        public float gst {  get; set; }
        public string fees_mode{  get; set; }
    }
}
