using ERPSystem_Models;

namespace ERPSystem_Services.Interfaces
{
    public interface ITrainerService
    {
        TrainerModel CheckTrainerLogin(string email_address, string password);
    }
}
