using Business.Abstract;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Helper.Login;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Entities.Concrete;


namespace Business.Concrete
{
    public class LoginManager : ILoginService
    {
        private IEmailService _emailService;
        private IStaffEmailService _staffEmailService;
        private IStaffService _staffService;
        private IAuthorizationService _authorizationService;
       

        public LoginManager(IEmailService emailService, IStaffEmailService staffEmailService, IStaffService staffService, IAuthorizationService authorizationService)
        {
            _emailService = emailService;
            _staffEmailService = staffEmailService;
            _staffService = staffService;
            _authorizationService = authorizationService;
            
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public DataServiceResult<Staff> Login(string email, string password)
        {
            var emailControl = EMailAddressControl(email);
            if (emailControl.Result == false)
                return new DataServiceResult<Staff>(false, emailControl.Message);

            var passwordControl = PasswordControl(password);
            if (passwordControl.Result == false)
                return new DataServiceResult<Staff>(false, passwordControl.Message);

            var createLoginToken = CreateToken(email);
            if (createLoginToken.Result == false)
                return new DataServiceResult<Staff>(false, createLoginToken.Message);

            var dbLoginControl = DbLoginControl(email, password);
            if (dbLoginControl.Result == false)
                return new DataServiceResult<Staff>(false, dbLoginControl.Message);

            var roles = GetRoles(dbLoginControl.Data);
            if (roles.Result == false)
                return new DataServiceResult<Staff>(false, dbLoginControl.Message);

            dbLoginControl.Data.OperationClaims = roles.Data;

            return new DataServiceResult<Staff>(dbLoginControl.Data,createLoginToken.Obj ,true, "Login");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public DataServiceResult<Staff> DbLoginControl(string email, string password)
        {
            var emailControl = DbEmailControl(email);
            if (emailControl.Result == false)
                return new DataServiceResult<Staff>(false, emailControl.Message);

            var passwordHashing = PasswordHashing(password, emailControl.Data.PasswordSalt);
            if (passwordHashing.Result == false)
                return new DataServiceResult<Staff>(false, passwordHashing.Message);

            string passwordObj = passwordHashing.Obj.ToString();

            if (emailControl.Data.Password != passwordObj)
                return new DataServiceResult<Staff>(false, "Error.Login_WrongPassword");

            return new SuccessDataServiceResult<Staff>(emailControl.Data, true, "ok");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public DataServiceResult<Staff> DbEmailControl(string email)
        {
            
            var dbEmail = _emailService.GetByEmail(email);
            if (dbEmail.Result != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            var dbStaffEmail = _staffEmailService.GetByEmailId(dbEmail.Data.Id);
            if (dbStaffEmail.Result != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            if (dbStaffEmail.Data.IsMain == false)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailIsNotIsMain");

            var dbStaff = _staffService.GetById(dbStaffEmail.Data.StaffId);
            if (dbStaff.Result != true)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotFound");

            return new SuccessDataServiceResult<Staff>(dbStaff.Data, dbStaff.Data.PasswordSalt, true, "ok");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public ServiceResult CreateToken(string email)
        {
            var token = TokenHelper.CreateLoginToken(email);
            if (string.IsNullOrEmpty(token))
                return new ErrorServiceResult(false, "Error.Login_TokenNotFound");

            return new ServiceResult(true, "ok", token);
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
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

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public ServiceResult PasswordControl(string password)
        {
            if (password == null)
                return new ErrorServiceResult(false, "Error.Login_WrongPassword");

            if (password.Length > 0 && password.Length < 6)
                return new ErrorServiceResult(false, "Error.Login_PasswordLengthMin6");

            if (password.Length > 0 && password.ToUpper(new CultureInfo("tr-TR", false)) == password)
                return new ErrorServiceResult(false, "Error.Login_PasswordMustHaveUpperLowerNumberControl");

            if (password.Length > 0 && password.ToLower(new CultureInfo("tr-TR", false)) == password)
                return new ErrorServiceResult(false, "Error.Login_PasswordMustHaveUpperLowerNumberControl");

            if (password.Length > 0 && !password.Any(char.IsDigit))
                return new ErrorServiceResult(false, "Error.Login_PasswordMustHaveUpperLowerNumberControl");

            return new ServiceResult(true, "ok");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public ServiceResult PasswordHashing(string password, string userDbSalt)
        {
            if (string.IsNullOrWhiteSpace(userDbSalt))
            {
                userDbSalt = Core.Helper.PasswordHashSaltHelper.CreateSalt(4);
            }
            var staffSalt = userDbSalt + password;

            var hashPassword = Core.Helper.PasswordHashSaltHelper.CreateHash256(staffSalt);
            if (hashPassword == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "ok", hashPassword);
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public ServiceResult ForgotPassword(string email)
        {
            var emailControl = EMailAddressControl(email);
            if (emailControl.Result == false)
                return new ServiceResult(false, emailControl.Message);

            var dbForgotPasswordControl = DbForgotPasswordControl(email);
            if (dbForgotPasswordControl.Result == false)
                return new ServiceResult(false, dbForgotPasswordControl.Message);

            var createLoginToken = CreateToken(email);
            if (createLoginToken.Result == false)
                return new ServiceResult(false, createLoginToken.Message);



            return new ServiceResult(true, "ok", createLoginToken.Obj);
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public DataServiceResult<Staff> DbForgotPasswordControl(string email)
        {

            var dbEmail = _emailService.GetByEmail(email);
            if (dbEmail.Result != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            var dbStaffEmail = _staffEmailService.GetByEmailId(dbEmail.Data.Id);
            if (dbStaffEmail.Result != true)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailNotFound");

            if (dbStaffEmail.Data.IsMain == false)
                return new DataServiceResult<Staff>(false, "Error.Login_EmailIsNotIsMain");

            var dbStaff = _staffService.GetById(dbStaffEmail.Data.StaffId);
            if (dbStaffEmail.Result != true)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotFound");

            if (dbStaff.Data.IsActive != true)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotActive");

            if (dbStaff.Data.IsLeaving)
                return new DataServiceResult<Staff>(false, "Error.Login_StaffNotActive");

            return new SuccessDataServiceResult<Staff>(dbStaff.Data, true, "ok");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public ServiceResult ResetPassword(string id, string password, string confirmPassword)
        {
            var passwordControl = PasswordControl(password);
            if (passwordControl.Result == false)
                return new ErrorServiceResult(false, passwordControl.Message);

            var confirmPasswordControl = PasswordControl(confirmPassword);
            if (confirmPasswordControl.Result == false)
                return new ErrorServiceResult(false, confirmPasswordControl.Message);

            if (password != confirmPassword)
                return new ErrorServiceResult(false, "Error.Login_UserPasswordNotMatch");

            var token = Core.Helper.StringHelper.Decrypt(id, Core.Helper.SettingsHelper.GetValue("Token", "ConfigSecret").ToString());
            string[] tokenSplit = token.Split("|");
            var email = tokenSplit[0].ToString();
            var applicationName = tokenSplit[1].ToString();
            var date = tokenSplit[2];

            DateTimeFormatInfo trDtfi = new CultureInfo("tr-TR", false).DateTimeFormat;

            if (DateTime.UtcNow.AddMinutes(-4) > Convert.ToDateTime(date, trDtfi) && DateTime.UtcNow.AddMinutes(4) < Convert.ToDateTime(date, trDtfi))
                return new ErrorServiceResult(false, "Error.Login_TokenDateExpired");

            if (applicationName != Core.Helper.SettingsHelper.GetValue("Token", "ApplicationName").ToString())
                return new ErrorServiceResult(false, "Error.Login_TokenApplicationNameError");

            var emailControl = DbEmailControl(email);
            if (emailControl.Result == false)
                return new ServiceResult(false, emailControl.Message);

            if (string.IsNullOrWhiteSpace(emailControl.Data.PasswordSalt )||(emailControl.Data.PasswordSalt=="1"))
            {
                emailControl.Data.PasswordSalt = Core.Helper.PasswordHashSaltHelper.CreateSalt(4);
            }

            string passwordObj = PasswordHashing(password, emailControl.Data.PasswordSalt).Obj.ToString();

            emailControl.Data.Password = passwordObj;

            var staffSave = _staffService.Save(emailControl.Data);
            if (staffSave.Result == false)
                return new ErrorServiceResult(false, "Error.Login_PasswordSaveUnSuccesful");

            return new ServiceResult(true, "ok");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public DataServiceResult<List<OperationClaim>> GetRoles(Staff staff)
        {
            var claims = _authorizationService.GetAllAuthorizationByStaffId(staff.Id);
            List<OperationClaim> operationClaims = new List<OperationClaim>();
            foreach (var authorization in claims.Data)
            {
                OperationClaim operationClaim = new OperationClaim();
                operationClaim.Id = authorization.Id;
                operationClaim.Name = authorization.AuthorizationName;
                operationClaims.Add(operationClaim);
            }
            return new SuccessDataServiceResult<List<OperationClaim>> (operationClaims, true,"");
        }
    }
}
