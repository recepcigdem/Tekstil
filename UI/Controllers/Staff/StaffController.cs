﻿using Microsoft.AspNetCore.Mvc;
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
            return View(model);
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

        public JsonResult Delete(int Id)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Staff entity = new Entities.Concrete.Staff();
            entity.Id = Id;
            if (entity.Id > 0)
            {
                var res = _staffService.DeleteAll(entity);
                res.Message = _localizer.GetString(res.Message);
                return Json(res);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(Models.Staff.Staff staff, string userPassword, string userConfirmPassword)
        {
            if (staff != null)
            {
                var staffMail = staff.SubEmail.data.Where(x => x.IsMain == true);
                if (staffMail.FirstOrDefault() == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffIsMainIsNotNull")));

                if ((userPassword == null || userConfirmPassword == null))
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffPasswordNull")));

                if (userPassword != userConfirmPassword)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffPasswordNotMatch")));

                var session = SessionHelper.GetStaff(Request);

                var encodePassword = Core.Helper.StringHelper.Base64Encode(userPassword);
                if (encodePassword == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("SystemError")));

                var hashPassword = Core.Helper.PasswordHashSaltHelper.CreateHash256(encodePassword);
                if (hashPassword == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("SystemError")));

                var dbStaff = _staffService.GetById(session.StaffId);
                if (dbStaff.Result == false)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("StaffNotFound")));

                if (dbStaff.Data.Password != hashPassword)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_StaffPasswordNotMatch")));

                if (string.IsNullOrEmpty(staff.Password))
                {
                    staff.Password = "1";
                    staff.PasswordSalt = "1";
                }

                Entities.Concrete.Staff entity = staff.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_SystemError")));

                List<StaffPhoneDto> staffPhoneDtos = staff.ListStaffPhone;
                List<StaffEmailDto> staffEmailDtos = staff.ListStaffEmail;
                List<StaffAuthorization> staffAuthorizations = staff.ListStaffAuthorizations;

                var result = _staffService.SaveAll(entity, staffEmailDtos, staffPhoneDtos, staffAuthorizations, userPassword);
                if (result.Result == false)
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
