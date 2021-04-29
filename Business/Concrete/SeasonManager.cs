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

        public SeasonManager(ISeasonDal seasonDal)
        {
            _seasonDal = seasonDal;
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
            ServiceResult result = BusinessRules.Run(CheckIfCodeExists(season),CheckIfDescriptionExists(season));
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
            var result = _seasonDal.Delete(season);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

       public IDataServiceResult<Season> SaveAll(Season season)
        {
            if (season.Id > 0)
            {
                Update(season);
            }
            else
            {
                Add(season);
            }

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
