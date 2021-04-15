using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UI.Models.Common;

namespace UI.Controllers
{
    public class BaseController : Controller
    {
        protected IStringLocalizer<BaseController> _localizer;
        protected IStringLocalizer _localizerShared;
        protected ILogger<BaseController> _logger;
        protected IWebHostEnvironment _env;

        public BaseController(IStringLocalizerFactory factory, IWebHostEnvironment env)
        {
            var type = typeof(Resources.SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizerShared = factory.Create("SharedResource", assemblyName.Name);
            _env = env;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();
            string ipAddress = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string method = context.HttpContext.Request.Method;
            string staffId = "0";
            bool UseParameter = Convert.ToBoolean(Core.Helper.SettingsHelper.GetValue("Log", "UseParameter"));
            StaffSession resultStaffSession = Helpers.SessionHelper.GetStaff(context.HttpContext.Request);
            //if (resultStaffSession != null)
            //{
            //    staffId = resultStaffSession.StaffId.ToString();

            //}
            //var param = "";
            //if (UseParameter)
            //{
            //    if (context.ActionArguments.Values.Count > 0)
            //    {
            //        param = JsonConvert.SerializeObject(context.ActionArguments.Values.FirstOrDefault());
            //    }
            //}

            //Helpers.LogHelper.AddLog("StaffId=" + staffId + " IpAddress=" + ipAddress + " Controller=" + controllerName + " Action=" + actionName + " Method=" + method, param);

        }
    }
}
