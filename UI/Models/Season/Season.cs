using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Season;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using UI.Models.Common;
using Entities.Concrete.Enums.DefinitionTitle;

namespace UI.Models.Season
{
    public class Season : BaseModel
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int ModelSeasonRowNumberProductGroupId { get; set; }
        public int ModelSeasonRowNumberMaxNumber { get; set; }

        #region SeasonPlanning

        public List<SeasonPlaning> SeasonPlannings { get; set; }

        public string SubSeasonPlanningString
        {
            get
            {
                return JsonConvert.SerializeObject(SeasonPlannings);

            }
            set
            {
                SeasonPlannings = JsonConvert.DeserializeObject<List<SeasonPlaning>>(value);
                
            }
        }

        public List<SeasonPlaning> ListSeasonPlannings { get; set; }

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

        public SubEntity<SeasonPaymentMethodShareDto> PaymentMethodShares { get; set; }

        public string SubPaymentMethodShareString
        {
            get { return JsonConvert.SerializeObject(PaymentMethodShares); }
            set { PaymentMethodShares = JsonConvert.DeserializeObject<SubEntity<SeasonPaymentMethodShareDto>>(value); }
        }

        public List<SeasonPaymentMethodShareDto> ListPaymentMethodShares { get; set; }

        public List<PaymentMethodShare> ListPaymentMethodShare { get; set; }

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

        public SubEntity<SeasonCountryShippingMultiplierDto> CountryShippingMultipliers { get; set; }

        public string SubCountryShippingMultiplierString
        {
            get { return JsonConvert.SerializeObject(CountryShippingMultipliers); }
            set { CountryShippingMultipliers = JsonConvert.DeserializeObject<SubEntity<SeasonCountryShippingMultiplierDto>>(value); }
        }

        public List<SeasonCountryShippingMultiplierDto> ListCountryShippingMultipliers { get; set; }
        public List<CountryShippingMultiplier> ListCountryShippingMultiplier { get; set; }

        #endregion

        #region Injections

        private ISeasonCurrencyService _seasonCurrencyService;
        private ISeasonPlaningService _seasonPlaningService;
        private IPaymentMethodShareService _paymentMethodShareService;
        private IModelSeasonRowNumberService _modelSeasonRowNumberService;
        private ICountryShippingMultiplierService _countryShippingMultiplierService;
        private IDefinitionTitleService _definitionTitleService;
        private IDefinitionService _definitionService;

        #endregion

