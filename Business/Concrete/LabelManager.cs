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
    public class LabelManager : ILabelService
    {
        private ILabelDal _labelDal;

        public LabelManager(ILabelDal labelDal)
        {
            _labelDal = labelDal;
        }

        public IDataResult<List<Label>> GetAll()
        {
            return new SuccessDataResult<List<Label>>(true, "Listed", _labelDal.GetAll());
        }

        public IDataResult<Label> GetById(int labelId)
        {
            return new SuccessDataResult<Label>(true, "Listed", _labelDal.Get(p => p.Id == labelId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(LabelValidator))]
        [TransactionScopeAspect]
        public IResult Add(Label label)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(label), CheckIfDescriptionExists(label));

            if (result != null)
                return result;

            _labelDal.Add(label);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(LabelValidator))]
        [TransactionScopeAspect]
        public IResult Update(Label label)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(label), CheckIfDescriptionExists(label));

            if (result != null)
                return result;

            _labelDal.Add(label);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Label label)
        {
            _labelDal.Delete(label);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Label label)
        {
            var result = _labelDal.GetAll(x => x.Description == label.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Label label)
        {
            var result = _labelDal.GetAll(x => x.Code == label.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
