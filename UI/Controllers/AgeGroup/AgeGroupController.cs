﻿using Business.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Business.Abstract;
using Core.Utilities.Results;
using UI.Models;
using UI.Models.AgeGroup;

namespace UI.Controllers.AgeGroup
{
    public class AgeGroupController : BaseController
    {
        private IAgeGroupService _ageGroupService;

        public AgeGroupController(IStringLocalizerFactory factory, IStringLocalizer<AgeGroupController> localizer, ILogger<AgeGroupController> logger, IWebHostEnvironment env, IAgeGroupService ageGroupService) : base(factory, env)
        {
            _ageGroupService = ageGroupService;
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
        public JsonResult AgeGroupList()
        {
            AgeGroupList list = new AgeGroupList(_ageGroupService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            var serviceDetail = _ageGroupService.GetById(id).Data;
            if (serviceDetail == null || serviceDetail.Id < 1)
                serviceDetail = new Entities.Concrete.AgeGroup();

            var model = new Models.AgeGroup.AgeGroup(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _ageGroupService.GetById(id).Data;
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
            #region  AgeGroup Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult( _localizer.GetString("Error_UserNotFound")));
            }
            #endregion
            
            Entities.Concrete.AgeGroup entity = ageGroup.GetBusinessModel();
            if (entity.Id > 0)
            {
                var res = _ageGroupService.Delete(entity);
                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.AgeGroup.AgeGroup ageGroup)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (ageGroup != null)
            {
                Entities.Concrete.AgeGroup entity = ageGroup.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorResult(false, _localizerShared.GetString("Error_SystemError")));

                var result = _ageGroupService.Add(entity);

                if (ageGroup.EntityId > 0)
                {
                    result = _ageGroupService.Update(entity);
                }
                else
                {
                    result = _ageGroupService.Add(entity);
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
