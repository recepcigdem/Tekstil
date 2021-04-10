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
    public class SeasonCurrencyManager : ISeasonCurrencyService
    {
        private ISeasonCurrencyDal _seasonCurrencyDal;

        public SeasonCurrencyManager(ISeasonCurrencyDal seasonCurrencyDal)
        {
            _seasonCurrencyDal = seasonCurrencyDal;
        }

        public IDataResult<List<SeasonCurrency>> GetAll()
        {
            return new SuccessDataResult<List<SeasonCurrency>>(true, "Listed", _seasonCurrencyDal.GetAll());
        }

        public IDataResult<SeasonCurrency> GetById(int seasonCurrencyId)
        {
            return new SuccessDataResult<SeasonCurrency>(true, "Listed", _seasonCurrencyDal.Get(p => p.Id == seasonCurrencyId));
        }

        [SecuredOperation("admin,season.add")]
        [ValidationAspect(typeof(SeasonCurrencyValidator))]
        [TransactionScopeAspect]
        public IResult Add(SeasonCurrency seasonCurrency)
        {
            IResult result = BusinessRules.Run(CheckIfCurrencyTypeExists(seasonCurrency));

            if (result != null)
                return result;

            _seasonCurrencyDal.Add(seasonCurrency);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,season.updated")]
        [ValidationAspect(typeof(SeasonCurrencyValidator))]
        [TransactionScopeAspect]
        public IResult Update(SeasonCurrency seasonCurrency)
        {
            IResult result = BusinessRules.Run(CheckIfCurrencyTypeExists(seasonCurrency));

            if (result != null)
                return result;

            _seasonCurrencyDal.Add(seasonCurrency);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,season.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(SeasonCurrency seasonCurrency)
        {
            _seasonCurrencyDal.Delete(seasonCurrency);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfCurrencyTypeExists(SeasonCurrency seasonCurrency)
        {
            var result = _seasonCurrencyDal.GetAll(x => x.SeasonId == seasonCurrency.SeasonId && x.CurrencyType == seasonCurrency.CurrencyType).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
       
    }
}
