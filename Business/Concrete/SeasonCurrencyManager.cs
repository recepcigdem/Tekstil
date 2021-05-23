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
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;

namespace Business.Concrete
{
    public class SeasonCurrencyManager : ISeasonCurrencyService
    {
        private ISeasonCurrencyDal _seasonCurrencyDal;
        private ICountryShippingMultiplierService _countryShippingMultiplierService;
        private IPaymentMethodShareService _paymentMethodShareService;
        private ITariffNoDetailService _tariffNoDetailService;

        public SeasonCurrencyManager(ISeasonCurrencyDal seasonCurrencyDal, ICountryShippingMultiplierService countryShippingMultiplierService, IPaymentMethodShareService paymentMethodShareService, ITariffNoDetailService tariffNoDetailService)
        {
            _seasonCurrencyDal = seasonCurrencyDal;
            _countryShippingMultiplierService = countryShippingMultiplierService;
            _paymentMethodShareService = paymentMethodShareService;
            _tariffNoDetailService = tariffNoDetailService;
        }

        public IDataServiceResult<List<SeasonCurrency>> GetAll(int customerId)
        {
            var dbResult = _seasonCurrencyDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<SeasonCurrency>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<SeasonCurrency>> GetAllBySeasonId(int seasonId)
        {
            var dbResult = _seasonCurrencyDal.GetAll(x => x.SeasonId == seasonId);

            return new SuccessDataServiceResult<List<SeasonCurrency>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<SeasonCurrency> GetById(int seasonCurrencyId)
        {
            var dbResult = _seasonCurrencyDal.Get(p => p.Id == seasonCurrencyId);
            if (dbResult == null)
                return new SuccessDataServiceResult<SeasonCurrency>(false, "Error_SystemError");

            return new SuccessDataServiceResult<SeasonCurrency>(dbResult, true, "Listed");
        }

        public IServiceResult Add(SeasonCurrency seasonCurrency)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCurrencyTypeExists(seasonCurrency));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonCurrencyDal.Add(seasonCurrency);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(SeasonCurrency seasonCurrency)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCurrencyTypeExists(seasonCurrency));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonCurrencyDal.Update(seasonCurrency);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(SeasonCurrency seasonCurrency)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<SeasonCurrency>(false, message);
            }

            #endregion

            IServiceResult isUsedResult = BusinessRules.Run(CheckIfSeasonCurrencyIsUsed(seasonCurrency));
            if (isUsedResult.Result == false)
                return new ErrorServiceResult(false, isUsedResult.Message);

            var result = _seasonCurrencyDal.Delete(seasonCurrency);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteBySeason(Season season)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<SeasonCurrency>(false, message);
            }

            #endregion

            var seasonCurrencies = GetAllBySeasonId(season.Id);
            if (seasonCurrencies.Result == false)
                return new ErrorServiceResult(false, "SeasonCurrencyNotFound");

            foreach (var seasonCurrency in seasonCurrencies.Data)
            {
                IServiceResult isUsedResult = BusinessRules.Run(CheckIfSeasonCurrencyIsUsed(seasonCurrency));
                if (isUsedResult.Result == false)
                    return new ErrorServiceResult(false, isUsedResult.Message);

                var deleteSeasonCurrency = Delete(seasonCurrency);
                if (deleteSeasonCurrency.Result == false)
                    return new ErrorServiceResult(false, "SeasonCurrencyNotDeleted");
            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(SeasonCurrencyValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<SeasonCurrency> Save(int seasonId, int customerId, List<SeasonCurrency> seasonCurrencies)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<SeasonCurrency>(false, message);
            }

            #endregion

            var dbSeasonCurrencies = GetAllBySeasonId(seasonId).Data;
            foreach (var dbSeasonCurrency in dbSeasonCurrencies)
            {
                var control = seasonCurrencies.Any(x => x.Id == dbSeasonCurrency.Id);
                if (control != true)
                {
                    Delete(dbSeasonCurrency);
                }
            }

            foreach (var seasonCurrency in seasonCurrencies)
            {
                seasonCurrency.CustomerId = customerId;
                if (seasonCurrency.Id > 0)
                {
                    Update(seasonCurrency);
                }
                else
                {
                    Add(seasonCurrency);
                }
            }
            return new SuccessDataServiceResult<SeasonCurrency>(true, "Saved");
        }

        private ServiceResult CheckIfCurrencyTypeExists(SeasonCurrency seasonCurrency)
        {
            var result = _seasonCurrencyDal.GetAll(x => x.SeasonId == seasonCurrency.SeasonId && x.CurrencyType == seasonCurrency.CurrencyType);

            if (result.Count > 1)
                new ErrorServiceResult(false, "SeasonCurrencyCurrencyTypeAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfSeasonCurrencyIsUsed(SeasonCurrency seasonCurrency)
        {
            var countryShippingMultiplier = _countryShippingMultiplierService.GetAllBySeasonCurrencyId(seasonCurrency.CustomerId, seasonCurrency.Id);
            if (countryShippingMultiplier.Result)
                return new ErrorServiceResult(false, "Message_SeasonCurrencyIsUsedCountryShippingMultiplier");

            var paymentMethodShare = _paymentMethodShareService.GetAllBySeasonCurrencyId(seasonCurrency.CustomerId, seasonCurrency.Id);
            if (paymentMethodShare.Result)
                return new ErrorServiceResult(false, "Message_SeasonCurrencyIsUsedPaymentMethodShare");

            var tariffNoDetail = _tariffNoDetailService.GetAllBySeasonCurrencyId(seasonCurrency.CustomerId, seasonCurrency.Id);
            if (tariffNoDetail.Result)
                return new ErrorServiceResult(false, "Message_SeasonCurrencyIsUsedTariffNoDetail");

            return new ServiceResult(true, "");
        }
    }
}
