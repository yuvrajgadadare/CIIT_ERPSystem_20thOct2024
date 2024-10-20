namespace ERPSystem_Models
{
    public class BatchModel
    {
        public int batch_id {  get; set; }
        public string batch_name {  get; set; }
        public int topic_id {  get; set; }
        public string topic_name {  get; set; }
        public int trainer_id {  get; set; }
        public string trainer_name {  get; set; }
        public DateTime start_date {  get; set; }
        public DateTime end_date {  get; set; }
        public string batch_time{  get; set; }
        public bool is_schedule_generated{  get; set; }
        public int student_count { get; set; }

    }
}
