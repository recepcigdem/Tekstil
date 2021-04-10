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
    public class SubGroupManager : ISubGroupService
    {
        private ISubGroupDal _subGroupDal;

        public SubGroupManager(ISubGroupDal subGroupDal)
        {
            _subGroupDal = subGroupDal;
        }

        public IDataResult<List<SubGroup>> GetAll()
        {
            return new SuccessDataResult<List<SubGroup>>(true, "Listed", _subGroupDal.GetAll());
        }

        public IDataResult<SubGroup> GetById(int subGroupId)
        {
            return new SuccessDataResult<SubGroup>(true, "Listed", _subGroupDal.Get(p => p.Id == subGroupId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(SubGroupValidator))]
        [TransactionScopeAspect]
        public IResult Add(SubGroup subGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(subGroup), CheckIfDescriptionExists(subGroup));

            if (result != null)
                return result;

            _subGroupDal.Add(subGroup);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(SubGroupValidator))]
        [TransactionScopeAspect]
        public IResult Update(SubGroup subGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(subGroup), CheckIfDescriptionExists(subGroup));

            if (result != null)
                return result;

            _subGroupDal.Add(subGroup);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(SubGroup subGroup)
        {
            _subGroupDal.Delete(subGroup);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(SubGroup subGroup)
        {
            var result = _subGroupDal.GetAll(x => x.Description == subGroup.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(SubGroup subGroup)
        {
            var result = _subGroupDal.GetAll(x => x.Code == subGroup.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
