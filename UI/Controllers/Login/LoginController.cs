﻿using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UI.Helpers;
using UI.Models.Common;
using UI.Models.Request;
using UI.Models.Response;

namespace UI.Controllers.Login
{
    public class LoginController : BaseController
    {
        public LoginController(IStringLocalizerFactory factory, IStringLocalizer<LoginController> localizer, ILogger<LoginController> logger, IWebHostEnvironment env) : base(factory, env)
        {
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

            if (loginRequest.UserName == null)
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.Login_WrongMail")));

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(loginRequest.UserName);
            if (!match.Success)
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.Login_WrongMail")));

            if (loginRequest.Password == null)
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.Login_WrongPassword")));

            if (loginRequest.Password.Length < 6)
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.Login_WrongPassword")));

            loginRequest.Password = Core.Helper.StringHelper.Base64Encode(loginRequest.Password);
            var token = TokenHelper.CreateLoginToken(loginRequest.UserName);
            if (string.IsNullOrEmpty(token))
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.TokenNotFound")));

            using (WebClient client = new WebClient())
            {
                client.BaseAddress = Core.Helper.SettingsHelper.GetValue("Info", "BaseURL");
                string url = "Login/login";
                client.Headers.Add("AnonToken", token);
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = JsonConvert.SerializeObject(loginRequest);
                var response = client.UploadString(url, data);

                // var result = JsonConvert.DeserializeObject(response);
                Response result = JsonConvert.DeserializeObject<Response>(response);
                if (!result.IsSuccess)
                {
                    ViewBag.Error = _localizer.GetString(result.ErrorMessage);

                    return View("Index");
                }
                if (loginRequest.RememberMe == true)
                {
                    Response.Cookies.Append("user", loginRequest.UserName);
                }
                else
                {
                    Response.Cookies.Delete("user");
                }
                StaffSession resultStaffSession = JsonConvert.DeserializeObject<StaffSession>(result.ResultData.ToString());
                Helpers.SessionHelper.SetStaff(Request, resultStaffSession);
            }


            return RedirectToAction("Index", "Home");


        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult PasswordResetSendMail(LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UserName))
            {
                ViewBag.Error = _localizerShared.GetString("Error.Login_WrongMail");
                return View("ForgotPassword");
            }
            try
            {
                var m = new MailAddress(loginRequest.UserName);
            }
            catch
            {
                ViewBag.Error = _localizerShared.GetString("Error.Login_WrongMail");
                return View("ForgotPassword");
            }

            var token = TokenHelper.CreateLoginToken(loginRequest.UserName);
            if (string.IsNullOrEmpty(token))
                return Json(new ErrorResult(false, _localizerShared.GetString("Error.TokenNotFound")));

            RequestMail requestMail = new RequestMail();
            requestMail.BaseUrl = Core.Helper.SettingsHelper.GetValue("Info", "ProjectUrl");
            requestMail.ProjectName = Core.Helper.SettingsHelper.GetValue("Token", "ApplicationName");
            requestMail.LogoUrl = requestMail.BaseUrl + Core.Helper.SettingsHelper.GetValue("Info", "LogoURL");
            requestMail.CompanyName = Core.Helper.SettingsHelper.GetValue("Info", "CompanyName");
            requestMail.MailHeader = _localizerShared["ForgotPassword_MailHeader"];
            requestMail.MailInfo = _localizerShared["ForgotPassword_MailInfo"];
            requestMail.ButtonText = _localizerShared["ForgotPassword_ButtonText"];
            requestMail.ButtonUrl = requestMail.BaseUrl + loginRequest.Culture + "/Login/ResetPassword?Id=" + token;
            requestMail.AddressLine1 = Core.Helper.SettingsHelper.GetValue("Info", "AddressLine1");
            requestMail.AddressLine2 = Core.Helper.SettingsHelper.GetValue("Info", "AddressLine2");
            requestMail.MailAddress = loginRequest.UserName;
            requestMail.TemplateName = Core.Helper.SettingsHelper.GetValue("Mailingtemplate", "ForgotPassword");


            using (WebClient client = new WebClient())
            {
                client.BaseAddress = Core.Helper.SettingsHelper.GetValue("Info", "BaseURL");
                string url = "Login/forgotpassword";
                client.Headers.Add("AnonToken", token);
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = JsonConvert.SerializeObject(requestMail);
                var response = client.UploadString(url, data);

                Response result = JsonConvert.DeserializeObject<Response>(response);
                if (!result.IsSuccess)
                {
                    ViewBag.Error = _localizer.GetString(result.ErrorMessage);

                    return View("Index");
                }
            }

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
            return Json(new SuccessResult( true, _localizer.GetString("Error_UserPasswordChanges")));

        }
    }
}
