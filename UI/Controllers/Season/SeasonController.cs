using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using UI.Helpers;
using UI.Models;
using UI.Models.Season;

namespace UI.Controllers.Season
{
    public class SeasonController : BaseController
    {
        private ISeasonService _seasonService;
        private ISeasonCurrencyService _seasonCurrencyService;
        private ISeasonPlaningService _seasonPlaningService;
        private IModelSeasonRowNumberService _modelSeasonRowNumberService;
        private IPaymentMethodShareService _paymentMethodShareService;
        private ICountryShippingMultiplierService _countryShippingMultiplierService;


        public SeasonController(IStringLocalizerFactory factory, IStringLocalizer<SeasonController> localizer, ILogger<SeasonController> logger, IWebHostEnvironment env,
            ISeasonService seasonService, ISeasonCurrencyService seasonCurrencyService, ISeasonPlaningService seasonPlaningService,
            IModelSeasonRowNumberService modelSeasonRowNumberService, IPaymentMethodShareService paymentMethodShareService, ICountryShippingMultiplierService countryShippingMultiplierService) : base(factory, env)
        {
            _seasonService = seasonService;
            _seasonCurrencyService = seasonCurrencyService;
            _seasonPlaningService = seasonPlaningService;
            _modelSeasonRowNumberService = modelSeasonRowNumberService;
            _paymentMethodShareService = paymentMethodShareService;
            _countryShippingMultiplierService = countryShippingMultiplierService;

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
        public JsonResult SeasonList()
        {
            SeasonList list = new SeasonList(Request, _seasonService);
            return Json(list);
        }

        public ActionResult Detail(int id)
        {
            Entities.Concrete.Season serviceDetail = new Entities.Concrete.Season();
            if (id > 0)
                serviceDetail = _seasonService.GetById(id).Data;

            var model = new Models.Season.Season(Request, serviceDetail, _localizerShared, _seasonCurrencyService, _seasonPlaningService,
                _paymentMethodShareService, _modelSeasonRowNumberService, _countryShippingMultiplierService);
            return View(model);
        }

        public ActionResult DetailReadOnly(int id)
        {
            if (id > 0)
            {
                var season = _seasonService.GetById(id).Data;
                if (season != null)
                {
                    Models.Season.Season model = new Models.Season.Season(Request, season, _localizerShared, _seasonCurrencyService, _seasonPlaningService,
                        _paymentMethodShareService, _modelSeasonRowNumberService, _countryShippingMultiplierService);

                    return PartialView(model);
                }
                return null;
            }
            return null;
        }

        public JsonResult Delete(int Id)
        {
            #region  Staff Session Control

            var sessionHelper = Helpers.HttpHelper.StaffSessionControl(Request);
            if (!sessionHelper.IsSuccess)
            {
                return Json(new ErrorServiceResult(false, _localizer.GetString("Error_UserNotFound")));
            }
            #endregion

            Entities.Concrete.Season entity = new Entities.Concrete.Season();
            entity.Id = Id;
            if (entity.Id > 0)
            {
                var res = _seasonService.DeleteAll(entity);
                res.Message = _localizer.GetString(res.Message);
                return Json(res);
            }
            return null;
        }

        [HttpPost]
        public JsonResult Save(Models.Season.Season season)
        {
            if (season != null)
            {
                Entities.Concrete.Season entity = season.GetBusinessModel();
                if (entity == null)
                    return Json(new ErrorServiceResult(false, _localizer.GetString("Error_SystemError")));

                entity.CustomerId = SessionHelper.GetStaff(Request).CustomerId;

                List<SeasonCurrency> seasonCurrencies = season.ListSeasonCurrencies;
                List<SeasonPlaning> seasonPlanings = season.ListSeasonPlannings;
                List<ModelSeasonRowNumber> modelSeasonRowNumbers = season.ListModelSeasonRowNumbers;
                List<PaymentMethodShare> paymentMethodShares = season.ListPaymentMethodShare;
                List<CountryShippingMultiplier> countryShippingMultipliers = season.ListCountryShippingMultiplier;

                var result = _seasonService.SaveAll(entity, seasonCurrencies, seasonPlanings, paymentMethodShares, modelSeasonRowNumbers, countryShippingMultipliers);
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

        public JsonResult ComboListSeasonCurrency(int seasonId)
        {
            var seasonCurrencyList = _seasonCurrencyService.GetAllBySeasonId(seasonId);
            if (seasonCurrencyList.Result)
            {
                List<Models.Common.ComboData> data = new List<Models.Common.ComboData>();
                foreach (SeasonCurrency entity in seasonCurrencyList.Data)
                {
                    Models.Common.ComboData model = new Models.Common.ComboData(entity.Id, entity.CurrencyType);
                    data.Add(model);
                }
                return Json(data);
            }
            return null;
        }
    }
}
