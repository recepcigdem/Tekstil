using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Core;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Staff;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UI.Models;
using UI.Models.Staff;
using UI.Helpers;
using UI.Models.Request;
using UI.Models.Response;

namespace UI.Controllers.Staff
{
    public class StaffController : BaseController
    {
        private IStaffService _staffService;
        private IStaffEmailService _staffEmailService;
        private IStaffPhoneService _staffPhoneService;
        private IStaffAuthorizationService _staffAuthorizationService;
        private IEmailService _emailService;
        private IPhoneService _phoneService;


        public StaffController(IStringLocalizerFactory factory, IStringLocalizer<StaffController> localizer, ILogger<StaffController> logger, IWebHostEnvironment env, IStaffService staffService, IStaffEmailService staffEmailService, IStaffPhoneService staffPhoneService, IStaffAuthorizationService staffAuthorizationService, IEmailService emailService, IPhoneService phoneService) : base(factory, env)
        {
            _staffService = staffService;
            _staffEmailService = staffEmailService;
            _staffPhoneService = staffPhoneService;
            _staffAuthorizationService = staffAuthorizationService;
            _emailService = emailService;
            _phoneService = phoneService;

            _localizer = localizer;
            _logger = logger;
            var type = typeof(Resources.SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizerShared = factory.Create("SharedResource", assemblyName.Name);
        }
        public IActionResult Index()
        {
            BaseModel model = new BaseModel(Request);
            return View(model);
        }

        [HttpPost]
        public JsonResult StaffList()
        {
            StaffList list = new StaffList(Request, _staffService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Staff serviceDetail = new Entities.Concrete.Staff();
            if (id > 0)
                serviceDetail = _staffService.GetById(id).Data;

            var model = new Models.Staff.Staff(Request, serviceDetail, _localizerShared, _env.WebRootPath, _staffEmailService, _staffPhoneService, _staffAuthorizationService, _emailService, _phoneService, _staffService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var staff = _staffService.GetById(id).Data;
                if (staff != null)
                {
                    Models.Staff.Staff model = new Models.Staff.Staff(Request, staff, _localizerShared, _env.WebRootPath, _staffEmailService, _staffPhoneService, _staffAuthorizationService, _emailService, _phoneService, _staffService);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Staff entity = new Entities.Concrete.Staff();
            entity.Id = Id;
            if (entity.Id > 0)
            {
                var res = _staffService.DeleteAll(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(Models.Staff.Staff staff, string userPassword, string userConfirmPassword)
        {
            if (staff != null)
            {
                var staffMails = staff.SubEmail.data.Where(x => x.IsMain == true).Select(x => x.EmailAddress);
                if (staffMails.FirstOrDefault() == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffIsMainIsNotNull")));

                if ((userPassword == null || userConfirmPassword == null))
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffPasswordNull")));

                if (userPassword != userConfirmPassword)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffPasswordNotMatch")));

                var session = SessionHelper.GetStaff(Request);

                var dbStaff = _staffService.GetById(session.StaffId);
                if (dbStaff.Result == false)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("StaffNotFound")));

                var staffSalt = dbStaff.Data.PasswordSalt + userPassword;

                var hashPassword = Core.Helper.PasswordHashSaltHelper.CreateHash256(staffSalt);
                if (hashPassword == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("SystemError")));


                if (dbStaff.Data.Password != hashPassword)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffPasswordNotMatch")));

                if (staff.EntityId < 1 && string.IsNullOrEmpty(staff.Password))
                {
                    staff.Password = "1";
                    staff.PasswordSalt = "1";
                }

                Entities.Concrete.Staff entity = staff.GetBusinessModel(_staffService);
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                entity.Password = staff.Password;
                entity.PasswordSalt = staff.PasswordSalt;

                List<StaffPhoneDto> staffPhoneDtos = staff.ListStaffPhone;
                List<StaffEmailDto> staffEmailDtos = staff.ListStaffEmail;
                List<StaffAuthorization> staffAuthorizations = staff.ListStaffAuthorizations;

                var result = _staffService.SaveAll(entity, staffEmailDtos, staffPhoneDtos, staffAuthorizations, userPassword);
                if (result.Result == false)
                    result.Message = _localizer.GetString(result.Message);
                else
                {
                    result.Message = _localizerShared.GetString(result.Message);
                    result.Data = entity;
                }

                if (staff.EntityId < 1)
                {
                    foreach (var staffEmailDto in staffMails)
                    {
                        CreatePasswordSendMail(staffEmailDto);
                    }
                }

                result.Data = entity;
                return Json(result);
            }

            return null;
        }

        public JsonResult CreatePasswordSendMail(string email)
        {

            var token = TokenHelper.CreateLoginToken(email);

            RequestMail requestMail = new RequestMail();
            requestMail.BaseUrl = Core.Helper.SettingsHelper.GetValue("Info", "ProjectUrl");
            requestMail.LogoUrl = requestMail.BaseUrl + Core.Helper.SettingsHelper.GetValue("Info", "LogoURL");
            requestMail.CompanyName = Core.Helper.SettingsHelper.GetValue("Info", "CompanyName");
            requestMail.MailHeader = _localizerShared["CreatePassword_MailHeader"];
            requestMail.MailInfo = _localizerShared["CreatePassword_MailInfo"];
            requestMail.ButtonText = _localizerShared["CreatePassword_ButtonText"];
            requestMail.ButtonUrl = requestMail.BaseUrl + requestMail.Culture + "/Login/ResetPassword?Id=" + token;
            requestMail.AddressLine1 = Core.Helper.SettingsHelper.GetValue("Info", "AddressLine1");
            requestMail.AddressLine2 = Core.Helper.SettingsHelper.GetValue("Info", "AddressLine2");
            requestMail.MailAddress = email;
            requestMail.TemplateName = Core.Helper.SettingsHelper.GetValue("Mailingtemplate", "CreatePassword");

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

            var emailResult = Core.Helper.SendMail.SendMailProcess(subject, messageBody, requestMail.MailAddress);
            if (emailResult.Result != true)
                ViewBag.Error = _localizer.GetString("MailError");

            return null;
        }
    }
}
