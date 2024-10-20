 
using ERPSystem_Models;

namespace ERPSystem_Services.Interfaces
{
    public interface IExtraService
    {
        void SendEmail(EmailModel email, EmailSettings _settings);
        string SendTransactionalSMS(string mobile_number, string message);
        string Encrypt(string clearText);
        string Decrypt(string cipherText);
        string GetRandomPassword(int length);
        string ConvertAmount(double amount);
        string ConvertAmountInWord(long amount);
    }
}
