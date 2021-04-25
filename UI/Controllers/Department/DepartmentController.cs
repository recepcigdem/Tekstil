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
using UI.Models.Department;

namespace UI.Controllers.Department
{
    public class DepartmentController : BaseController
    {
        private IDepartmentService _departmentService;

        public DepartmentController(IStringLocalizerFactory factory, IStringLocalizer<DepartmentController> localizer, ILogger<DepartmentController> logger, IWebHostEnvironment env, IDepartmentService departmentService) : base(factory, env)
        {
            _departmentService = departmentService;
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
        public JsonResult DepartmentList()
        {
            DepartmentList list = new DepartmentList(_departmentService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            var serviceDetail = _departmentService.GetById(id).Data;
            if (serviceDetail == null || serviceDetail.Id < 1)
                serviceDetail = new Entities.Concrete.Department();

            var model = new Models.Department.Department(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _departmentService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Department.Department model = new Models.Department.Department(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Department Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Department entity = new Entities.Concrete.Department();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _departmentService.Delete(entity);
                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Department.Department department)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (department != null)
            {
                Entities.Concrete.Department entity = department.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorResult(false, _localizerShared.GetString("Error_SystemError")));

                var result = _departmentService.Save(entity);
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
            var departmentList = _departmentService.GetAll().Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Department entity in departmentList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.DepartmentName);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
