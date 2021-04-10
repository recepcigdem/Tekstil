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
    public class SubDetailGroupManager : ISubDetailGroupService
    {
        private ISubDetailGroupDal _subDetailGroupDal;

        public SubDetailGroupManager(ISubDetailGroupDal subDetailGroupDal)
        {
            _subDetailGroupDal = subDetailGroupDal;
        }

        public IDataResult<List<SubDetailGroup>> GetAll()
        {
            return new SuccessDataResult<List<SubDetailGroup>>(true, "Listed", _subDetailGroupDal.GetAll());
        }

        public IDataResult<SubDetailGroup> GetById(int subDetailGroupId)
        {
            return new SuccessDataResult<SubDetailGroup>(true, "Listed", _subDetailGroupDal.Get(p => p.Id == subDetailGroupId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(SubDetailGroupValidator))]
        [TransactionScopeAspect]
        public IResult Add(SubDetailGroup subDetailGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(subDetailGroup), CheckIfDescriptionExists(subDetailGroup));

            if (result != null)
                return result;

            _subDetailGroupDal.Add(subDetailGroup);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(SubDetailGroupValidator))]
        [TransactionScopeAspect]
        public IResult Update(SubDetailGroup subDetailGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(subDetailGroup), CheckIfDescriptionExists(subDetailGroup));

            if (result != null)
                return result;

            _subDetailGroupDal.Add(subDetailGroup);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(SubDetailGroup subDetailGroup)
        {
            _subDetailGroupDal.Delete(subDetailGroup);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(SubDetailGroup subDetailGroup)
        {
            var result = _subDetailGroupDal.GetAll(x => x.Description == subDetailGroup.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(SubDetailGroup subDetailGroup)
        {
            var result = _subDetailGroupDal.GetAll(x => x.Code == subDetailGroup.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
