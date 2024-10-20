using ERPSystem_Models;
namespace ERPSystem_Services.Interfaces
{
    public interface IEmployeeService
    {
        List<EmployeeModel> GetEmployees();
         EmployeeModel  GetEmployee(int id);
         EmployeeModel  CheckEmployeeLogin(string employee_code,string password);
        void UpdateEmployeeDetails(EmployeeModel employee); 
        void ChangeEmployeeDetailPassword(EmployeeModel employee); 
        void ChangeProfilePhoto(EmployeeModel employee);
    }
}