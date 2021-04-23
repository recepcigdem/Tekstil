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
            staffSession.FirstName = login.Data.FirstName;
            staffSession.LastName = login.Data.LastName;
            staffSession.Token = login.Obj.ToString();
            staffSession.IsCompanyAdmin = login.Data.IsCompanyAdmin;
            staffSession.IsSuperAdmin = login.Data.IsSuperAdmin;

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
            requestMail.ProjectName = Core.Helper.SettingsHelper.GetValue("Token", "ApplicationName");
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

            SendMail(subject, messageBody, requestMail.MailAddress);

            return View("ForgotPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CompleteResetPassword(string Id, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.Login_WrongMail")));

            if (password != confirmPassword)
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.Login_UserPasswordNotMatch")));


            Models.Request.RequestPasswordChange request = new RequestPasswordChange() { Password = Core.Helper.StringHelper.Base64Encode(password), Token = Id };
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = Core.Helper.SettingsHelper.GetValue("Info", "BaseURL");
                string url = "Login/passwordchange";
                client.Headers.Add("AnonToken", Id);
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = JsonConvert.SerializeObject(request);
                var response = client.UploadString(url, data);

                // var result = JsonConvert.DeserializeObject(response);
                Response result = JsonConvert.DeserializeObject<Response>(response);
                if (!result.IsSuccess)
                {
                    result.ErrorMessage = _localizerShared[result.ErrorMessage];

                    return Json(new ErrorResult(false, result.ErrorMessage));
                }

            }
            return Json(new SuccessResult(true, _localizer.GetString("Error_UserPasswordChanges")));

        }


        public string SendMail(string konu, string mesaj, string eposta)
        {
            var smtpAddress = Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpUser");
            var smtpPassword = Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpPassword");
            var smtpSendPort = Convert.ToInt32(Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpSendPort"));
            var smtpHost = Core.Helper.SettingsHelper.GetValue("Smtp", "SmtpServer");


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpAddress);
                mail.To.Add(eposta);
                mail.Subject = konu;
                mail.Body = mesaj;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpSendPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpAddress, smtpPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

          
            return "basarili";
        }

    }
}

