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
    public class SeasonPlaningManager : ISeasonPlaningService
    {
        private ISeasonPlaningDal _seasonPlaningDal;

        public SeasonPlaningManager(ISeasonPlaningDal seasonPlaningDal)
        {
            _seasonPlaningDal = seasonPlaningDal;
        }

        public IDataResult<List<SeasonPlaning>> GetAll()
        {
            return new SuccessDataResult<List<SeasonPlaning>>(true, "Listed", _seasonPlaningDal.GetAll());
        }

        public IDataResult<SeasonPlaning> GetById(int seasonPlaningId)
        {
            return new SuccessDataResult<SeasonPlaning>(true, "Listed", _seasonPlaningDal.Get(p => p.Id == seasonPlaningId));
        }

        [SecuredOperation("admin,season.add")]
        [ValidationAspect(typeof(SeasonPlaningValidator))]
        [TransactionScopeAspect]
        public IResult Add(SeasonPlaning seasonPlaning)
        {
            IResult result = BusinessRules.Run(CheckIfPlaningTypeExists(seasonPlaning));

            if (result != null)
                return result;

            _seasonPlaningDal.Add(seasonPlaning);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,season.updated")]
        [ValidationAspect(typeof(SeasonPlaningValidator))]
        [TransactionScopeAspect]
        public IResult Update(SeasonPlaning seasonPlaning)
        {
            IResult result = BusinessRules.Run(CheckIfPlaningTypeExists(seasonPlaning));

            if (result != null)
                return result;

            _seasonPlaningDal.Add(seasonPlaning);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,season.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(SeasonPlaning seasonPlaning)
        {
            _seasonPlaningDal.Delete(seasonPlaning);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfPlaningTypeExists(SeasonPlaning seasonPlaning)
        {
            var result = _seasonPlaningDal.GetAll(x => x.SeasonId == seasonPlaning.SeasonId && x.ProductGroupId == seasonPlaning.ProductGroupId).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
       
    }
}
