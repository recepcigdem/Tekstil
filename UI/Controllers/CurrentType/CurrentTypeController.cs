using Business.Abstract;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using UI.Helpers;
using UI.Models;
using UI.Models.CurrentType;

namespace UI.Controllers.CurrentType
{
    public class CurrentTypeController : BaseController
    {
        private ICurrentTypeService _currentTypeService;

        public CurrentTypeController(IStringLocalizerFactory factory, IStringLocalizer<CurrentTypeController> localizer, ILogger<CurrentTypeController> logger, IWebHostEnvironment env, ICurrentTypeService currentTypeService) : base(factory, env)
        {
            _currentTypeService = currentTypeService;
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
        public JsonResult CurrentTypeList()
        {
            CurrentTypeList list = new CurrentTypeList(Request, _currentTypeService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.CurrentType serviceDetail = new Entities.Concrete.CurrentType();
            if (id > 0)
                serviceDetail = _currentTypeService.GetById(id).Data;

            var model = new Models.CurrentType.CurrentType(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _currentTypeService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.CurrentType.CurrentType model = new Models.CurrentType.CurrentType(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  CurrentType Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.CurrentType entity = new Entities.Concrete.CurrentType();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _currentTypeService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.CurrentType.CurrentType currentType)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (currentType != null)
            {
                Entities.Concrete.CurrentType entity = currentType.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var result = _currentTypeService.Save(entity);
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

            var currentTypeList = _currentTypeService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.CurrentType entity in currentTypeList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.Description);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
