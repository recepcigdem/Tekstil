using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete.Dtos.Current;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using UI.Controllers.Customer;
using UI.Helpers;
using UI.Models;
using UI.Models.Current;
using UI.Models.Customer;

namespace UI.Controllers.Current
{
    public class CurrentController : BaseController
    {
        private ICurrentService _currentService;
        private ICurrentEmailService _currentEmailService;
        private ICurrentPhoneService _currentPhoneService;
        private IEmailService _emailService;
        private IPhoneService _phoneService;

        public CurrentController(IStringLocalizerFactory factory, IStringLocalizer<CurrentController> localizer, ILogger<CurrentController> logger, IWebHostEnvironment env, ICurrentService currentService, ICurrentEmailService currentEmailService, ICurrentPhoneService currentPhoneService, IEmailService emailService, IPhoneService phoneService) : base(factory, env)
        {
            _currentService = currentService;
            _currentEmailService = currentEmailService;
            _currentPhoneService = currentPhoneService;
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
        public JsonResult CurrentList()
        {
            CurrentList list = new CurrentList(Request, _currentService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Customer serviceDetail = new Entities.Concrete.Customer();
            if (id > 0)
                serviceDetail = _currentService.GetById(id).Data;

            var model = new Models.Current.Current(Request, serviceDetail, _localizerShared,_currentEmailService,_currentPhoneService,_emailService, _phoneService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _currentService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    var model = new Models.Current.Current(Request, serviceDetail, _localizerShared, _currentEmailService, _currentPhoneService, _emailService, _phoneService);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Customer Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Customer entity = new Entities.Concrete.Customer();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _currentService.DeleteAll(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Current.Current customer)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (customer != null)
            {
                Entities.Concrete.Customer entity = customer.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                List<CurrentPhoneDto> currentPhoneDtos = customer.ListCurrentPhone;
                List<CurrentEmailDto> currentEmailDtos = customer.ListCurrentEmail;

                var result = _currentService.SaveAll(entity, currentEmailDtos, currentPhoneDtos);
                if (result.Result == false)
                    result.Message = _localizer.GetString(result.Message);
                else
                {
                    result.Message = _localizerShared.GetString(result.Message);
                    result.Data = entity;
                }

                return Json(result);
            }

            return null;
        }

        public JsonResult ComboList()
        {
            var customerId = Helpers.SessionHelper.GetStaff(Request).CustomerId;

            var customerList = _currentService.GetAll(true, customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Customer entity in customerList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.CustomerName);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
