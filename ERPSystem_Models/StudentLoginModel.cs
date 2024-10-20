using System.ComponentModel.DataAnnotations;

namespace ERPSystem_Models
{
    public class StudentLoginModel
    {
        [Required(ErrorMessage ="*")]
        [EmailAddress(ErrorMessage ="invalid email address")]
        public string email_address {  get; set; }
        [Required(ErrorMessage ="*")]
        public string password {  get; set; }
    }
}
