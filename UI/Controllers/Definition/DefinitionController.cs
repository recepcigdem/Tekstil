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
using UI.Models.Definition;

namespace UI.Controllers.Definition
{
    public class DefinitionController : BaseController
    {
        private IDefinitionService _definitionService;
        private IDefinitionTitleService _definitionTitleService;

        public DefinitionController(IStringLocalizerFactory factory, IStringLocalizer<DefinitionController> localizer, ILogger<DefinitionController> logger, IWebHostEnvironment env, IDefinitionService definitionService, IDefinitionTitleService definitionTitleService) : base(factory, env)
        {
            _definitionService = definitionService;
            _definitionTitleService = definitionTitleService;
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
        public JsonResult DefinitionList()
        {
            DefinitionList list = new DefinitionList(Request, _definitionService,_definitionTitleService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Definition serviceDetail = new Entities.Concrete.Definition();
            if (id>0)
                serviceDetail = _definitionService.GetById(id).Data;

            var model = new Models.Definition.Definition(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _definitionService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Definition.Definition model = new Models.Definition.Definition(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Definition Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false,_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Definition entity = new Entities.Concrete.Definition();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _definitionService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Definition.Definition definition)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false,_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (definition != null)
            {
                Entities.Concrete.Definition entity = definition.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = Helpers.SessionHelper.GetStaff(Request).CustomerId;

                var result = _definitionService.Save(entity);
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

        public JsonResult ComboList(int value)
        {
            var customerId = Helpers.SessionHelper.GetStaff(Request).CustomerId;

            var definitionTitle = _definitionTitleService.GetByValue(customerId, value);

            var definitionList = _definitionService.GetAllByCustomerIdAndDefinitionTitleId(customerId, definitionTitle.Data.Id).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Definition entity in definitionList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.DescriptionTr);
                data.Add(model);
            }
            return Json(data);
        }
    }
}
