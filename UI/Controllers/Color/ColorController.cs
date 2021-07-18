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
using UI.Models.Color;

namespace UI.Controllers.Color
{
    public class ColorController : BaseController
    {
        private IColorService _colorService;

        public ColorController(IStringLocalizerFactory factory, IStringLocalizer<ColorController> localizer, ILogger<ColorController> logger, IWebHostEnvironment env, IColorService colorService) : base(factory, env)
        {
            _colorService = colorService;
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
        public JsonResult ColorList()
        {
            ColorList list = new ColorList(Request, _colorService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Color serviceDetail = new Entities.Concrete.Color();
            if (id > 0)
                serviceDetail = _colorService.GetById(id).Data;

            var model = new Models.Color.Color(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _colorService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Color.Color model = new Models.Color.Color(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Color Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Color entity = new Entities.Concrete.Color();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _colorService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Color.Color color)
        {
            #region  Color Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (color != null)
            {
                Entities.Concrete.Color entity = color.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var result = _colorService.Save(entity);
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

            var colorList = _colorService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Color entity in colorList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.DescriptionTr);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
