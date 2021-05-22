using System.Collections.Generic;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Linq;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;

namespace Business.Concrete
{
    public class CountryShippingMultiplierManager : ICountryShippingMultiplierService
    {
        private ICountryShippingMultiplierDal _countryShippingMultiplierDal;

        public CountryShippingMultiplierManager(ICountryShippingMultiplierDal countryShippingMultiplierDal)
        {
            _countryShippingMultiplierDal = countryShippingMultiplierDal;
        }

        public IDataServiceResult<List<CountryShippingMultiplier>> GetAll(int customerId)
        {
            var dbResult = _countryShippingMultiplierDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<CountryShippingMultiplier>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CountryShippingMultiplier>> GetAllBySeasonId(int seasonId)
        {
            var dbResult = _countryShippingMultiplierDal.GetAll(x =>x.SeasonId==seasonId);

            return new SuccessDataServiceResult<List<CountryShippingMultiplier>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CountryShippingMultiplier>> GetAllBySeasonCurrencyId(int customerId, int seasonCurrencyId)
        {
            var dbResult = _countryShippingMultiplierDal.GetAll(x =>x.CustomerId==customerId && x.SeasonCurrencyId == seasonCurrencyId);

            return new SuccessDataServiceResult<List<CountryShippingMultiplier>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CountryShippingMultiplier>> GetAllByShippingMethodId(int customerId, int shippingMethodId)
        {
            var dbResult = _countryShippingMultiplierDal.GetAll(x => x.CustomerId == customerId && x.ShippingMethodId == shippingMethodId);

            return new SuccessDataServiceResult<List<CountryShippingMultiplier>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CountryShippingMultiplier>> GetAllByCountryId(int customerId, int countryId)
        {
            var dbResult = _countryShippingMultiplierDal.GetAll(x => x.CustomerId == customerId && x.CountryId == countryId);

            return new SuccessDataServiceResult<List<CountryShippingMultiplier>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CountryShippingMultiplier> GetById(int countryShippingMultipliersId)
        {
            var dbResult = _countryShippingMultiplierDal.Get(p => p.Id == countryShippingMultipliersId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CountryShippingMultiplier>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CountryShippingMultiplier>(dbResult, true, "Listed");
        }

        public IServiceResult Add(CountryShippingMultiplier countryShippingMultiplier)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCountryAndShippingExists(countryShippingMultiplier));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _countryShippingMultiplierDal.Add(countryShippingMultiplier);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(CountryShippingMultiplier countryShippingMultiplier)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCountryAndShippingExists(countryShippingMultiplier));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _countryShippingMultiplierDal.Update(countryShippingMultiplier);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(CountryShippingMultiplier countryShippingMultiplier)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<CountryShippingMultiplier>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _countryShippingMultiplierDal.Delete(countryShippingMultiplier);
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
                return new DataServiceResult<CountryShippingMultiplier>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var countryShippingMultipliers = GetAllBySeasonId(season.Id);
            if (countryShippingMultipliers.Result == false)
                return new ErrorServiceResult(false, "CountryShippingMultiplierNotFound");

            foreach (var countryShippingMultiplier in countryShippingMultipliers.Data)
            {
                var deleteCountryShippingMultiplier = Delete(countryShippingMultiplier);
                if (deleteCountryShippingMultiplier.Result == false)
                    return new ErrorServiceResult(false, "CountryShippingMultiplierNotDeleted");
            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(CountryShippingMultiplierValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<CountryShippingMultiplier> Save(int seasonId, int customerId, List<CountryShippingMultiplier> countryShippingMultipliers)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<CountryShippingMultiplier>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var dbCountryShippingMultipliers = GetAllBySeasonId(seasonId).Data;
            foreach (var dbCountryShippingMultiplier in dbCountryShippingMultipliers)
            {
                var control = countryShippingMultipliers.Any(x => x.Id == dbCountryShippingMultiplier.Id);
                if (control != true)
                {
                    Delete(dbCountryShippingMultiplier);
                }
            }

            foreach (var countryShippingMultiplier in countryShippingMultipliers)
            {
                countryShippingMultiplier.CustomerId = customerId;
                if (countryShippingMultiplier.Id > 0)
                {
                    Update(countryShippingMultiplier);
                }
                else
                {
                    Add(countryShippingMultiplier);
                }
            }
            return new SuccessDataServiceResult<CountryShippingMultiplier>(true, "Saved");
        }

        private ServiceResult CheckIfCountryAndShippingExists(CountryShippingMultiplier countryShippingMultiplier)
        {
            var result = _countryShippingMultiplierDal.GetAll(x => x.SeasonId == countryShippingMultiplier.SeasonId && x.CountryId == countryShippingMultiplier.CountryId && x.ShippingMethodId == countryShippingMultiplier.ShippingMethodId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "CountryShippingMultiplierDescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
