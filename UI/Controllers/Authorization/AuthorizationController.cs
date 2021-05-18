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
using UI.Models.Authorization;
using Core.Aspects.Autofac.Transaction;

namespace UI.Controllers.Authorization
{
    public class AuthorizationController : BaseController
    {
        private IAuthorizationService _authorizationService;

        public AuthorizationController(IStringLocalizerFactory factory, IStringLocalizer<AuthorizationController> localizer, ILogger<AuthorizationController> logger, IWebHostEnvironment env, IAuthorizationService authorizationService) : base(factory, env)
        {
            _authorizationService = authorizationService;
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
        public JsonResult AuthorizationList()
        {
            AuthorizationList list = new AuthorizationList(_authorizationService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Authorization serviceDetail = new Entities.Concrete.Authorization();
            if (id > 0)
                serviceDetail = _authorizationService.GetById(id).Data;

            var model = new Models.Authorization.Authorization(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _authorizationService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Authorization.Authorization model = new Models.Authorization.Authorization(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Authorization Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Authorization entity = new Entities.Concrete.Authorization();
            entity.Id = Id;

            if (Id > 0)
            {
                
                var res = _authorizationService.Delete(entity);
               
                #region SystemError
                //if (TransactionScopeAspect.Message.ToString() != "")
                //{
                //    ServiceResult service = new ServiceResult();
                //    service.Result = false;
                //    service.Message = _localizerShared.GetString("Error_SystemError");
                //    return Json(service);
                //}
                #endregion

                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
               
                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Authorization.Authorization authorization)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_UserNotFound")));
            }
            #endregion

            if (authorization != null)
            {
                Entities.Concrete.Authorization entity = authorization.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                var result = _authorizationService.Save(entity);
                
                #region SystemError
                //if (TransactionScopeAspect.Message.ToString() != "")
                //{
                //    ServiceResult service = new ServiceResult();
                //    service.Result = false;
                //    service.Message = _localizerShared.GetString("Error_SystemError");
                //    return Json(service);
                //}
                #endregion

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
            var authorizationList = _authorizationService.GetAll().Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Authorization entity in authorizationList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.AuthorizationName);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
