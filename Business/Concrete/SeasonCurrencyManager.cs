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
                return new SuccessDataServiceResult<SeasonCurrency>(false, "SystemError");

            return new SuccessDataServiceResult<SeasonCurrency>(dbResult, true, "Listed");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(SeasonCurrencyValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(SeasonCurrency seasonCurrency)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCurrencyTypeExists(seasonCurrency));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonCurrencyDal.Add(seasonCurrency);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(SeasonCurrencyValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(SeasonCurrency seasonCurrency)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCurrencyTypeExists(seasonCurrency));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonCurrencyDal.Update(seasonCurrency);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult Delete(SeasonCurrency seasonCurrency)
        {
            var result = _seasonCurrencyDal.Delete(seasonCurrency);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult DeleteBySeason(Season season)
        {
            var seasonCurrencies = GetAllBySeasonId(season.Id);
            if (seasonCurrencies.Result == false)
                return new ErrorServiceResult(false, "SeasonCurrencyNotFound");

            foreach (var seasonCurrency in seasonCurrencies.Data)
            {
                var deleteSeasonCurrency = Delete(seasonCurrency);
                if (deleteSeasonCurrency.Result == false)
                    return new ErrorServiceResult(false, "SeasonCurrencyNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<SeasonCurrency> Save(int customerId, List<SeasonCurrency> seasonCurrencies)
        {
            var dbSeasonCurrencies = GetAll(customerId).Data;
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
                new ErrorServiceResult(false, "CurrencyTypeAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
