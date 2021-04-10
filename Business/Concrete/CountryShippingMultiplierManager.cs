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

        public IDataResult<List<CountryShippingMultiplier>> GetAll()
        {
            return new SuccessDataResult<List<CountryShippingMultiplier>>(true, "Listed", _countryShippingMultiplierDal.GetAll());
        }

        public IDataResult<CountryShippingMultiplier> GetById(int countryShippingMultiplierId)
        {
            return new SuccessDataResult<CountryShippingMultiplier>(true, "Listed", _countryShippingMultiplierDal.Get(p => p.Id == countryShippingMultiplierId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(CountryShippingMultiplierValidator))]
        [TransactionScopeAspect]
        public IResult Add(CountryShippingMultiplier countryShippingMultiplier)
        {
            IResult result = BusinessRules.Run(CheckIfCountryAndShippingExists(countryShippingMultiplier));

            if (result != null)
                return result;

            _countryShippingMultiplierDal.Add(countryShippingMultiplier);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(CountryShippingMultiplierValidator))]
        [TransactionScopeAspect]
        public IResult Update(CountryShippingMultiplier countryShippingMultiplier)
        {
            IResult result = BusinessRules.Run(CheckIfCountryAndShippingExists(countryShippingMultiplier));

            if (result != null)
                return result;

            _countryShippingMultiplierDal.Add(countryShippingMultiplier);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(CountryShippingMultiplier countryShippingMultiplier)
        {
            _countryShippingMultiplierDal.Delete(countryShippingMultiplier);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfCountryAndShippingExists(CountryShippingMultiplier countryShippingMultiplier)
        {
            var result = _countryShippingMultiplierDal.GetAll(x => x.SeasonId == countryShippingMultiplier.SeasonId && x.CountryId == countryShippingMultiplier.CountryId && x.ShippingMethodId == countryShippingMultiplier.ShippingMethodId).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
