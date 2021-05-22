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
using UI.Models.Hierarchy;

namespace UI.Controllers.Hierarchy
{
    public class HierarchyController : BaseController
    {
        private IHierarchyService _hierarchyService;

        public HierarchyController(IStringLocalizerFactory factory, IStringLocalizer<HierarchyController> localizer, ILogger<HierarchyController> logger, IWebHostEnvironment env, IHierarchyService hierarchyService) : base(factory, env)
        {
            _hierarchyService = hierarchyService;
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
        public JsonResult HierarchyList()
        {
            HierarchyList list = new HierarchyList(Request, _hierarchyService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Hierarchy serviceDetail = new Entities.Concrete.Hierarchy();
            if (id > 0)
                serviceDetail = _hierarchyService.GetById(id).Data;

            var model = new Models.Hierarchy.Hierarchy(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _hierarchyService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Hierarchy.Hierarchy model = new Models.Hierarchy.Hierarchy(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Hierarchy Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Hierarchy entity = new Entities.Concrete.Hierarchy();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _hierarchyService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Hierarchy.Hierarchy hierarchy)
        {
            #region  Hierarchy Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (hierarchy != null)
            {
                Entities.Concrete.Hierarchy entity = hierarchy.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var result = _hierarchyService.Save(entity);
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

            var hierarchyList = _hierarchyService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Hierarchy entity in hierarchyList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.Code);
                data.Add(model);
            }
            return Json(data);
        }
    }
}