        public Season() : base()
        {
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
            ModelSeasonRowNumberProductGroupId = 0;
            ModelSeasonRowNumberMaxNumber = 0;


            SeasonPlannings = new List<SeasonPlaning>();
            ListSeasonPlannings = new List<SeasonPlaning>();

            SeasonCurrencies = new List<SeasonCurrency>();
            ListSeasonCurrencies = new List<SeasonCurrency>();

            PaymentMethodShares = new SubEntity<SeasonPaymentMethodShareDto>();
            ListPaymentMethodShares = new List<SeasonPaymentMethodShareDto>();
            ListPaymentMethodShare = new List<PaymentMethodShare>();

            ModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();
            ListModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();

            CountryShippingMultipliers = new SubEntity<SeasonCountryShippingMultiplierDto>();
            ListCountryShippingMultipliers = new List<SeasonCountryShippingMultiplierDto>();
            ListCountryShippingMultiplier = new List<CountryShippingMultiplier>();
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
            ListPaymentMethodShares = new List<SeasonPaymentMethodShareDto>();
            ListModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();
            ListCountryShippingMultipliers = new List<SeasonCountryShippingMultiplierDto>();

            EntityId = season.Id;
            IsActive = season.IsActive;
            Code = season.Code;
            Description = season.Description;

            ModelSeasonRowNumberProductGroupId = 0;
            ModelSeasonRowNumberMaxNumber = 0;

            #region SeasonCurrency
            var seasonCurrencyList = _seasonCurrencyService.GetAllBySeasonId(EntityId);
            if (seasonCurrencyList.Data.Count > 0)
            {
                foreach (var seasonCurrency in seasonCurrencyList.Data)
                {
                    ListSeasonCurrencies.Add(seasonCurrency);
                }
            }
            #endregion

            #region SeasonPlaning
            var seasonPlaningList = _seasonPlaningService.GetAllBySeasonId(EntityId);
            if (seasonPlaningList.Data.Count > 0)
            {
                foreach (var seasonPlaning in seasonPlaningList.Data)
                {
                    ListSeasonPlannings.Add(seasonPlaning);
                }
            }
            #endregion

            #region PaymentMethodShare

            var paymentMethodShareList = _paymentMethodShareService.GetAllBySeasonId(EntityId);
            if (paymentMethodShareList.Data.Count>0)
            {
                SeasonPaymentMethodShareDto paymentMethodShareDto = new SeasonPaymentMethodShareDto();
                foreach (var paymentMethodShare in paymentMethodShareList.Data)
                {
                    paymentMethodShareDto.CustomerId = CustomerId;
                    paymentMethodShareDto.SeasonId = EntityId;
                    paymentMethodShareDto.PaymentMethodId = paymentMethodShare.Id;
                    paymentMethodShareDto.CenterShare = paymentMethodShare.CenterShare;
                    paymentMethodShareDto.CenterShareEuro = paymentMethodShare.CenterShareEuro;
                    paymentMethodShareDto.AccessoryCenterShareEuro = paymentMethodShare.AccessoryCenterShareEuro;
                    paymentMethodShareDto.SeasonCurrencyId = paymentMethodShare.SeasonCurrencyId;

                    var seasonCurrency = _seasonCurrencyService.GetById(paymentMethodShare.SeasonCurrencyId);
                    if (seasonCurrency != null)
                    {
                        paymentMethodShareDto.SeasonCurrency = seasonCurrency.Data.CurrencyType;
                        paymentMethodShareDto.ExchangeRates = seasonCurrency.Data.ExchangeRate;
                        paymentMethodShareDto.CenterShareTl = (paymentMethodShareDto.CenterShare * seasonCurrency.Data.ExchangeRate);
                    }
                }

                ListPaymentMethodShares.Add(paymentMethodShareDto);
            }


            #endregion

            #region ModelSeasonRowNumber
            var modelSeasonRowNumberList = _modelSeasonRowNumberService.GetAllBySeasonId(EntityId);
            if (modelSeasonRowNumberList.Data.Count > 0)
            {
                foreach (var modelSeasonRowNumber in modelSeasonRowNumberList.Data)
                {
                    ListModelSeasonRowNumbers.Add(modelSeasonRowNumber);
                }
            }
            #endregion

            #region CountryShippingMultiplier
            var countryShippingMultiplierList = _countryShippingMultiplierService.GetAllBySeasonId(EntityId);
            if (countryShippingMultiplierList.Data.Count > 0)
            {

                SeasonCountryShippingMultiplierDto countryShippingMultiplierDto =
                    new SeasonCountryShippingMultiplierDto();
                foreach (var countryShippingMultiplier in countryShippingMultiplierList.Data)
                {
                    countryShippingMultiplierDto.CustomerId = CustomerId;
                    countryShippingMultiplierDto.SeasonId = EntityId;
                    countryShippingMultiplierDto.SeasonCurrencyId = countryShippingMultiplier.SeasonCurrencyId;
                    countryShippingMultiplierDto.ShippingMethodId = countryShippingMultiplier.ShippingMethodId;
                    countryShippingMultiplierDto.CountryId = countryShippingMultiplier.CountryId;
                    countryShippingMultiplierDto.TestPrice = countryShippingMultiplier.TestPrice;
                    countryShippingMultiplierDto.CargoPercentage = countryShippingMultiplier.CargoPercentage;
                    countryShippingMultiplierDto.FreightPercentage = countryShippingMultiplier.FreightPercentage;
                    countryShippingMultiplierDto.InsurancePercentage = countryShippingMultiplier.InsurancePercentage;
                    countryShippingMultiplierDto.Multiplier = countryShippingMultiplier.Multiplier;
                    countryShippingMultiplierDto.TestPercentage = countryShippingMultiplier.TestPercentage;

                    var seasonCurrency = _seasonCurrencyService.GetById(countryShippingMultiplier.SeasonCurrencyId);
                    if (seasonCurrency != null)
                    {
                        countryShippingMultiplierDto.SeasonCurrency = seasonCurrency.Data.CurrencyType;
                        countryShippingMultiplierDto.ExchangeRates = seasonCurrency.Data.ExchangeRate;
                        countryShippingMultiplierDto.TestPriceTl = (countryShippingMultiplier.TestPrice * seasonCurrency.Data.ExchangeRate);
                    }

                }
                ListCountryShippingMultipliers.Add(countryShippingMultiplierDto);
            }
            #endregion

        }

