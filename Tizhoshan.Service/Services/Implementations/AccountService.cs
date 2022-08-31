using System;
using System.Linq;
using Tizhoshan.DataLayer.Context;
using Tizhoshan.DataLayer.Models.Account;
using Tizhoshan.ServiceLayer.DTOs.AccountViewModels;
using Tizhoshan.ServiceLayer.ENUMs.UserENUMs;
using Tizhoshan.ServiceLayer.Services.Interfaces;
using Tizhoshan.ServiceLayer.Services.PublicClasses.Security;
using Tizhoshan.ServiceLayer.Services.PublicClasses.Sender;

namespace Tizhoshan.ServiceLayer.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly iVerificationSender _verificationSender;

        public AccountService(ApplicationDbContext context 
            ,iVerificationSender verificationSender)
        {
            _context = context;
            _verificationSender = verificationSender;
        }


        #region Users

        public User FindUserByPhoneNumber(string phoneNumber)
        {
            return _context.Users.Where(x => x.PhoneNumber == phoneNumber && x.IsDeleted == false).FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }



        #region account

        public string GenereateConfirmationCode()
        {
            Random r = new Random();
            return r.Next(20000, 99999).ToString();
        }

        public int RegisterUser(RegisterViewModel model)
        {
            if (_context.Users.Any(z => z.PhoneNumber == model.PhoneNumber))
            {
                return -100;
            }
            else
            {
                User user = new User();

                user.PhoneNumber = model.PhoneNumber.Trim();
                user.Password = PasswordHelper.EncodePasswordMd5(model.Password.Trim());
                user.CreateDate = DateTime.Now;
                user.AvatarName = "Default.jpg";
                user.DisplayName = model.DisplayName;
                user.IsDeleted = false;

                _context.Users.Add(user);
                _context.SaveChanges();
                return user.UserId;


            }
        }

        public PhoneNumberInUseEnum IsExistsPhoneNumber(string phoneNumber)
        {
            var user = FindUserByPhoneNumber(phoneNumber);
            if (user == null)
            {
                return PhoneNumberInUseEnum.Available;
            }
            if (user.PhoneNumberConfirmed)
            {
                return PhoneNumberInUseEnum.InUse;
            }
            if (!user.PhoneNumberConfirmed)
            {
                return PhoneNumberInUseEnum.InUseUnConfirmed;
            }
            return PhoneNumberInUseEnum.Available;

        }

        public RegisterUserConditionsEnum RegisterUser(RegisterViewModel model, string test = "")
        {
            var userNameCondition = IsExistsPhoneNumber(model.PhoneNumber);
            if (userNameCondition == PhoneNumberInUseEnum.InUse)
            {
                return RegisterUserConditionsEnum.AlternativePhone;
            }
            else if (userNameCondition == PhoneNumberInUseEnum.InUseUnConfirmed || userNameCondition == PhoneNumberInUseEnum.Available)
            {
                User user = null;
                if (userNameCondition == PhoneNumberInUseEnum.Available)
                {
                    user = new User()
                    {
                        PhoneNumber = model.PhoneNumber,
                        Password = PasswordHelper.EncodePasswordMd5(model.Password),
                        CreateDate = DateTime.Now,
                        AvatarName = "Default.jpg",
                        DisplayName = model.DisplayName,
                        IsDeleted = false,
                        ConfirmationCode = GenereateConfirmationCode(),
                        ConfirmationCodeDateTime = DateTime.Now
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();


                }
                else if (userNameCondition == PhoneNumberInUseEnum.InUseUnConfirmed)
                {
                    user = FindUserByPhoneNumber(model.PhoneNumber);
                    if (user.ConfirmationCodeDateTime.AddMinutes(1) > DateTime.Now)
                    {
                        return RegisterUserConditionsEnum.OneMinuteNeeded;
                    }
                    UpdateUserVerificationCodeByPhoneNumber(model.PhoneNumber);
                    user.Password = PasswordHelper.EncodePasswordMd5(model.Password);
                    user.DisplayName = model.DisplayName;
                    user.CreateDate = DateTime.Now;
                    user.ConfirmationCodeDateTime = DateTime.Now;
                    UpdateUser(user);

                }
                UpdateUserVerificationCodeByPhoneNumber(model.PhoneNumber);
                _verificationSender.Send(user.PhoneNumber, user.DisplayName,user.ConfirmationCode);
                return RegisterUserConditionsEnum.Registerd;
            }
            return RegisterUserConditionsEnum.Error;
        }

        public string UpdateUserVerificationCodeByPhoneNumber(string phoneNumber)
        {
            var user = FindUserByPhoneNumber(phoneNumber);
            if (user == null) return "";
            string newVerificationCode = GenereateConfirmationCode();
            user.ConfirmationCode = newVerificationCode;
            user.ConfirmationCodeDateTime = DateTime.Now;
            UpdateUser(user);
            return newVerificationCode;
        }


        public ConfirmPhoneRegisterEnum RegisterConfirmPhone(ConfirmPhoneViewModel model)
        {
            var iccuser = _context.Users.Where
                (x => x.PhoneNumber == model.PhoneNumber
                && x.ConfirmationCode == model.Code
               ).FirstOrDefault();
            if (iccuser != null)
            {
                if (iccuser.ConfirmationCodeDateTime.AddMinutes(5) < DateTime.Now)
                {
                    return ConfirmPhoneRegisterEnum.Expierd;
                }
                iccuser.PhoneNumberConfirmed = true;
                iccuser.ConfirmationCode = GenereateConfirmationCode();
                UpdateUser(iccuser);
                return ConfirmPhoneRegisterEnum.Confirmed;
            }
            return ConfirmPhoneRegisterEnum.ErrorConfirmed;
        }


        public RequestAnotherRegisterVerificationCodeEnum RequestAnotherRegisterVerificationCode(string phoneNumber)
        {
            var user = FindUserByPhoneNumber(phoneNumber);
            if (user == null)
            {
                return RequestAnotherRegisterVerificationCodeEnum.UserNotFound;
            }
            if (user.PhoneNumberConfirmed)
            {
                return RequestAnotherRegisterVerificationCodeEnum.UserIsActive;
            }
            if (user.ConfirmationCodeDateTime.AddMinutes(1) > DateTime.Now)
            {
                return RequestAnotherRegisterVerificationCodeEnum.NotAllowed;
            }
            string newVerificationCode = UpdateUserVerificationCodeByPhoneNumber(phoneNumber);
            bool res = _verificationSender.Send(phoneNumber, user.DisplayName, user.ConfirmationCode);
            if (res == true)
            {
                return RequestAnotherRegisterVerificationCodeEnum.NotSend;
            }
            return RequestAnotherRegisterVerificationCodeEnum.Sent;
        }


        public User GetUserForLogin(string phoneNumber, string pasword)
        {
            return _context.Users.Where(x => x.PhoneNumber == phoneNumber && x.Password == PasswordHelper.EncodePasswordMd5(pasword)).FirstOrDefault();
        }

        #endregion


        //public string GetUserFirstNameByUserName(string userName)
        //{
        //    return _context.Users.Where(x => x.UserName == userName).Select(x => x.FirstName).FirstOrDefault();
        //}


        //        public bool ChangeUserProfilePic(IFormFile file, string userName)
        //        {
        //            try
        //            {
        //                var user = GetUserByUserName(userName);

        //                //check file firs
        //                if (!ImageValidator.IsImage(file))
        //                {
        //                    return false;
        //                }
        //                if (file.Length > 15728640)
        //                {
        //                    return false;
        //                }
        //                //delete current pic
        //                if (user != null && !string.IsNullOrEmpty(user.IndexImage))
        //                {
        //                    if (System.IO.File.Exists(Directory.GetCurrentDirectory() + @"\wwwroot\images\profilePic\main\" + user.IndexImage + ""))
        //                    {
        //                        _iFileUploader.DeleteFileFromroot(@"\images\profilePic\main\" + user.IndexImage + "");
        //                    }
        //                }

        //                //upload main pic
        //                var fileName = _iFileUploader.Upload(new List<IFormFile>() { file }, @"\images\profilePic\utils\");
        //                if (fileName == null || fileName.Count() < 1)
        //                {
        //                    return false;
        //                }

        //                //delete first pic
        //                if (File.Exists((Directory.GetCurrentDirectory() + @"\wwwroot\images\profilePic\utils\" + fileName.First() + "")))
        //                {
        //                    _iFileUploader.DeleteFileFromroot(@"\images\profilePic\utils\" + fileName.First() + "");
        //                }

        //                user.IndexImage = fileName.First();
        //                _context.AplicationUser_TBL.Update(user);
        //                _context.SaveChanges();

        //                return true;
        //            }
        //            catch
        //            {
        //                return false;
        //            }

        //        }


        //        public void RemoveRoleFromUser(int userId, int RoleId)
        //        {
        //            MyUserRole userrole = _context.MyUserRole_TBL.Where(x => x.AplicationUserId == userId && x.RoleId == RoleId).FirstOrDefault();
        //            if (userrole != null)
        //            {
        //                _context.MyUserRole_TBL.Remove(userrole);
        //                _context.SaveChanges();
        //            }
        //        }


        //        public int ChangePassword(ChangePasswordViewModel model, string phoneNumber)
        //        {

        //            string hashOldPassword = PasswordHelper.EncodePasswordMd5(model.CurrentPassword);

        //            //اگر پسوورد قبلی وجود داشت
        //            if (_context.AplicationUser_TBL.Any(z => z.Password == hashOldPassword) == true)
        //            {
        //                var user = _context.AplicationUser_TBL.FirstOrDefault(u => u.UserName == phoneNumber);
        //                user.Password = PasswordHelper.EncodePasswordMd5(model.NewPassword.Trim());
        //                _context.AplicationUser_TBL.Update(user);
        //                _context.SaveChanges();
        //                return user.AplicationUserId;
        //            }

        //            return -1;
        //        }



        //        public BaseChangePasswordSendSMSEnum BaseChangePasswordSendSms(BaseChangePasswordViewModel model)
        //        {
        //            var user = FindUserByUserName(model.UserName);
        //            if (user == null)
        //            {
        //                return BaseChangePasswordSendSMSEnum.Error;
        //            }
        //            if (user.Status == false)
        //            {
        //                return BaseChangePasswordSendSMSEnum.NotAllowed;
        //            }
        //            if (user.PhoneNumberConfirmed == false)
        //            {
        //                return BaseChangePasswordSendSMSEnum.UnconfirmedPhone;
        //            }
        //            if (user.ConfirmationCodeDateTime.AddMinutes(1) > DateTime.Now)
        //            {
        //                return BaseChangePasswordSendSMSEnum.OneMiniuteError;
        //            }
        //            string verificationCode = UpdateUserVerificationCodeByUserName(model.UserName);
        //            _iVerificationSender.Send(verificationCode, model.UserName, 2);
        //            return BaseChangePasswordSendSMSEnum.Sent;
        //        }
        //        public RequestAnotherChangePasswordVerificationCodeEnum RequestAnotherChangePasswordVerificationCode(string userName)
        //        {
        //            var user = FindUserByUserName(userName);
        //            if (user == null)
        //            {
        //                return RequestAnotherChangePasswordVerificationCodeEnum.UserNotFound;
        //            }

        //            if (user.ConfirmationCodeDateTime.AddMinutes(1) > DateTime.Now)
        //            {
        //                return RequestAnotherChangePasswordVerificationCodeEnum.NotAllowed;
        //            }
        //            string newVerificationCode = UpdateUserVerificationCodeByUserName(userName);
        //            int res = _iVerificationSender.Send(newVerificationCode, userName, 1);

        //            if (res == -1)
        //            {
        //                return RequestAnotherChangePasswordVerificationCodeEnum.Error;
        //            }
        //            return RequestAnotherChangePasswordVerificationCodeEnum.Sent;
        //        }
        //        public UserNameInUseEnum IsExistsUserName(string userName)
        //        {
        //            var user = FindUserByUserName(userName);
        //            if (user == null)
        //            {
        //                return UserNameInUseEnum.Available;
        //            }
        //            if (user.PhoneNumberConfirmed)
        //            {
        //                return UserNameInUseEnum.InUse;
        //            }
        //            if (!user.PhoneNumberConfirmed)
        //            {
        //                return UserNameInUseEnum.InUseUnConfirmed;
        //            }
        //            return UserNameInUseEnum.Available;

        //        }
        //        public AplicationUser FindUserByUserName(string userName)
        //        {
        //            return _context.AplicationUser_TBL.Where(x => x.UserName == userName && x.IssDeleted == false).FirstOrDefault();
        //        }

        //        public AplicationUser FindUserById(long id)
        //        {
        //            return _context.AplicationUser_TBL.Find(id);
        //        }
        //        public string UpdateUserVerificationCodeByUserName(string userName)
        //        {
        //            var user = FindUserByUserName(userName);
        //            if (user == null) return "";
        //            string newVerificationCode = GenereateConfirmationCode();
        //            user.ConfirmationCode = newVerificationCode;
        //            user.ConfirmationCodeDateTime = DateTime.Now;
        //            UpdateUser(user);
        //            return newVerificationCode;
        //        }

        //        public int RegisterUserFromAdmin(RegisterViewModel model)
        //        {
        //            if (_context.AplicationUser_TBL.Any(z => z.UserName == model.PhoneNumber))
        //            {
        //                return -100;
        //            }
        //            else
        //            {
        //                AplicationUser user = new AplicationUser();
        //                user.PhoneNumber = model.PhoneNumber.Trim();
        //                user.UserName = model.PhoneNumber.Trim();
        //                user.DateRegistered = DateTime.Now;
        //                user.PhoneNumberConfirmed = true;
        //                user.NikName = model.NikName;
        //                user.Status = true;
        //                user.Password = PasswordHelper.EncodePasswordMd5(model.Password.Trim());
        //                try
        //                {
        //                    _context.AplicationUser_TBL.Add(user);
        //                    _context.SaveChanges();
        //                    return user.AplicationUserId;
        //                }
        //                catch
        //                {
        //                    return -1;
        //                }
        //            }
        //        }
        //        public string GetPhoneNumbyId(int id)
        //        {
        //            if (_context.AplicationUser_TBL.Any(x => x.AplicationUserId == id))
        //            {
        //                return _context.AplicationUser_TBL.Where(x => x.AplicationUserId == id).FirstOrDefault().PhoneNumber;
        //            }
        //            else
        //            {
        //                return "nok";
        //            }
        //        }
        //        public int GetIdByPhone(string phone)
        //        {
        //            if (_context.AplicationUser_TBL.Any(x => x.PhoneNumber == phone))
        //            {
        //                return _context.AplicationUser_TBL.Where(x => x.PhoneNumber == phone).FirstOrDefault().AplicationUserId;
        //            }
        //            else
        //            {
        //                return -1;
        //            }
        //        }
        //        public int GetIdByUserName(string userName)
        //        {
        //            if (_context.AplicationUser_TBL.Any(x => x.UserName == userName))
        //            {
        //                return _context.AplicationUser_TBL.Where(x => x.UserName == userName).FirstOrDefault().AplicationUserId;
        //            }
        //            else
        //            {
        //                return -1;
        //            }
        //        }
        //        public bool IsExistThisCodeForThisPhone(string phone, string code)
        //        {
        //            return _context.AplicationUser_TBL.Any(x => x.ConfirmationCode == code && x.AplicationUserId == GetIdByPhone(phone) && x.DateRegistered < DateTime.Now.AddMinutes(10));
        //        }
        //        public BaseChangePasswordFunctionEnum BaseChangePasswordFunction(BaseChangePasswordEnterNewViewModel model)
        //        {
        //            var user = FindUserByUserName(model.UserName);
        //            if (user == null)
        //            {
        //                return BaseChangePasswordFunctionEnum.UserNotFound;
        //            }
        //            if (user.ConfirmationCodeDateTime.AddMinutes(5) < DateTime.Now)
        //            {
        //                return BaseChangePasswordFunctionEnum.CodeExspierd;
        //            }
        //            if (user.ConfirmationCode != model.VerificationCode)
        //            {
        //                return BaseChangePasswordFunctionEnum.InvalidCode;
        //            }
        //            try
        //            {
        //                user.Password = PasswordHelper.EncodePasswordMd5(model.Password);
        //                UpdateUserVerificationCodeByUserName(model.UserName);
        //                UpdateUser(user);
        //                return BaseChangePasswordFunctionEnum.Changed;
        //            }
        //            catch
        //            {
        //                return BaseChangePasswordFunctionEnum.Error;
        //            }

        //        }
        //        public bool ConfirmPhoneNumber(string phone, string code)
        //        {
        //            if (IsExistThisCodeForThisPhone(phone, code))
        //            {
        //                _context.AplicationUser_TBL.Where(x => x.PhoneNumber == phone).SingleOrDefault().PhoneNumberConfirmed = true;
        //                _context.SaveChanges();
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        public AplicationUser GetUserByPhone(string phoneNum)
        //        {
        //            var user = _context.AplicationUser_TBL.FirstOrDefault(x => x.PhoneNumber == phoneNum);
        //            return user;
        //        }
        //        public AplicationUser GetUserByUserName(string userName)
        //        {
        //            //return _context.AplicationUser_TBL.Where(x => x.UserName == userName)
        //            //            .Include(x => x.Orders).SingleOrDefault();

        //            return _context.AplicationUser_TBL.Where(x => x.UserName == userName).SingleOrDefault();
        //        }
        //        public int GetUserIdByUserName(string userName)
        //        {
        //            //return _context.AplicationUser_TBL.Where(x => x.UserName == userName)
        //            //            .Include(x => x.Orders).Select(x => x.AplicationUserId).SingleOrDefault();

        //            return _context.AplicationUser_TBL.Where(x => x.UserName == userName).Select(x => x.AplicationUserId).SingleOrDefault();
        //        }
        //        public string GetUserNameByUserId(int id)
        //        {
        //            return _context.AplicationUser_TBL.Where(x => x.AplicationUserId == id)
        //                        .Select(x => x.UserName).SingleOrDefault();
        //        }
        //        public bool IsExistsThisPhoneNumber(string phoneNumber)
        //        {
        //            return _context.AplicationUser_TBL.Any(x => x.PhoneNumber == phoneNumber);
        //        }
        //        public bool IsCompoletePersonalInfo(string UserName)
        //        {
        //            var user = _context.AplicationUser_TBL.Any(x => x.UserName == UserName && !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.LastName));
        //            return _context.AplicationUser_TBL.Any(x => x.UserName == UserName && !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.LastName));

        //        }
        //        public SimplePersonalInformation GetUserPersonalInformationToShow(string usernam)
        //        {
        //            var user = _context.AplicationUser_TBL.Where(x => x.UserName == usernam).SingleOrDefault();
        //            return new SimplePersonalInformation()
        //            {
        //                Email = user.Email,
        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                UserName = user.UserName,
        //            };
        //        }
        //        public void UpdatePersonalInformation(SimplePersonalInformation model)
        //        {
        //            var user = _context.AplicationUser_TBL.Where(x => x.UserName == model.UserName).SingleOrDefault();

        //            if (!string.IsNullOrEmpty(model.FirstName))
        //            {
        //                user.FirstName = model.FirstName;
        //            }
        //            if (!string.IsNullOrEmpty(model.LastName))
        //            {
        //                user.LastName = model.LastName;
        //            }
        //            if (!string.IsNullOrEmpty(model.Email))
        //            {
        //                user.Email = model.Email;
        //            }

        //            _context.AplicationUser_TBL.Attach(user);
        //            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //            _context.SaveChanges();

        //        }
        //        public string[] GetUserFnameAndLnameAndImageForUserPasne(string usernam)
        //        {
        //            string[] res = { "----", "----", "default.png" };
        //            var user = _context.AplicationUser_TBL.Where(x => x.UserName == usernam).SingleOrDefault();
        //            if (user != null)
        //            {
        //                if (!string.IsNullOrEmpty(user.FirstName))
        //                {
        //                    res[0] = user.FirstName;
        //                }
        //                if (!string.IsNullOrEmpty(user.LastName))
        //                {
        //                    res[1] = user.LastName;
        //                }
        //                if (!string.IsNullOrEmpty(user.IndexImage))
        //                {
        //                    res[2] = user.IndexImage;
        //                }
        //            }
        //            return res;
        //        }
        //        public bool CanWeCheckPassword(ChangePasswordVieModel model, string username)
        //        {
        //            throw new NotImplementedException();
        //        }
        //        public AplicationUser GetUserById(int userId)
        //        {
        //            return _context.AplicationUser_TBL.Where(x => x.AplicationUserId == userId).FirstOrDefault();
        //        }
        //        public Tuple<List<AllUserViewModel>, int, int> GetAlluserForAdmin(string search, int page)//GetSystemUserForAdmin
        //        {
        //            IQueryable<AplicationUser> res = _context.AplicationUser_TBL.OrderByDescending(x => x.DateRegistered).Where(x => x.IssDeleted == false);
        //            if (!string.IsNullOrEmpty(search))
        //            {
        //                res = res.Where(x => x.UserName.Contains(search));
        //            }
        //            int take = 40;
        //            int pagecount = 0;
        //            if (res.Count() % take == 0)
        //            {
        //                pagecount = res.Count() / take;
        //            }
        //            else
        //            {
        //                pagecount = (res.Count() / take) + 1;
        //            }

        //            int skip = (page - 1) * take;

        //            if (page > 1 && res.Count() < 1)
        //            {
        //                page = page - 1;
        //            }
        //            var outPut = res.Skip(skip).Take(take).Select(x => new AllUserViewModel()
        //            {
        //                FirstName = x.FirstName,
        //                LastName = x.LastName,
        //                PhoneNumber = x.UserName,
        //                ConfirmedPhoneNumber = x.PhoneNumberConfirmed,
        //                AplicationUserId = x.AplicationUserId,
        //                status = x.Status
        //            }).ToList();


        //            return Tuple.Create(outPut, pagecount, page);
        //        }
        //        public Tuple<List<AllUserViewModel>, int, int> GetSystemUserForAdmin(string search, int page)
        //        {
        //            //IQueryable<AplicationUser> res = _context.AplicationUser_TBL;



        //            var res = (from user in _context.AplicationUser_TBL
        //                       join
        //                userRole in _context.MyUserRole_TBL on
        //                user.AplicationUserId equals userRole.AplicationUserId
        //                       join
        //role in _context.AplicationRole_TBL on userRole.RoleId
        //equals role.RoleId
        //                       where role.RoleId != 6
        //                       orderby user.DateRegistered descending
        //                       select user).Distinct();


        //            int take = 40;
        //            int pagecount = 0;
        //            if (res.Count() % take == 0)
        //            {
        //                pagecount = res.Count() / take;
        //            }
        //            else
        //            {
        //                pagecount = (res.Count() / take) + 1;
        //            }

        //            int skip = (page - 1) * take;
        //            if (!string.IsNullOrEmpty(search))
        //            {
        //                res = res.Where(x => x.UserName.Contains(search));
        //            }

        //            var outPut = res.Select(x => new AllUserViewModel()
        //            {
        //                FirstName = x.FirstName,
        //                LastName = x.LastName,
        //                PhoneNumber = x.UserName,
        //                ConfirmedPhoneNumber = x.PhoneNumberConfirmed,
        //                status = x.Status,
        //                AplicationUserId = x.AplicationUserId
        //            });
        //            List<AllUserViewModel> outPut1 = new List<AllUserViewModel>();
        //            foreach (var alluserr in outPut)
        //            {
        //                if (!outPut1.Any(x => x.AplicationUserId == alluserr.AplicationUserId))
        //                {
        //                    outPut1.Add(alluserr);
        //                }
        //            }
        //            outPut1 = outPut1.Skip(skip).Take(take).ToList();
        //            return Tuple.Create(outPut1, pagecount, page);
        //        }
        //        public EditUserFromAdminViewModel GetEditUserFromAdminViewModel(int userid)
        //        {
        //            var res = _context.AplicationUser_TBL.Where(x => x.AplicationUserId == userid).Select(x => new EditUserFromAdminViewModel()
        //            {
        //                Adress = x.Adress,
        //                Email = x.Email,
        //                FirstName = x.FirstName,
        //                LastName = x.LastName,
        //                Postalcode = x.PostalCode,
        //                UserName = x.UserName,
        //                UserId = x.AplicationUserId,
        //                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
        //                Status = x.Status

        //            }).FirstOrDefault();
        //            return res;
        //        }
        //        public void EditUserFromAdmin(EditUserFromAdminViewModel model)
        //        {
        //            var user = _context.AplicationUser_TBL.Find(model.UserId);
        //            if (user != null)
        //            {


        //                if (!string.IsNullOrEmpty(model.Adress))
        //                    user.Adress = model.Adress.Trim();


        //                if (!string.IsNullOrEmpty(model.Email))
        //                    user.Email = model.Email.Trim();

        //                if (!string.IsNullOrEmpty(model.FirstName))
        //                    user.FirstName = model.FirstName.Trim();

        //                if (!string.IsNullOrEmpty(model.LastName))
        //                    user.LastName = model.LastName.Trim();

        //                if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ReNewPassword) && model.ReNewPassword == model.NewPassword)
        //                {
        //                    user.Password = PasswordHelper.EncodePasswordMd5(model.NewPassword);
        //                    UpdateUser(user);

        //                }
        //                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
        //                user.Status = model.Status;
        //                _context.Update(user);
        //                _context.SaveChanges();
        //            }
        //        }
        //        public ShowUserForAdminModalViewModel GetUserInformationforAdminModal(int userId)
        //        {
        //            var user = _context.AplicationUser_TBL.Where(x => x.AplicationUserId == userId).FirstOrDefault();
        //            if (user != null)
        //            {
        //                ShowUserForAdminModalViewModel model = new ShowUserForAdminModalViewModel()
        //                {
        //                    Adress = user.Adress,
        //                    LastNam = user.LastName,
        //                    Email = user.Email,
        //                    UserName = user.UserName,
        //                    UserId = user.AplicationUserId,
        //                    FirstName = user.FirstName,
        //                    Postalcode = user.PostalCode,
        //                    IsConfirmedPhone = user.PhoneNumberConfirmed,
        //                    Status = user.Status
        //                };
        //                return model;
        //            }
        //            return null;
        //        }
        //        public void DisableEnableUSer(int id)
        //        {
        //            var user = _context.AplicationUser_TBL.Find(id);
        //            if (user != null)
        //            {
        //                if (user.Status == true)
        //                    user.Status = false;
        //                else
        //                    user.Status = true;
        //                _context.AplicationUser_TBL.Update(user);
        //                _context.SaveChanges();
        //            }
        //        }
        //        public void DeleteAndReverseUser(int id)
        //        {
        //            var user = _context.AplicationUser_TBL.Find(id);
        //            if (user != null)
        //            {
        //                if (user.IssDeleted == true)
        //                    user.IssDeleted = false;
        //                else
        //                    user.IssDeleted = true;
        //                _context.AplicationUser_TBL.Update(user);
        //                _context.SaveChanges();
        //            }
        //        }
        //        public bool PermissionSendCodeForBasePAssworgForget(int userId)
        //        {
        //            return !_context.AplicationUser_TBL.Any(x => x.AplicationUserId == userId && x.DateRegistered.AddMinutes(1) > DateTime.Now);
        //        }
        //        public bool PermisionSendAnotherCode(string userName)
        //        {
        //            if (!userName.StartsWith("0"))
        //            {
        //                userName = "0" + userName;
        //            }
        //            var userId = GetUserIdByUserName(userName);
        //            var res = _context.AplicationUser_TBL.Any(x => x.AplicationUserId == userId && x.ConfirmationCodeDateTime.AddMinutes(1) > DateTime.Now);
        //            return !res;
        //        }
        //        public void UpdateUser(AplicationUser user)
        //        {
        //            _context.AplicationUser_TBL.Update(user);
        //            _context.SaveChanges();
        //        }

        //public UserInfoViewModel GetUserInfoVeiwModel(string userName)
        //{
        //    return _context.AplicationUser_TBL.Where(x => x.UserName == userName).Select(x => new UserInfoViewModel()
        //    {
        //        FirstName = x.FirstName,
        //        LastName = x.LastName,
        //        UserName = x.UserName,
        //        UserId = x.AplicationUserId
        //    }).FirstOrDefault();
        //}
        //public UserInfoViewModel GetUserInfoVeiwModel(int userId)
        //{
        //    return _context.AplicationUser_TBL.Where(x => x.AplicationUserId == userId).Select(x => new UserInfoViewModel()
        //    {
        //        FirstName = x.FirstName,
        //        LastName = x.LastName,
        //        UserName = x.UserName,
        //        UserId = x.AplicationUserId,
        //        NikName = x.NikName
        //    }).FirstOrDefault();
        //}
        //public ProfileUserInfoViewModel GetProfileUserInfoViewModel(string userName)
        //{
        //    AplicationUser user = GetUserByUserName(userName);
        //    var info = _context.UserInfo_TBL.Include(x => x.AplicationUser).Where(x => x.ApplicationUserId == user.AplicationUserId).FirstOrDefault();
        //    if (info != null)
        //    {
        //        return new ProfileUserInfoViewModel()
        //        {
        //            Email = info.Email,
        //            FirstName = info.FirstName,
        //            LastName = info.LastName,
        //            UserId = info.ApplicationUserId,
        //            UserName = userName,
        //            NikName = info.AplicationUser.NikName,
        //            Adress = info.Adress
        //        };
        //    }
        //    else
        //    {
        //        return new ProfileUserInfoViewModel()
        //        {
        //            Email = "---",
        //            FirstName = "---",
        //            LastName = "---",
        //            Adress = "---",
        //            UserId = user.AplicationUserId,
        //            UserName = user.UserName,
        //            NikName = user.NikName
        //        };
        //    }
        //}
        #endregion

        //#region roles

        //public IEnumerable<SelectListItem> FillAddRoleDropDown()
        //{
        //    return _context.AplicationRole_TBL.Select(x => new SelectListItem()
        //    {
        //        Value = x.RoleId.ToString(),
        //        Text = x.RoleTitle
        //    }).ToList();
        //}
        //public List<ShowRoleViewModel> GetShowRoleViewModel()
        //{
        //    return _context.AplicationRole_TBL.OrderByDescending(x => x.RoleId).Select(x => new ShowRoleViewModel()
        //    {
        //        RoleTitle = x.RoleTitle,
        //        RoleId = x.RoleId
        //    }).Where(x => x.RoleId != 6).ToList();
        //}
        //public List<Tuple<string, int>> GetAllRolesToAddToUser()
        //{
        //    var a = _context.AplicationRole_TBL.Where(x => x.IsDelete == false && x.RoleId != 35).Select(x => new
        //    {
        //        x.RoleTitle,
        //        x.RoleId
        //    }).Where(x => x.RoleId != 6).ToList();
        //    List<Tuple<string, int>> res = new List<Tuple<string, int>>();
        //    foreach (var i in a)
        //    {

        //        res.Add(Tuple.Create(i.RoleTitle, i.RoleId));
        //    }
        //    return res;
        //}
        //public List<int> GetUserRoles(int id)
        //{
        //    return _context.MyUserRole_TBL
        //        .Include(x => x.AplicationRole)
        //          .Where(x => x.AplicationUserId == id)
        //            .Select(x => x.AplicationRole.RoleId).ToList();
        //}
        //public void AddRole(AddRoleViewModel model, List<int> SelectePermissions)
        //{
        //    AplicationRole role = new AplicationRole()
        //    {
        //        RoleTitle = model.RoleTitle
        //    };
        //    _context.AplicationRole_TBL.Add(role);
        //    _context.SaveChanges();
        //    foreach (var perm in SelectePermissions.Where(x => x != 35))
        //    {
        //        RolePermission rp = new RolePermission()
        //        {
        //            PermissionId = perm,
        //            RoleId = role.RoleId
        //        };
        //        _context.RolePermission_TBL.Add(rp);
        //    }
        //    _context.SaveChanges();
        //}
        //public void AddRoleToUser(int userId, int RoleId)
        //{
        //    MyUserRole userrole = new MyUserRole()
        //    {
        //        AplicationUserId = userId,
        //        RoleId = RoleId
        //    };
        //    _context.MyUserRole_TBL.Add(userrole);
        //    _context.SaveChanges();
        //}
        //public List<GetPermissionsViewModel> GetGetPermissionsViewModel()
        //{
        //    return _context.Permission_TBL.Where(x => x.PermissionId != 35).Select(x => new GetPermissionsViewModel
        //    {
        //        PermissionId = x.PermissionId,
        //        PermissionTitle = x.PermissionTitle,
        //        parent = x.ParentId
        //    }).ToList();
        //}
        //public Tuple<string, int> GetRoleTitleAndId(int roleId)
        //{
        //    var role = _context.AplicationRole_TBL.Where(x => x.RoleId == roleId).Select(x => new
        //    {
        //        x.RoleTitle,
        //        x.RoleId
        //    }).FirstOrDefault();
        //    return Tuple.Create(role.RoleTitle, role.RoleId);
        //}
        //public void DeleteAplicationRole(int id)
        //{

        //    if (id != 6)
        //    {
        //        foreach (var rolePerm in _context.RolePermission_TBL.Where(x => x.RoleId == id))
        //        {
        //            _context.RolePermission_TBL.Remove(rolePerm);
        //        }
        //        foreach (var userRole in _context.MyUserRole_TBL.Where(x => x.RoleId == id))
        //        {
        //            _context.MyUserRole_TBL.Remove(userRole);
        //        }
        //        var role = _context.AplicationRole_TBL.Where(x => x.RoleId == id).FirstOrDefault();
        //        if (role != null)
        //        {
        //            _context.AplicationRole_TBL.Remove(role);
        //            _context.SaveChanges();
        //        }
        //    }

        //}
        //public void EditRole(EditRoleViewModel model, List<int> SelectePermissions)
        //{
        //    if (model.RoleId != 6)
        //    {
        //        var role = _context.AplicationRole_TBL.Where(x => x.RoleId == model.RoleId).FirstOrDefault();
        //        if (role != null)
        //        {
        //            role.RoleTitle = model.RoleTitle;
        //            foreach (var rolePerm in _context.RolePermission_TBL.Where(x => x.RoleId == model.RoleId))
        //            {
        //                _context.RolePermission_TBL.Remove(rolePerm);
        //            }
        //            foreach (var perm in SelectePermissions.Where(x => x != 35))
        //            {
        //                RolePermission rp = new RolePermission()
        //                {
        //                    PermissionId = perm,
        //                    RoleId = role.RoleId
        //                };
        //                _context.RolePermission_TBL.Add(rp);
        //            }
        //            _context.SaveChanges();
        //        }
        //    }
        //}
        //public GetRoleAndPermissionsViewModel GetRoleAndPermissonsToEdit(int id)
        //{
        //    var role = _context.AplicationRole_TBL
        //         .Include(x => x.Permissions)
        //           .ThenInclude(x => x.Permission).Where(x => x.RoleId == id)
        //             .FirstOrDefault();

        //    GetRoleAndPermissionsViewModel model = new GetRoleAndPermissionsViewModel()
        //    {
        //        RoleId = role.RoleId,
        //        RoleTitle = role.RoleTitle,
        //        Permissions = new List<GetPermissionsViewModel>()
        //    };
        //    foreach (var rolePermission in role.Permissions)
        //    {
        //        Permission currentPerm = rolePermission.Permission;
        //        GetPermissionsViewModel curentgetperm = new GetPermissionsViewModel();

        //        curentgetperm.parent = currentPerm.ParentId;
        //        curentgetperm.PermissionId = currentPerm.PermissionId;
        //        curentgetperm.PermissionTitle = currentPerm.PermissionTitle;
        //        model.Permissions.Add(curentgetperm);
        //    }
        //    return model;
        //}
        //public void EditUsrRoles(int id, List<int> selectedRoles)//id is user Id
        //{
        //    if (_context.AplicationUser_TBL.Any(x => x.AplicationUserId == id))
        //    {
        //        foreach (var curentUserRole in _context.MyUserRole_TBL.Where(x => x.AplicationUserId == id && x.RoleId != 6).ToList())
        //        {
        //            _context.MyUserRole_TBL.Remove(curentUserRole);
        //        }
        //        foreach (var newUserRole in selectedRoles)
        //        {
        //            _context.MyUserRole_TBL.Add(new MyUserRole()
        //            {
        //                AplicationUserId = id,
        //                RoleId = newUserRole,
        //            });
        //        }
        //        _context.SaveChanges();
        //    }
        //}
        //public bool CheckPermisiion(int permissionId, string userName)
        //{
        //    int userId = _context.AplicationUser_TBL.Single(x => x.UserName == userName).AplicationUserId;
        //    List<int> userRoles = _context.MyUserRole_TBL.Where(x => x.AplicationUserId == userId).Select(x => x.RoleId).ToList();
        //    if (!userRoles.Any())
        //    {
        //        return false;
        //    }
        //    var flag = false;
        //    foreach (var _userrole in userRoles)
        //    {
        //        foreach (var _rolepermision in _context.RolePermission_TBL.Where(x => x.RoleId == _userrole).ToList())
        //        {
        //            if (_rolepermision.PermissionId == permissionId)
        //            {
        //                flag = true;
        //            }
        //        }
        //    }
        //    return flag;


        //}

        //#endregion

    }
}
