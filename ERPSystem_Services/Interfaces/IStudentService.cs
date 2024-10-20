using ERPSystem_Models;
namespace ERPSystem_Services.Interfaces
{
    public interface IStudentService
    {
        string NextPINNumber();
        void AddStudentRegistration(StudentModel sm);
        void UpdateStudentDetails(StudentModel sm);
        public List<StudentModel> GetStudents();
        List<StudentModel> GetAllStudents();
        public List<StudentModel> GetGuestStudents();
        StudentModel GetStudent(int id);
        public List<RegistrationModel> GetStudentWiseRegistrations(int student_id);
        public List<RegistrationModel> GetAllRegistrations();
        public List<RegistrationModel> GetAllGuestRegistrations();
        public RegistrationModel GetGuestRegistration(int id);
        public RegistrationModel GetRegistration(int id);
        public List<StudentPaymentModel> GetStudentPayments();
        public  StudentPaymentModel  GetStudentPayment(int payment_id);
        public List<StudentPaymentModel> GetStudentWisePreviousPayments(int registration_id,int payment_id);
        public List<StudentPaymentModel> GetStudentWisePayments(int student_id);
        List<StudentPaymentModel> GetRegistrationWisePayments(int registration_id);

        public void AddPayment(StudentPaymentModel p);
        public void AddQualification(StudentQualificationModel p);
        public void UpdateQualification(StudentQualificationModel p);
        public void DeleteQualification(int qualification_id);
        public void RestoreQualification(int qualification_id);
        List<StudentQualificationModel> GetStudentWiseQualifications(int student_id);
          void ChangeStudentProfilePhoto(int student_id,string imgname);
        void ChangeStudentAadharPhoto(int student_id,string imgname);
        void ChangeStudentPassword(int student_id, string password);


    }
}
