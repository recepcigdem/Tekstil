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

        public IDataResult<List<Season>> GetAll()
        {
            return new SuccessDataResult<List<Season>>(true, "Listed", _seasonDal.GetAll());
        }

        public IDataResult<Season> GetById(int seasonId)
        {
            return new SuccessDataResult<Season>(true, "Listed", _seasonDal.Get(p => p.Id == seasonId));
        }

        [SecuredOperation("admin,season.add")]
        [ValidationAspect(typeof(SeasonValidator))]
        [TransactionScopeAspect]
        public IResult Add(Season season)
        {
            IResult result = BusinessRules.Run(CheckIfDescriptionExists(season), CheckIfCodeExists(season));

            if (result != null)
                return result;

            _seasonDal.Add(season);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,season.updated")]
        [ValidationAspect(typeof(SeasonValidator))]
        [TransactionScopeAspect]
        public IResult Update(Season season)
        {
            IResult result = BusinessRules.Run(CheckIfDescriptionExists(season),CheckIfCodeExists(season));

            if (result != null)
                return result;

            _seasonDal.Add(season);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,season.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Season season)
        {
            _seasonDal.Delete(season);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Season season)
        {
            var result = _seasonDal.GetAll(x => x.Description == season.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Season season)
        {
            var result = _seasonDal.GetAll(x => x.Code == season.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }

    }
}
