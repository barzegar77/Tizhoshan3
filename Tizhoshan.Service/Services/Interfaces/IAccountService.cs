using System;
using System.Collections.Generic;
using Tizhoshan.DataLayer.Models.Account;
using Tizhoshan.ServiceLayer.DTOs.AccountViewModels;
using Tizhoshan.ServiceLayer.ENUMs.UserENUMs;

namespace Tizhoshan.ServiceLayer.Services.Interfaces
{
    public interface IAccountService
    {

        UserInfoViewModel GetUserPersonalInformationToShow(string phoneNumber);

        User FindUserByPhoneNumber(string phoneNumber);
        void UpdateUser(User user);
        string GenereateConfirmationCode();
        //int RegisterUser(RegisterViewModel model);
        PhoneNumberInUseEnum IsExistsPhoneNumber(string phoneNumber);
        RegisterUserConditionsEnum RegisterUser(RegisterViewModel model, string test = "");
        string UpdateUserVerificationCodeByPhoneNumber(string phoneNumber);
        ConfirmPhoneRegisterEnum RegisterConfirmPhone(ConfirmPhoneViewModel model);
        RequestAnotherRegisterVerificationCodeEnum RequestAnotherRegisterVerificationCode(string phoneNumber);
        User GetUserForLogin(string phoneNumber, string pasword);
        ForgotPasswordSendSMSEnum ForgotPasswordSendSms(ForgotPasswordViewModel model);
        bool ChangePassword(ChangePasswordViewModel model);
        RequestAnotherChangePasswordVerificationCodeEnum RequestAnotherChangePasswordVerificationCode(string phoneNumber);


        #region admin

        Tuple<List<UsersListForAdminViewModel>, int, int> GetUsersListForAdmin(string search, int page);
        int RegisterUserFromAdmin(CreateUserViewModel model);

        #endregion
    }
}
