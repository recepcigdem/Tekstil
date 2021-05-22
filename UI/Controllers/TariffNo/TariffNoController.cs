using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos.TariffNo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using UI.Helpers;
using UI.Models;
using UI.Models.TariffNo;

namespace UI.Controllers.TariffNo
{
    public class TariffNoController : BaseController
    {
        private ITariffNoService _tariffNoService;
        private ITariffNoDetailService _tariffNoDetailService;
        private ISeasonService _seasonService;
        private ISeasonCurrencyService _seasonCurrencyService;
        private IDefinitionService _definitionService;

        public TariffNoController(IStringLocalizerFactory factory, IStringLocalizer<TariffNoController> localizer, ILogger<TariffNoController> logger, IWebHostEnvironment env, ITariffNoService tariffNoService, ISeasonService seasonService, ISeasonCurrencyService seasonCurrencyService, IDefinitionService definitionService, ITariffNoDetailService tariffNoDetailService) : base(factory, env)
        {
            _tariffNoService = tariffNoService;
            _tariffNoDetailService = tariffNoDetailService;
            _seasonService = seasonService;
            _seasonCurrencyService = seasonCurrencyService;
            _definitionService = definitionService;
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
        public JsonResult TariffNoList()
        {
            TariffNoList list = new TariffNoList(Request, _tariffNoService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.TariffNo serviceDetail = new Entities.Concrete.TariffNo();
            if (id > 0)
                serviceDetail = _tariffNoService.GetById(id).Data;

            var model = new Models.TariffNo.TariffNo(Request, serviceDetail, _localizerShared, _seasonService, _seasonCurrencyService, _definitionService, _tariffNoDetailService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _tariffNoService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.TariffNo.TariffNo model = new Models.TariffNo.TariffNo(Request, serviceDetail, _localizerShared, _seasonService, _seasonCurrencyService, _definitionService, _tariffNoDetailService);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  TariffNo Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.TariffNo entity = new Entities.Concrete.TariffNo();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _tariffNoService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.TariffNo.TariffNo tariffNo)
        {
            #region  TariffNo Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (tariffNo != null)
            {
                Entities.Concrete.TariffNo entity = tariffNo.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                List<TariffNoDetail> tariffNoDetails = tariffNo.TariffNoDetails;

                var result = _tariffNoService.Save(entity, tariffNoDetails);
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
            var customerId = SessionHelper.GetStaff(Request).CustomerId;

            var tariffNoList = _tariffNoService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.TariffNo entity in tariffNoList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.Description);
                data.Add(model);
            }
            return Json(data);
        }
    }
}