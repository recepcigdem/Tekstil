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
using UI.Models.Test;

namespace UI.Controllers.Test
{
    public class TestController : BaseController
    {
        private ITestService _testService;

        public TestController(IStringLocalizerFactory factory, IStringLocalizer<TestController> localizer, ILogger<TestController> logger, IWebHostEnvironment env, ITestService testService) : base(factory, env)
        {
            _testService = testService;
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
        public JsonResult TestList()
        {
            TestList list = new TestList(Request, _testService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Test serviceDetail = new Entities.Concrete.Test();
            if (id > 0)
                serviceDetail = _testService.GetById(id).Data;

            var model = new Models.Test.Test(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _testService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Test.Test model = new Models.Test.Test(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Test Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Test entity = new Entities.Concrete.Test();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _testService.Delete(entity);
                return Json(res);
            }
            return null;
        }
        
        [HttpPost]
        public JsonResult Save(Models.Test.Test test)
        {
            #region  Test Session Control

            var sessionHelper = HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (test != null)
            {
                Entities.Concrete.Test entity = test.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var result = _testService.Save(entity);
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
            var customerId = SessionHelper.GetStaff(Request).CustomerId;

            var testList = _testService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Test entity in testList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.Description);
                data.Add(model);
            }
            return Json(data);
        }

    }
}