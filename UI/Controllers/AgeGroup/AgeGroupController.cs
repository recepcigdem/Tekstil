using Business.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Core.Utilities.Results;
using UI.Models;
using UI.Models.AgeGroup;

namespace UI.Controllers.AgeGroup
{
    public class AgeGroupController : BaseController
    {
        private AgeGroupManager _ageGroupManager;

        public AgeGroupController(IStringLocalizerFactory factory, IStringLocalizer<AgeGroupController> localizer, ILogger<AgeGroupController> logger, IWebHostEnvironment env, AgeGroupManager ageGroupManager) : base(factory, env)
        {
            _ageGroupManager = ageGroupManager;
            _localizer = localizer;
            _logger = logger;
            var type = typeof(Resouces.SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizerShared = factory.Create("SharedResource", assemblyName.Name);
        }
        public IActionResult Index()
        {
            BaseModel model = new BaseModel(Request);
            return View(model);
        }
        public JsonResult AgeGroupList()
        {
            AgeGroupList list = new AgeGroupList(_ageGroupManager);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            var serviceDetail = _ageGroupManager.GetById(id).Data;
            if (serviceDetail == null || serviceDetail.Id < 1)
                serviceDetail = new Entities.Concrete.AgeGroup();

            var model = new Models.AgeGroup.AgeGroup(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _ageGroupManager.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.AgeGroup.AgeGroup model = new Models.AgeGroup.AgeGroup(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(Models.AgeGroup.AgeGroup ageGroup)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult( _localizer.GetString("Error_Programmatic_UserNotFound")));
            }
            #endregion
            
            Entities.Concrete.AgeGroup entity = ageGroup.GetBusinessModel();
            if (entity.Id > 0)
            {
                var res = _ageGroupManager.Delete(entity);
                return Json(res);
            }
            return null;
        }

        public JsonResult Save(Models.AgeGroup.AgeGroup ageGroup)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_Programmatic_UserNotFound")));
            }
            #endregion

            if (ageGroup != null)
            {
                Entities.Concrete.AgeGroup entity = ageGroup.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorResult(false, _localizerShared.GetString("Error_SystemError")));

                var result = _ageGroupManager.Add(entity);

                if (ageGroup.EntityId > 0)
                {
                    result = _ageGroupManager.Update(entity);
                }
                else
                {
                    result = _ageGroupManager.Add(entity);
                }

                if (result.Success == false)
                {
                    result.Message = _localizer.GetString(result.Message);
                    return Json(result);
                }


                result.Data = entity;
                return Json(result);
            }

            return null;
        }

    }
}
