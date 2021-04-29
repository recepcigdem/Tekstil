using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using UI.Models.Common;

namespace UI.Models.Season
{
    public class Season : BaseModel
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        #region SeasonPlanning

        public List<Entities.Concrete.SeasonPlaning> SeasonPlannings { get; set; }

        public string SubSeasonPlanningString
        {
            get { return JsonConvert.SerializeObject(SeasonPlannings); }
            set { SeasonPlannings = JsonConvert.DeserializeObject<List<Entities.Concrete.SeasonPlaning>>(value); }
        }

        public List<Entities.Concrete.SeasonPlaning> ListSeasonPlannings { get; set; }

        #endregion

        #region SeasonCurrency

        public List<Entities.Concrete.SeasonCurrency> SeasonCurrencies { get; set; }

        public string SubSeasonCurrencyString
        {
            get { return JsonConvert.SerializeObject(SeasonCurrencies); }
            set { SeasonCurrencies = JsonConvert.DeserializeObject<List<Entities.Concrete.SeasonCurrency>>(value); }
        }

        public List<Entities.Concrete.SeasonCurrency> ListSeasonCurrencies { get; set; }

        #endregion

        #region PaymentMethodShare

        public List<Entities.Concrete.PaymentMethodShare> PaymentMethodShares { get; set; }

        public string SubPaymentMethodShareString
        {
            get { return JsonConvert.SerializeObject(PaymentMethodShares); }
            set { PaymentMethodShares = JsonConvert.DeserializeObject<List<Entities.Concrete.PaymentMethodShare>>(value); }
        }

        public List<Entities.Concrete.PaymentMethodShare> ListPaymentMethodShares { get; set; }

        #endregion

        #region ModelSeasonRowNumber

        public List<Entities.Concrete.ModelSeasonRowNumber> ModelSeasonRowNumbers { get; set; }

        public string SubModelSeasonRowNumberString
        {
            get { return JsonConvert.SerializeObject(ModelSeasonRowNumbers); }
            set { ModelSeasonRowNumbers = JsonConvert.DeserializeObject<List<Entities.Concrete.ModelSeasonRowNumber>>(value); }
        }

        public List<Entities.Concrete.ModelSeasonRowNumber> ListModelSeasonRowNumbers { get; set; }

        #endregion

        #region CountryShippingMultiplier

        public List<Entities.Concrete.CountryShippingMultiplier> CountryShippingMultipliers { get; set; }

        public string SubCountryShippingMultiplierString
        {
            get { return JsonConvert.SerializeObject(CountryShippingMultipliers); }
            set { CountryShippingMultipliers = JsonConvert.DeserializeObject<List<Entities.Concrete.CountryShippingMultiplier>>(value); }
        }

        public List<Entities.Concrete.CountryShippingMultiplier> ListCountryShippingMultipliers { get; set; }

        #endregion

        #region Injections

        private ISeasonCurrencyService _seasonCurrencyService;
        private ISeasonPlaningService _seasonPlaningService;
        private IPaymentMethodShareService _paymentMethodShareService;
        private IModelSeasonRowNumberService _modelSeasonRowNumberService;
        private ICountryShippingMultiplierService _countryShippingMultiplierService;

        #endregion

        public Season() : base()
        {
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;

            SeasonPlannings = new List<SeasonPlaning>();
            ListSeasonPlannings = new List<SeasonPlaning>();

            SeasonCurrencies = new List<SeasonCurrency>();
            ListSeasonCurrencies = new List<SeasonCurrency>();

            PaymentMethodShares = new List<PaymentMethodShare>();
            ListPaymentMethodShares = new List<PaymentMethodShare>();

            ModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();
            ListModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();

            CountryShippingMultipliers = new List<CountryShippingMultiplier>();
            ListCountryShippingMultipliers = new List<CountryShippingMultiplier>();
        }

        public Season(HttpRequest request, Entities.Concrete.Season season, IStringLocalizer _localizerShared, ISeasonCurrencyService seasonCurrencyService, ISeasonPlaningService seasonPlaningService, IPaymentMethodShareService paymentMethodShareService, IModelSeasonRowNumberService modelSeasonRowNumberService, ICountryShippingMultiplierService countryShippingMultiplierService)
        {
            _seasonCurrencyService = seasonCurrencyService;
            _seasonPlaningService = seasonPlaningService;
            _paymentMethodShareService = paymentMethodShareService;
            _modelSeasonRowNumberService = modelSeasonRowNumberService;
            _countryShippingMultiplierService = countryShippingMultiplierService;

            ListSeasonPlannings = new List<SeasonPlaning>();
            ListSeasonCurrencies = new List<SeasonCurrency>();
            ListPaymentMethodShares = new List<PaymentMethodShare>();
            ListModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();
            ListCountryShippingMultipliers = new List<CountryShippingMultiplier>();

            EntityId = season.Id;
            IsActive = season.IsActive;
            Code = season.Code;
            Description = season.Description;

            #region SeasonCurrency
            var seasonCurrencyList = _seasonCurrencyService.GetAllBySeasonId(EntityId);
            if (seasonCurrencyList != null)
            {
                foreach (var seasonCurrency in seasonCurrencyList.Data)
                {
                    ListSeasonCurrencies.Add(seasonCurrency);
                }
            }
            #endregion
           
            #region SeasonPlaning
            var seasonPlaningList = _seasonPlaningService.GetAllBySeasonId(EntityId);
            if (seasonPlaningList != null)
            {
                foreach (var seasonPlaning in seasonPlaningList.Data)
                {
                    ListSeasonPlannings.Add(seasonPlaning);
                }
            }
            #endregion

            #region PaymentMethodShare
            var paymentMethodShareList = _paymentMethodShareService.GetAllBySeasonId(EntityId);
            if (paymentMethodShareList != null)
            {
                foreach (var paymentMethodShare in paymentMethodShareList.Data)
                {
                    ListPaymentMethodShares.Add(paymentMethodShare);
                }
            }
            #endregion

            #region ModelSeasonRowNumber
            var modelSeasonRowNumberList = _modelSeasonRowNumberService.GetAllBySeasonId(EntityId);
            if (modelSeasonRowNumberList != null)
            {
                foreach (var modelSeasonRowNumber in modelSeasonRowNumberList.Data)
                {
                    ListModelSeasonRowNumbers.Add(modelSeasonRowNumber);
                }
            }
            #endregion

            #region CountryShippingMultiplier
            var countryShippingMultiplierList = _countryShippingMultiplierService.GetAllBySeasonId(EntityId);
            if (countryShippingMultiplierList != null)
            {
                foreach (var countryShippingMultiplier in countryShippingMultiplierList.Data)
                {
                    ListCountryShippingMultipliers.Add(countryShippingMultiplier);
                }
            }
            #endregion

        }

        public Entities.Concrete.Season GetBusinessModel()
        {
            Entities.Concrete.Season season = new Entities.Concrete.Season();

            season.IsActive = IsActive;
            season.Code = Code;
            season.Description = Description;


            return season;
        }
    }
}
