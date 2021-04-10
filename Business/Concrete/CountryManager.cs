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
    public class CountryManager : ICountryService
    {
        private ICountryDal _countryDal;

        public CountryManager(ICountryDal countryDal)
        {
            _countryDal = countryDal;
        }

        public IDataResult<List<Country>> GetAll()
        {
            return new SuccessDataResult<List<Country>>(true, "Listed", _countryDal.GetAll());
        }

        public IDataResult<Country> GetById(int countryId)
        {
            return new SuccessDataResult<Country>(true, "Listed", _countryDal.Get(p => p.Id == countryId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(CountryValidator))]
        [TransactionScopeAspect]
        public IResult Add(Country country)
        {
            IResult result = BusinessRules.Run( CheckIfDescriptionExists(country));

            if (result != null)
                return result;

            _countryDal.Add(country);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(CountryValidator))]
        [TransactionScopeAspect]
        public IResult Update(Country country)
        {
            IResult result = BusinessRules.Run( CheckIfDescriptionExists(country));

            if (result != null)
                return result;

            _countryDal.Add(country);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Country country)
        {
            _countryDal.Delete(country);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Country country)
        {
            var result = _countryDal.GetAll(x => x.Description == country.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
