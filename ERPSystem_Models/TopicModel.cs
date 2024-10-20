namespace ERPSystem_Models
{
    public class TopicModel
    {
        public int topic_id {  get; set; }
        public string topic_name {  get; set; }
        public bool is_selected { get; set; }
        public List<ContentModel> contents { get; set; }

    }
}
