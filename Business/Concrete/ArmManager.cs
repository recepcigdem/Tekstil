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
    public class ArmManager : IArmService
    {
        private IArmDal _armDal;

        public ArmManager(IArmDal armDal)
        {
            _armDal = armDal;
        }

        public IDataResult<List<Arm>> GetAll()
        {
            return new SuccessDataResult<List<Arm>>(true, "Listed", _armDal.GetAll());
        }

        public IDataResult<Arm> GetById(int armId)
        {
            return new SuccessDataResult<Arm>(true, "Listed", _armDal.Get(p => p.Id == armId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ArmValidator))]
        [TransactionScopeAspect]
        public IResult Add(Arm arm)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(arm), CheckIfDescriptionExists(arm));

            if (result != null)
                return result;

            _armDal.Add(arm);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ArmValidator))]
        [TransactionScopeAspect]
        public IResult Update(Arm arm)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(arm), CheckIfDescriptionExists(arm));

            if (result != null)
                return result;

            _armDal.Add(arm);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Arm arm)
        {
            _armDal.Delete(arm);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Arm arm)
        {
            var result = _armDal.GetAll(x => x.Description == arm.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Arm arm)
        {
            var result = _armDal.GetAll(x => x.Code == arm.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
