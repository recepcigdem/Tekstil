using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Business.Abstract;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using UI.Helpers;
using UI.Models.Common;
using UI.Models.Request;
using UI.Models.Response;

namespace UI.Controllers.Login
{
    public class LoginController : BaseController
    {
        private ILoginService _loginService;

        public LoginController(IStringLocalizerFactory factory, IStringLocalizer<LoginController> localizer, ILogger<LoginController> logger, IWebHostEnvironment env, ILoginService loginService) : base(factory, env)
        {
            _loginService = loginService;
            _localizer = localizer;
            _logger = logger;
        }
        public IActionResult Index()
        {
            Models.Login.Login model = new Models.Login.Login(Request);
            if (Request.Cookies["user"] != null)
            {
                model.UserName = Request.Cookies["user"].ToString();
                model.RememberMe = true;
            }
            return View("Index", model);
        }

        public IActionResult Login(LoginRequest loginRequest)
        {

            var login = _loginService.Login(loginRequest.UserName, loginRequest.Password);
            if (login.Result == false)
            {
                ViewBag.Error = _localizer.GetString(login.Message);
                return View("Index");
            }

            #region RememberMe

            if (loginRequest.RememberMe == true)
                Response.Cookies.Append("staff", loginRequest.UserName);
            else
                Response.Cookies.Delete("staff");

            #endregion

            #region Session

            StaffSession staffSession = new StaffSession();
            staffSession.StaffId = login.Data.Id;
            staffSession.CustomerId = login.Data.CustomerId;
            staffSession.FirstName = login.Data.FirstName;
            staffSession.LastName = login.Data.LastName;
            staffSession.Token = login.Obj.ToString();
            staffSession.IsCompanyAdmin = login.Data.IsCompanyAdmin;
            staffSession.IsSuperAdmin = login.Data.IsSuperAdmin;
            staffSession.OperationClaims = login.Data.OperationClaims;

            SessionHelper.SetStaff(Request, staffSession);

            #endregion

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult PasswordResetSendMail(LoginRequest loginRequest)
        {

            var forgotPassword = _loginService.ForgotPassword(loginRequest.UserName);
            if (forgotPassword.Result == false)
            {
                ViewBag.Error = _localizer.GetString(forgotPassword.Message);
                return View("ForgotPassword");
            }

            RequestMail requestMail = new RequestMail();
            requestMail.BaseUrl = Core.Helper.SettingsHelper.GetValue("Info", "ProjectUrl");
            requestMail.LogoUrl = requestMail.BaseUrl + Core.Helper.SettingsHelper.GetValue("Info", "LogoURL");
            requestMail.CompanyName = Core.Helper.SettingsHelper.GetValue("Info", "CompanyName");
            requestMail.MailHeader = _localizerShared["ForgotPassword_MailHeader"];
            requestMail.MailInfo = _localizerShared["ForgotPassword_MailInfo"];
            requestMail.ButtonText = _localizerShared["ForgotPassword_ButtonText"];
            requestMail.ButtonUrl = requestMail.BaseUrl + loginRequest.Culture + "/Login/ResetPassword?Id=" + forgotPassword.Obj;
            requestMail.AddressLine1 = Core.Helper.SettingsHelper.GetValue("Info", "AddressLine1");
            requestMail.AddressLine2 = Core.Helper.SettingsHelper.GetValue("Info", "AddressLine2");
            requestMail.MailAddress = loginRequest.UserName;
            requestMail.TemplateName = Core.Helper.SettingsHelper.GetValue("Mailingtemplate", "ForgotPassword");

            var pathToFile = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + requestMail.TemplateName;


            var builder = new BodyBuilder();
            using (StreamReader sourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = sourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                requestMail.LogoUrl,
                requestMail.CompanyName,
                requestMail.MailHeader,
                requestMail.MailInfo,
                requestMail.ButtonUrl,
                requestMail.ButtonText,
                requestMail.AddressLine1,
                requestMail.AddressLine2
            );

            string subject = requestMail.MailHeader;
            string logoUrl = requestMail.LogoUrl;

            var emailReuslt = Core.Helper.SendMail.SendMailProcess(subject, messageBody, requestMail.MailAddress);
            if (emailReuslt.Result != true)
                ViewBag.Error = _localizer.GetString("MailError");
            

            return View("ForgotPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CompleteResetPassword(string Id, string password, string confirmPassword)
        {

            var resetPassword = _loginService.ResetPassword(Id,password,confirmPassword);
            if (resetPassword.Result == false)
            {
               resetPassword.Message = _localizer.GetString(resetPassword.Message);
                return Json(new ServiceResult(true, resetPassword.Message));
            }

            return Json(new ServiceResult(true, _localizer.GetString("Error_UserPasswordChanges")));

        }

    }
}

