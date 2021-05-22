using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using UI.Helpers;
using UI.Models;
using UI.Models.CsNoDeliveryDate;

namespace UI.Controllers.CsNoDeliveryDate
{
    public class CsNoDeliveryDateController : BaseController
    {
        private ICsNoDeliveryDateService _csNoDeliveryDateService;
        private ICsNoDeliveryDateHistoryService _csNoDeliveryDateHistoryService;
        private ISeasonService _seasonService;

        public CsNoDeliveryDateController(IStringLocalizerFactory factory, IStringLocalizer<CsNoDeliveryDateController> localizer, ILogger<CsNoDeliveryDateController> logger, IWebHostEnvironment env, ICsNoDeliveryDateService csNoDeliveryDateService, ICsNoDeliveryDateHistoryService csNoDeliveryDateHistoryService, ISeasonService seasonService) : base(factory, env)
        {
            _csNoDeliveryDateService = csNoDeliveryDateService;
            _localizer = localizer;
            _logger = logger;
            var type = typeof(Resources.SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizerShared = factory.Create("SharedResource", assemblyName.Name);
            _csNoDeliveryDateHistoryService = csNoDeliveryDateHistoryService;
            _seasonService = seasonService;
        }

        public IActionResult Index()
        {
            BaseModel model = new BaseModel(Request);
            return View(model);
        }

        [HttpPost]
        public JsonResult CsNoDeliveryDateList()
        {
            CsNoDeliveryDateList list = new CsNoDeliveryDateList(Request, _csNoDeliveryDateService,_seasonService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.CsNoDeliveryDate serviceDetail = new Entities.Concrete.CsNoDeliveryDate();
            if (id > 0)
                serviceDetail = _csNoDeliveryDateService.GetById(id).Data;

            var model = new Models.CsNoDeliveryDate.CsNoDeliveryDate(Request, serviceDetail, _localizerShared, _csNoDeliveryDateHistoryService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _csNoDeliveryDateService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.CsNoDeliveryDate.CsNoDeliveryDate model = new Models.CsNoDeliveryDate.CsNoDeliveryDate(Request, serviceDetail, _localizerShared, _csNoDeliveryDateHistoryService);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  CsNoDeliveryDate Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.CsNoDeliveryDate entity = new Entities.Concrete.CsNoDeliveryDate();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _csNoDeliveryDateService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(Models.CsNoDeliveryDate.CsNoDeliveryDate csNoDeliveryDate)
        {
            #region  CsNoDeliveryDate Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (csNoDeliveryDate != null)
            {
                Entities.Concrete.CsNoDeliveryDate entity = csNoDeliveryDate.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var staffId = SessionHelper.GetStaff(Request).StaffId;

                var result = _csNoDeliveryDateService.Save(staffId, entity);
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
    }
}
