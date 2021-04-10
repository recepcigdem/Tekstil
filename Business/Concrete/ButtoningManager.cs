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
    public class ButtoningManager : IButtoningService
    {
        private IButtoningDal _buttoningDal;

        public ButtoningManager(IButtoningDal buttoningDal)
        {
            _buttoningDal = buttoningDal;
        }

        public IDataResult<List<Buttoning>> GetAll()
        {
            return new SuccessDataResult<List<Buttoning>>(true, "Listed", _buttoningDal.GetAll());
        }

        public IDataResult<Buttoning> GetById(int buttoningId)
        {
            return new SuccessDataResult<Buttoning>(true, "Listed", _buttoningDal.Get(p => p.Id == buttoningId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ButtoningValidator))]
        [TransactionScopeAspect]
        public IResult Add(Buttoning buttoning)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(buttoning), CheckIfDescriptionExists(buttoning));

            if (result != null)
                return result;

            _buttoningDal.Add(buttoning);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ButtoningValidator))]
        [TransactionScopeAspect]
        public IResult Update(Buttoning buttoning)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(buttoning), CheckIfDescriptionExists(buttoning));

            if (result != null)
                return result;

            _buttoningDal.Add(buttoning);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Buttoning buttoning)
        {
            _buttoningDal.Delete(buttoning);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Buttoning buttoning)
        {
            var result = _buttoningDal.GetAll(x => x.Description == buttoning.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Buttoning buttoning)
        {
            var result = _buttoningDal.GetAll(x => x.Code == buttoning.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
