using Tizhoshan.DataLayer.Models.Account;
using Tizhoshan.ServiceLayer.DTOs.AccountViewModels;
using Tizhoshan.ServiceLayer.ENUMs.UserENUMs;

namespace Tizhoshan.ServiceLayer.Services.Interfaces
{
    public interface IAccountService
    {
        User FindUserByPhoneNumber(string phoneNumber);
        void UpdateUser(User user);
        string GenereateConfirmationCode();
        //int RegisterUser(RegisterViewModel model);
        PhoneNumberInUseEnum IsExistsPhoneNumber(string phoneNumber);
        RegisterUserConditionsEnum RegisterUser(RegisterViewModel model, string test = "");
        string UpdateUserVerificationCodeByPhoneNumber(string phoneNumber);
    }
}
