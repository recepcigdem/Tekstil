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
using UI.Models.DefinitionTitle;

namespace UI.Controllers.DefinitionTitle
{
    public class DefinitionTitleController : BaseController
    {
        private IDefinitionTitleService _definitionTitleService;

        public DefinitionTitleController(IStringLocalizerFactory factory, IStringLocalizer<DefinitionTitleController> localizer, ILogger<DefinitionTitleController> logger, IWebHostEnvironment env, IDefinitionTitleService definitionTitleService) : base(factory, env)
        {
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
        public JsonResult DefinitionTitleList()
        {
            DefinitionTitleList list = new DefinitionTitleList(Request, _definitionTitleService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.DefinitionTitle serviceDetail = new Entities.Concrete.DefinitionTitle();
            if (id > 0)
                serviceDetail = _definitionTitleService.GetById(id).Data;

            var model = new Models.DefinitionTitle.DefinitionTitle(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _definitionTitleService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.DefinitionTitle.DefinitionTitle model = new Models.DefinitionTitle.DefinitionTitle(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  DefinitionTitle Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.DefinitionTitle entity = new Entities.Concrete.DefinitionTitle();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _definitionTitleService.Delete(entity);
                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.DefinitionTitle.DefinitionTitle definitionTitle)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (definitionTitle != null)
            {


                Entities.Concrete.DefinitionTitle entity = definitionTitle.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = Helpers.SessionHelper.GetStaff(Request).CustomerId;

                var result = _definitionTitleService.Save(entity);
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

        public JsonResult ComboList()
        {
            var customerId = Helpers.SessionHelper.GetStaff(Request).CustomerId;
            var definitionTitleList = _definitionTitleService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.DefinitionTitle entity in definitionTitleList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.Title);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
