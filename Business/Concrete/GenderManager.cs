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
    public class GenderManager : IGenderService
    {
        private IGenderDal _genderDal;

        public GenderManager(IGenderDal genderDal)
        {
            _genderDal = genderDal;
        }

        public IDataResult<List<Gender>> GetAll()
        {
            return new SuccessDataResult<List<Gender>>(true, "Listed", _genderDal.GetAll());
        }

        public IDataResult<Gender> GetById(int genderId)
        {
            return new SuccessDataResult<Gender>(true, "Listed", _genderDal.Get(p => p.Id == genderId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(GenderValidator))]
        [TransactionScopeAspect]
        public IResult Add(Gender gender)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(gender), CheckIfDescriptionExists(gender),CheckIfShortDescriptionExists(gender));

            if (result != null)
                return result;

            _genderDal.Add(gender);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(GenderValidator))]
        [TransactionScopeAspect]
        public IResult Update(Gender gender)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(gender), CheckIfDescriptionExists(gender),CheckIfShortDescriptionExists(gender));

            if (result != null)
                return result;

            _genderDal.Add(gender);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Gender gender)
        {
            _genderDal.Delete(gender);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfShortDescriptionExists(Gender gender)
        {
            var result = _genderDal.GetAll(x => x.ShortDescription == gender.ShortDescription).Any();

            if (result)
                new ErrorResult("ShortDescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfDescriptionExists(Gender gender)
        {
            var result = _genderDal.GetAll(x => x.Description == gender.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Gender gender)
        {
            var result = _genderDal.GetAll(x => x.CardCode == gender.CardCode).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
