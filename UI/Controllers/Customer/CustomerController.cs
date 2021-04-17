using Business.Abstract;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using UI.Models;
using UI.Models.Customer;

namespace UI.Controllers.Customer
{
    public class CustomerController : BaseController
    {
        private ICustomerService _customerService;

        public CustomerController(IStringLocalizerFactory factory, IStringLocalizer<CustomerController> localizer, ILogger<CustomerController> logger, IWebHostEnvironment env, ICustomerService customerService) : base(factory, env)
        {
            _customerService = customerService;
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
        public JsonResult CustomerList()
        {
            CustomerList list = new CustomerList(_customerService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            var serviceDetail = _customerService.GetById(id).Data;
            if (serviceDetail == null || serviceDetail.Id < 1)
                serviceDetail = new Entities.Concrete.Customer();

            var model = new Models.Customer.Customer(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _customerService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Customer.Customer model = new Models.Customer.Customer(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(Models.Customer.Customer customer)
        {
            #region  Customer Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Customer entity = customer.GetBusinessModel();
            if (entity.Id > 0)
            {
                var res = _customerService.Delete(entity);
                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Customer.Customer customer)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorResult(_localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (customer != null)
            {
                Entities.Concrete.Customer entity = customer.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorResult(false, _localizerShared.GetString("Error_SystemError")));

                var result = _customerService.Save(entity);
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
        public JsonResult ComboList()
        {
            var customerList = _customerService.GetAll().Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Customer entity in customerList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.CustomerName);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
