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
    public class FitManager : IFitService
    {
        private IFitDal _fitDal;

        public FitManager(IFitDal fitDal)
        {
            _fitDal = fitDal;
        }

        public IDataResult<List<Fit>> GetAll()
        {
            return new SuccessDataResult<List<Fit>>(true, "Listed", _fitDal.GetAll());
        }

        public IDataResult<Fit> GetById(int fitId)
        {
            return new SuccessDataResult<Fit>(true, "Listed", _fitDal.Get(p => p.Id == fitId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(FitValidator))]
        [TransactionScopeAspect]
        public IResult Add(Fit fit)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(fit), CheckIfDescriptionExists(fit));

            if (result != null)
                return result;

            _fitDal.Add(fit);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(FitValidator))]
        [TransactionScopeAspect]
        public IResult Update(Fit fit)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(fit), CheckIfDescriptionExists(fit));

            if (result != null)
                return result;

            _fitDal.Add(fit);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Fit fit)
        {
            _fitDal.Delete(fit);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Fit fit)
        {
            var result = _fitDal.GetAll(x => x.Description == fit.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Fit fit)
        {
            var result = _fitDal.GetAll(x => x.Code == fit.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
