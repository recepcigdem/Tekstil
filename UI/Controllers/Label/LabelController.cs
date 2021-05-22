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
using UI.Models.Label;

namespace UI.Controllers.Label
{
    public class LabelController : BaseController
    {
        private ILabelService _labelService;

        public LabelController(IStringLocalizerFactory factory, IStringLocalizer<LabelController> localizer, ILogger<LabelController> logger, IWebHostEnvironment env, ILabelService labelService) : base(factory, env)
        {
            _labelService = labelService;
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
        public JsonResult LabelList()
        {
            LabelList list = new LabelList(Request, _labelService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Label serviceDetail = new Entities.Concrete.Label();
            if (id > 0)
                serviceDetail = _labelService.GetById(id).Data;

            var model = new Models.Label.Label(Request, serviceDetail, _localizerShared,_env.WebRootPath);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _labelService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Label.Label model = new Models.Label.Label(Request, serviceDetail, _localizerShared, _env.WebRootPath);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Label Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Label entity = new Entities.Concrete.Label();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _labelService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(Models.Label.Label label)
        {
            #region  Label Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (label != null)
            {
                Entities.Concrete.Label entity = label.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var result = _labelService.Save(entity);
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

            var labelList = _labelService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Label entity in labelList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.Description);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
