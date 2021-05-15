using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class SeasonManager : ISeasonService
    {
        private ISeasonDal _seasonDal;
        private ISeasonCurrencyService _seasonCurrencyService;
        private ISeasonPlaningService _seasonPlaningService;
        private IPaymentMethodShareService _paymentMethodShareService;
        private ICountryShippingMultiplierService _countryShippingMultiplierService;
        private IModelSeasonRowNumberService _modelSeasonRowNumberService;

        public SeasonManager(ISeasonDal seasonDal, ISeasonCurrencyService seasonCurrencyService, ISeasonPlaningService seasonPlaningService, IPaymentMethodShareService paymentMethodShareService, ICountryShippingMultiplierService countryShippingMultiplierService, IModelSeasonRowNumberService modelSeasonRowNumberService)
        {
            _seasonDal = seasonDal;
            _seasonCurrencyService = seasonCurrencyService;
            _seasonPlaningService = seasonPlaningService;
            _paymentMethodShareService = paymentMethodShareService;
            _countryShippingMultiplierService = countryShippingMultiplierService;
            _modelSeasonRowNumberService = modelSeasonRowNumberService;
        }
        public IDataServiceResult<List<Season>> GetAll(int customerId)
        {
            var dbResult = _seasonDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Season>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Season> GetById(int seasonId)
        {
            var dbResult = _seasonDal.Get(p => p.Id == seasonId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Season>(false, "SystemError");

            return new SuccessDataServiceResult<Season>(dbResult, true, "Listed");
        }


        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(SeasonValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Season season)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCodeExists(season), CheckIfDescriptionExists(season));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonDal.Add(season);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(SeasonValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Season season)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCodeExists(season), CheckIfDescriptionExists(season));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonDal.Update(season);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult DeleteAll(Season season)
        {
            var seasonPlanning = _seasonPlaningService.DeleteBySeason(season);
            if (seasonPlanning.Result == false)
                return new ErrorServiceResult(false, "SeasonPlanningNotFound");

            var seasonCurrency = _seasonCurrencyService.DeleteBySeason(season);
            if (seasonCurrency.Result == false)
                return new ErrorServiceResult(false, "SeasonCurrencyNotFound");

            var paymentMethodShare = _paymentMethodShareService.DeleteBySeason(season);
            if (paymentMethodShare.Result == false)
                return new ErrorServiceResult(false, "PaymentMethodShareNotFound");

            var modelSeasonRowNumber = _modelSeasonRowNumberService.DeleteBySeason(season);
            if (modelSeasonRowNumber.Result == false)
                return new ErrorServiceResult(false, "ModelSeasonRowNumberNotFound");

            var countryShippingMultiplier = _countryShippingMultiplierService.DeleteBySeason(season);
            if (countryShippingMultiplier.Result == false)
                return new ErrorServiceResult(false, "CountryShippingMultiplierNotFound");

            var result = _seasonDal.Delete(season);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<Season> SaveAll(Season season, List<SeasonCurrency> seasonCurrencies, List<SeasonPlaning> seasonPlanings, List<PaymentMethodShare> paymentMethodShares,
            List<ModelSeasonRowNumber> modelSeasonRowNumbers, List<CountryShippingMultiplier> countryShippingMultipliers)
        {
            if (season.Id > 0)
            {
                Update(season);
            }
            else
            {
                Add(season);
            }

            _seasonPlaningService.Save(season.Id, season.CustomerId, seasonPlanings);
            _seasonCurrencyService.Save(season.Id, season.CustomerId, seasonCurrencies);
            _paymentMethodShareService.Save(season.Id, season.CustomerId, paymentMethodShares);
            _countryShippingMultiplierService.Save(season.Id, season.CustomerId, countryShippingMultipliers);
            _modelSeasonRowNumberService.Save(season.Id, season.CustomerId, modelSeasonRowNumbers);

            return new SuccessDataServiceResult<Season>(season, true, "Saved");
        }

        private ServiceResult CheckIfDescriptionExists(Season season)
        {
            var result = _seasonDal.GetAll(x => x.Description == season.Description);

            if (result.Count > 1)
                new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }
        private ServiceResult CheckIfCodeExists(Season season)
        {
            var result = _seasonDal.GetAll(x => x.Code == season.Code);

            if (result.Count > 1)
                new ErrorServiceResult(false, "CodeAlreadyExists");

            return new ServiceResult(true, "");
        }

    }
}
