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
    public class LinerManager : ILinerService
    {
        private ILinerDal _linerDal;

        public LinerManager(ILinerDal linerDal)
        {
            _linerDal = linerDal;
        }

        public IDataResult<List<Liner>> GetAll()
        {
            return new SuccessDataResult<List<Liner>>(true, "Listed", _linerDal.GetAll());
        }

        public IDataResult<Liner> GetById(int linerId)
        {
            return new SuccessDataResult<Liner>(true, "Listed", _linerDal.Get(p => p.Id == linerId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(LinerValidator))]
        [TransactionScopeAspect]
        public IResult Add(Liner liner)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(liner), CheckIfDescriptionExists(liner));

            if (result != null)
                return result;

            _linerDal.Add(liner);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(LinerValidator))]
        [TransactionScopeAspect]
        public IResult Update(Liner liner)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(liner), CheckIfDescriptionExists(liner));

            if (result != null)
                return result;

            _linerDal.Add(liner);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Liner liner)
        {
            _linerDal.Delete(liner);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Liner liner)
        {
            var result = _linerDal.GetAll(x => x.Description == liner.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Liner liner)
        {
            var result = _linerDal.GetAll(x => x.Code == liner.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
