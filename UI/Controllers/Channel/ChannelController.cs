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
using UI.Models.Channel;

namespace UI.Controllers.Channel
{
    public class ChannelController : BaseController
    {
        private IChannelService _channelService;

        public ChannelController(IStringLocalizerFactory factory, IStringLocalizer<ChannelController> localizer, ILogger<ChannelController> logger, IWebHostEnvironment env, IChannelService channelService) : base(factory, env)
        {
            _channelService = channelService;
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
        public JsonResult ChannelList()
        {
            ChannelList list = new ChannelList(Request, _channelService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Channel serviceDetail = new Entities.Concrete.Channel();
            if (id > 0)
                serviceDetail = _channelService.GetById(id).Data;

            var model = new Models.Channel.Channel(Request, serviceDetail, _localizerShared);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var serviceDetail = _channelService.GetById(id).Data;
                if (serviceDetail != null)
                {
                    Models.Channel.Channel model = new Models.Channel.Channel(Request, serviceDetail, _localizerShared);
                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Channel Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Channel entity = new Entities.Concrete.Channel();
            entity.Id = Id;

            if (Id > 0)
            {
                var res = _channelService.Delete(entity);
                if (res.Result == false)
                    res.Message = _localizer.GetString(res.Message);
                else
                    res.Message = _localizerShared.GetString(res.Message);

                return Json(res);
            }
            return null;
        }
        [HttpPost]
        public JsonResult Save(Models.Channel.Channel channel)
        {
            #region  Channel Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            if (channel != null)
            {
                Entities.Concrete.Channel entity = channel.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizerShared.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                var result = _channelService.Save(entity);
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

            var channelList = _channelService.GetAll(customerId).Data;
            List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
            foreach (Entities.Concrete.Channel entity in channelList)
            {
                Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.ChannelName);
                data.Add(model);
            }
            return Json(data);
        }

    }
}