        public Entities.Concrete.Season GetBusinessModel()
        {
            Entities.Concrete.Season season = new Entities.Concrete.Season();

            season.IsActive = IsActive;
            season.Code = Code;
            season.Description = Description;

            #region SeasonPlannings

            if (SeasonPlannings != null)
            {
                ListSeasonPlannings = new List<SeasonPlaning>();
                foreach (var item in SeasonPlannings)
                {
                    Entities.Concrete.SeasonPlaning seasonPlaning = new SeasonPlaning();
                    if (item.Id > 0)
                    {
                        seasonPlaning.Id = item.Id;
                    }
                    seasonPlaning.CustomerId = item.CustomerId;
                    seasonPlaning.SeasonId = item.SeasonId;
                    seasonPlaning.ProductGroupId = item.ProductGroupId;
                    seasonPlaning.PlanUnitQuantity = item.PlanUnitQuantity;
                    seasonPlaning.PlanModelMain = item.PlanModelMain;
                    seasonPlaning.PlanModelMidi = item.PlanModelMidi;
                    seasonPlaning.PlanModelMini = item.PlanModelMini;
                    seasonPlaning.PlanOptionMain = item.PlanOptionMain;
                    seasonPlaning.PlanOptionMidi = item.PlanOptionMidi;
                    seasonPlaning.PlanOptionMini = item.PlanOptionMini;
                    seasonPlaning.PlanQuantityMain = item.PlanQuantityMain;
                    seasonPlaning.PlanQuantityMidi = item.PlanQuantityMidi;
                    seasonPlaning.PlanQuantityMini = item.PlanQuantityMini;
                    seasonPlaning.PlanTotalQuantity = ((seasonPlaning.PlanOptionMain * seasonPlaning.PlanQuantityMain) +
                                                       (seasonPlaning.PlanOptionMini * seasonPlaning.PlanQuantityMini) +
                                                       (seasonPlaning.PlanOptionMidi * seasonPlaning.PlanQuantityMidi));

                    seasonPlaning.RealModelMain = item.RealModelMain;
                    seasonPlaning.RealModelMidi = item.RealModelMidi;
                    seasonPlaning.RealModelMini = item.RealModelMini;
                    seasonPlaning.RealOptionMain = item.RealOptionMain;
                    seasonPlaning.RealOptionMidi = item.RealOptionMidi;
                    seasonPlaning.RealOptionMini = item.RealOptionMini;
                    seasonPlaning.RealQuantityMain = item.RealQuantityMain;
                    seasonPlaning.RealQuantityMidi = item.RealQuantityMidi;
                    seasonPlaning.RealQuantityMini = item.RealQuantityMini;
                    seasonPlaning.RealTotalQuantity = ((seasonPlaning.RealOptionMain * seasonPlaning.RealQuantityMain) +
                                                       (seasonPlaning.RealOptionMini * seasonPlaning.RealQuantityMini) +
                                                       (seasonPlaning.RealOptionMidi * seasonPlaning.RealQuantityMidi));
                    ListSeasonPlannings.Add(seasonPlaning);
                }
            }
            

            #endregion

            #region SeasonCurrency

            if (SeasonCurrencies != null)
            {
                ListSeasonCurrencies = new List<SeasonCurrency>();
                foreach (var item in SeasonCurrencies)
                {
                    Entities.Concrete.SeasonCurrency seasonCurrency = new SeasonCurrency();
                    if (item.Id > 0)
                    {
                        seasonCurrency.Id = item.Id;
                    }
                    seasonCurrency.CustomerId = item.CustomerId;
                    seasonCurrency.SeasonId = item.SeasonId;
                    seasonCurrency.IsDefault = item.IsDefault;
                    seasonCurrency.CurrencyType = item.CurrencyType;
                    seasonCurrency.ExchangeRate = item.ExchangeRate;

                    ListSeasonCurrencies.Add(seasonCurrency);
                }
            }
            
            #endregion

            #region ModelSeasonRowNumber

            if (ModelSeasonRowNumbers!=null)
            {
                ListModelSeasonRowNumbers = new List<ModelSeasonRowNumber>();
                foreach (var item in ModelSeasonRowNumbers)
                {
                    Entities.Concrete.ModelSeasonRowNumber modelSeasonRowNumber = new ModelSeasonRowNumber();
                    if (item.Id > 0)
                    {
                        modelSeasonRowNumber.Id = item.Id;
                    }
                    modelSeasonRowNumber.CustomerId = item.CustomerId;
                    modelSeasonRowNumber.SeasonId = item.SeasonId;
                    modelSeasonRowNumber.ProductGroupId = item.ProductGroupId;
                    modelSeasonRowNumber.RowNumber = item.RowNumber;
                    modelSeasonRowNumber.IsActive = item.IsActive;

                    ListModelSeasonRowNumbers.Add(modelSeasonRowNumber);
                }
            }
            

            #endregion

            #region PaymentMethodShare

            if (PaymentMethodShares.data != null)
            {
                ListPaymentMethodShare = new List<PaymentMethodShare>();
                foreach (var item in PaymentMethodShares.data)
                {
                    Entities.Concrete.PaymentMethodShare paymentMethodShare = new PaymentMethodShare();
                    if (item.Id > 0)
                    {
                        paymentMethodShare.Id = item.Id;
                    }
                    paymentMethodShare.CustomerId = item.CustomerId;
                    paymentMethodShare.SeasonId = item.SeasonId;
                    paymentMethodShare.PaymentMethodId = item.PaymentMethodId;
                    paymentMethodShare.SeasonCurrencyId = item.SeasonCurrencyId;
                    paymentMethodShare.CenterShare = item.CenterShare;
                    paymentMethodShare.CenterShareEuro = item.CenterShareEuro;
                    paymentMethodShare.AccessoryCenterShareEuro = item.AccessoryCenterShareEuro;

                    ListPaymentMethodShare.Add(paymentMethodShare);
                }
            }
            
            #endregion

            #region CountryShippingMultiplier

            if (CountryShippingMultipliers.data != null)
            {
                ListCountryShippingMultiplier = new List<CountryShippingMultiplier>();
                foreach (var item in CountryShippingMultipliers.data)
                {
                    Entities.Concrete.CountryShippingMultiplier countryShippingMultiplier = new CountryShippingMultiplier();
                    if (item.Id > 0)
                    {
                        countryShippingMultiplier.Id = item.Id;
                    }
                    countryShippingMultiplier.CustomerId = item.CustomerId;
                    countryShippingMultiplier.SeasonId = item.SeasonId;
                    countryShippingMultiplier.CountryId = item.CountryId;
                    countryShippingMultiplier.ShippingMethodId = item.ShippingMethodId;
                    countryShippingMultiplier.SeasonCurrencyId = item.SeasonCurrencyId;
                    countryShippingMultiplier.Multiplier = item.Multiplier;
                    countryShippingMultiplier.TestPrice = item.TestPrice;
                    countryShippingMultiplier.TestPercentage = item.TestPercentage;
                    countryShippingMultiplier.CargoPercentage = item.CargoPercentage;
                    countryShippingMultiplier.InsurancePercentage = item.InsurancePercentage;
                    countryShippingMultiplier.FreightPercentage = item.FreightPercentage;

                    ListCountryShippingMultiplier.Add(countryShippingMultiplier);
                }
            }
            
            #endregion

            return season;
        }
    }
}
