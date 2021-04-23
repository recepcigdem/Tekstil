using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Core.Helper.Login;
using Core.Utilities.Business;
using Entities.Concrete;

namespace Business.Concrete
{
    public class LoginManager : ILoginService
    {
        private IEmailService _emailService;
        private IStaffEmailService _staffEmailService;
        private IStaffService _staffService;

        public LoginManager(IEmailService emailService, IStaffEmailService staffEmailService, IStaffService staffService)
        {
            _emailService = emailService;
            _staffEmailService = staffEmailService;
            _staffService = staffService;
        }

        public DataServiceResult<Staff> Login(string email, string password)
        {
            var emailControl = EMailAddressControl(email);
            if (emailControl.Result == false)
                return new DataServiceResult<Staff>(false, emailControl.Message);

            var passwordControl = PasswordControl(password);
            if (passwordControl.Result == false)
                return new DataServiceResult<Staff>(false, passwordControl.Message);

            var createLoginToken = CreateLoginToken(email);
            if (createLoginToken.Result == false)
                return new DataServiceResult<Staff>(false, createLoginToken.Message);

            var dbLoginControl = DbLoginControl(email, password);
            if (dbLoginControl.Result == false)
                return new DataServiceResult<Staff>(false, dbLoginControl.Message);

            return new DataServiceResult<Staff>(dbLoginControl.Data, createLoginToken.Obj, true, "Login");
        }


        public DataServiceResult<Staff> DbLoginControl(string email, string password)
        {
            #region PasswordHash

            //Kullanıcının girdiği password encode ve salt işlemlerinden sonra hashlaniyor.
            //string salt = Core.Helper.PasswordHashSaltHelper.CreateSalt(4); 
            var encodePassword = Core.Helper.StringHelper.Base64Encode(password);
            var hashPassword = Core.Helper.PasswordHashSaltHelper.CreateHash256(encodePassword);

            #endregion

            var dbEmail = _emailService.GetByEmail(email);
            if (dbEmail.Success != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            var dbStaffEmail = _staffEmailService.GetByEmailId(dbEmail.Data.Id);
            if (dbStaffEmail.Success != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            if (dbStaffEmail.Data.IsMain == false)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailIsNotIsMain");

            var dbStaff = _staffService.GetById(dbStaffEmail.Data.StaffId);
            if (dbStaffEmail.Success != true)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotFound");

            if (dbStaff.Data.Password != hashPassword)
                return new DataServiceResult<Staff>(false, "Error.Login_WrongPassword");

            return new SuccessDataServiceResult<Staff>(dbStaff.Data, true, "ok");
        }

        public ServiceResult CreateLoginToken(string email)
        {
            var token = TokenHelper.CreateLoginToken(email);
            if (string.IsNullOrEmpty(token))
                return new ErrorServiceResult(false, "Error.Login_TokenNotFound");

            return new ServiceResult(true, "ok", token);
        }

        public ServiceResult EMailAddressControl(string email)
        {
            if (email == null)
                return new ErrorServiceResult(false, "Error.Login_WrongMail");

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
                return new ErrorServiceResult(false, "Error.Login_WrongMail");

            return new ServiceResult(true, "ok");
        }

        public ServiceResult PasswordControl(string password)
        {
            if (password == null)
                return new ErrorServiceResult(false, "Error.Login_WrongPassword");

            if (password.Length < 6)
                return new ErrorServiceResult(false, "Error.Login_WrongPassword");

            return new ServiceResult(true, "ok");
        }

        public ServiceResult ForgotPassword(string email)
        {
            var emailControl = EMailAddressControl(email);
            if (emailControl.Result == false)
                return new ServiceResult(false, emailControl.Message);

            var dbForgotPasswordControl = DbForgotPasswordControl(email);
            if (dbForgotPasswordControl.Result == false)
                return new ServiceResult(false, dbForgotPasswordControl.Message);

            var createLoginToken = CreateLoginToken(email);
            if (createLoginToken.Result == false)
                return new ServiceResult(false, createLoginToken.Message);

            

            return new ServiceResult(true, "ok",createLoginToken.Obj);
        }

        public DataServiceResult<Staff> DbForgotPasswordControl(string email)
        {

            var dbEmail = _emailService.GetByEmail(email);
            if (dbEmail.Success != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            var dbStaffEmail = _staffEmailService.GetByEmailId(dbEmail.Data.Id);
            if (dbStaffEmail.Success != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            if (dbStaffEmail.Data.IsMain == false)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailIsNotIsMain");

            var dbStaff = _staffService.GetById(dbStaffEmail.Data.StaffId);
            if (dbStaffEmail.Success != true)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotFound");

            if (dbStaff.Data.IsActive != true)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotActive");

            if (dbStaff.Data.IsLeaving)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotActive");

            return new SuccessDataServiceResult<Staff>(dbStaff.Data, true, "ok");
        }
    }
}
