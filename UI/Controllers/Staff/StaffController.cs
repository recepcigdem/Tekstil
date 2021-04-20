using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Core;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        private IAuthorizationService _authorizationService;


        public StaffController(IStringLocalizerFactory factory, IStringLocalizer<StaffController> localizer, ILogger<StaffController> logger, IWebHostEnvironment env, IStaffService staffService, IStaffEmailService staffEmailService, IStaffPhoneService staffPhoneService, IStaffAuthorizationService staffAuthorizationService, IEmailService emailService, IPhoneService phoneService, IAuthorizationService authorizationService) : base(factory, env)
        {
            _staffService = staffService;
            _staffEmailService = staffEmailService;
            _staffPhoneService = staffPhoneService;
            _staffAuthorizationService = staffAuthorizationService;
            _emailService = emailService;
            _phoneService = phoneService;
            _authorizationService = authorizationService;
            _localizer = localizer;
            _logger = logger;
            var type = typeof(Resources.SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizerShared = factory.Create("SharedResource", assemblyName.Name);
        }
        public IActionResult Index()
        {
            BaseModel model = new BaseModel(Request);
            return View();
        }

        [HttpPost]
        public JsonResult StaffList()
        {
            StaffList list = new StaffList(_staffService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            var staff = _staffService.GetById(id).Data;
            if (staff == null || staff.Id < 1)
                staff = new Entities.Concrete.Staff();

            var model = new Models.Staff.Staff(Request, staff, _localizerShared, _env.WebRootPath, _staffEmailService, _staffPhoneService, _staffAuthorizationService, _emailService, _phoneService, _authorizationService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var staff = _staffService.GetById(id).Data;
                if (staff != null)
                {
                    Models.Staff.Staff model = new Models.Staff.Staff(Request, staff, _localizerShared, _env.WebRootPath, _staffEmailService, _staffPhoneService, _staffAuthorizationService, _emailService, _phoneService, _authorizationService);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(Models.Staff.Staff staff)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Staff entity = staff.GetBusinessModel();
            if (entity.Id > 0)
            {
                var res = _staffService.Delete(entity);
                res.Message = _localizer.GetString(res.Message);
                return Json(res);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(Models.Staff.Staff staff, string password, string confirmPassword)
        {
            var token = string.Empty;
            var registerApiResult = string.Empty;
            var staffMail = staff.SubEmail.data.Where(x => x.IsMain == true && x.IsDeleted == false);
            if (staffMail.FirstOrDefault() == null)
                return Json(new ErrorResult(false, _localizer.GetString("Error_UserIsMainIsNotNull")));

            if ((password == null || confirmPassword == null) && staff.EntityId == 0)
                return Json(new ErrorResult(false, _localizer.GetString("Error_UserPasswordNull")));

            if (password != confirmPassword)
                return Json(new ErrorResult(false, _localizer.GetString("Error_UserPasswordNotMatch")));

            try
            {

                token = TokenHelper.CreateLoginToken(staffMail.FirstOrDefault().EmailAddress);

            }
            catch (Exception e)
            {
                return Json(new ErrorResult(false, _localizer.GetString("Error_SystemError")));
            }

            var passwordhash = staff.Password == null || staff.Password == "" ? "" : Core.Helper.StringHelper.Base64Encode(staff.Password);


            if (staff != null)
            {
                Entities.Concrete.Staff entity = staff.GetBusinessModel();

                if (entity == null)
                {
                    return Json(new ErrorResult(false, _localizer.GetString("Error_SystemError")));
                }

                Register registerData = new Register()
                {
                    StaffId = staff.EntityId,
                    CustomerId = staff.CustomerId,
                    DepartmentId = staff.DepartmentId,
                    IsActive = staff.IsActive,
                    Title = staff.Title == null || staff.Title == "" ? "" : staff.Title,
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staffMail.First().EmailAddress,
                    Password = passwordhash,
                    RegisterDate = DateTime.Now,
                    IsLeaving = staff.IsLeaving,
                    LeavingDate = staff.LeavingDate,
                    IsSendMail = staff.IsSendEmail,
                    Photo = staff.Photo,
                    IsSuperAdmin = staff.IsSuperAdmin,
                    IsCompanyAdmin = staff.IsCompanyAdmin
                };

                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = Core.Helper.SettingsHelper.GetValue("Info", "BaseURL");

                    string url = "Register/Register";
                    client.Headers.Add("AnonToken", token);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string data = JsonConvert.SerializeObject(registerData);
                    var response = client.UploadString(url, data);

                    Response result1 = JsonConvert.DeserializeObject<Response>(response);
                    if (!result1.IsSuccess)
                        return Json(new ErrorResult(false, _localizer.GetString(result1.ErrorMessage)));


                    JObject parseValue = JObject.Parse(result1.ResultData.ToString());
                    JToken parseValue2 = parseValue["resultData"];
                    JObject result3 = JObject.Parse(parseValue2.ToString());
                    JToken StaffId = result3["StaffId"];
                    int staffId = Convert.ToInt32(StaffId.ToString());

                    entity.Id = staffId;
                }

                List<StaffPhoneDto> staffPhoneDtos = staff.ListStaffPhone;
                List<StaffEmailDto> staffEmailDtos = staff.ListStaffEmail;
                List<StaffAuthorization> staffAuthorizations = staff.StaffAuthorizations;

                var result = _staffService.SaveAll(entity, staffEmailDtos, staffPhoneDtos, staffAuthorizations);
                if (result.Success == false)
                {
                    result.Message = _localizer.GetString(result.Message);
                    return Json(result);
                }

                result.Data = entity;
                return Json(result);
            }
            return null;
        }
    }
}
