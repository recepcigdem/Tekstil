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

        public IDataServiceResult<CountryShippingMultiplier> GetById(int countryShippingMultipliersId)
        {
            var dbResult = _countryShippingMultiplierDal.Get(p => p.Id == countryShippingMultipliersId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CountryShippingMultiplier>(false, "SystemError");

            return new SuccessDataServiceResult<CountryShippingMultiplier>(dbResult, true, "Listed");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(CountryShippingMultiplierValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(CountryShippingMultiplier countryShippingMultiplier)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCountryAndShippingExists(countryShippingMultiplier));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _countryShippingMultiplierDal.Add(countryShippingMultiplier);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(CountryShippingMultiplierValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(CountryShippingMultiplier countryShippingMultiplier)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCountryAndShippingExists(countryShippingMultiplier));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _countryShippingMultiplierDal.Update(countryShippingMultiplier);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult Delete(CountryShippingMultiplier countryShippingMultiplier)
        {
            var result = _countryShippingMultiplierDal.Delete(countryShippingMultiplier);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult DeleteBySeason(Season season)
        {
            var countryShippingMultipliers = GetAllBySeasonId(season.Id);
            if (countryShippingMultipliers.Result == false)
                return new ErrorServiceResult(false, "CountryShippingMultiplierNotDeleted");

            foreach (var countryShippingMultiplier in countryShippingMultipliers.Data)
            {
                var deleteCountryShippingMultiplier = Delete(countryShippingMultiplier);
                if (deleteCountryShippingMultiplier.Result == false)
                    return new ErrorServiceResult(false, "CountryShippingMultiplierNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<CountryShippingMultiplier> Save(int customerId, List<CountryShippingMultiplier> countryShippingMultipliers)
        {
            var dbCountryShippingMultipliers = GetAll(customerId).Data;
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
                new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
