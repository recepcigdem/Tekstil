using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UI.Models;
using UI.Models.Staff;

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

            var model = new Models.Staff.Staff(Request, staff, _localizerShared,_env.WebRootPath, _staffEmailService,_staffPhoneService,_staffAuthorizationService,_emailService,_phoneService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var staff = _staffService.GetById(id).Data;
                if (staff != null)
                {
                    Models.Staff.Staff model = new Models.Staff.Staff(Request, staff, _localizerShared, _env.WebRootPath, _staffEmailService, _staffPhoneService, _staffAuthorizationService, _emailService, _phoneService);
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
                var res = _staffService.DeleteAll(entity.Id);
                res.Message = _localizer.GetString(res.Message);
                return Json(res);
            }
            return null;
        }
    }
}
