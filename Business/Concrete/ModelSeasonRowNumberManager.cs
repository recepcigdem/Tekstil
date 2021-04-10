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
    public class ModelSeasonRowNumberManager : IModelSeasonRowNumberService
    {
        private IModelSeasonRowNumberDal _modelSeasonRowNumberDal;

        public ModelSeasonRowNumberManager(IModelSeasonRowNumberDal modelSeasonRowNumberDal)
        {
            _modelSeasonRowNumberDal = modelSeasonRowNumberDal;
        }

        public IDataResult<List<ModelSeasonRowNumber>> GetAll()
        {
            return new SuccessDataResult<List<ModelSeasonRowNumber>>(true, "Listed", _modelSeasonRowNumberDal.GetAll());
        }

        public IDataResult<ModelSeasonRowNumber> GetById(int seasonId,int productGroupId)
        {
            return new SuccessDataResult<ModelSeasonRowNumber>(true, "Listed", _modelSeasonRowNumberDal.Get(p => p.SeasonId == seasonId && p.ProductGroupId == productGroupId));
        }

        [SecuredOperation("admin,season.add")]
        [ValidationAspect(typeof(ModelSeasonRowNumberValidator))]
        [TransactionScopeAspect]
        public IResult Add(ModelSeasonRowNumber modelSeasonRowNumber)
        {
            _modelSeasonRowNumberDal.Add(modelSeasonRowNumber);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,season.updated")]
        [ValidationAspect(typeof(ModelSeasonRowNumberValidator))]
        [TransactionScopeAspect]
        public IResult Update(ModelSeasonRowNumber modelSeasonRowNumber)
        {
            _modelSeasonRowNumberDal.Add(modelSeasonRowNumber);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,season.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(ModelSeasonRowNumber modelSeasonRowNumber)
        {
            _modelSeasonRowNumberDal.Delete(modelSeasonRowNumber);

            return new SuccessResult("Deleted");
        }
    }
}
