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
    public class BeltManager : IBeltService
    {
        private IBeltDal _beltDal;

        public BeltManager(IBeltDal beltDal)
        {
            _beltDal = beltDal;
        }

        public IDataResult<List<Belt>> GetAll()
        {
            return new SuccessDataResult<List<Belt>>(true, "Listed", _beltDal.GetAll());
        }

        public IDataResult<Belt> GetById(int beltId)
        {
            return new SuccessDataResult<Belt>(true, "Listed", _beltDal.Get(p => p.Id == beltId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(BeltValidator))]
        [TransactionScopeAspect]
        public IResult Add(Belt belt)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(belt), CheckIfDescriptionExists(belt));

            if (result != null)
            {
                return result;

            }
            _beltDal.Add(belt);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(BeltValidator))]
        [TransactionScopeAspect]
        public IResult Update(Belt belt)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(belt), CheckIfDescriptionExists(belt));

            if (result != null)
            {
                return result;

            }
            _beltDal.Add(belt);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Belt belt)
        {
            _beltDal.Delete(belt);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Belt belt)
        {
            var result = _beltDal.GetAll(x => x.Description == belt.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Belt belt)
        {
            var result = _beltDal.GetAll(x => x.Code == belt.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
