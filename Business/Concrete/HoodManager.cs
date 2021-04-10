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
    public class HoodManager : IHoodService
    {
        private IHoodDal _hoodDal;

        public HoodManager(IHoodDal hoodDal)
        {
            _hoodDal = hoodDal;
        }

        public IDataResult<List<Hood>> GetAll()
        {
            return new SuccessDataResult<List<Hood>>(true, "Listed", _hoodDal.GetAll());
        }

        public IDataResult<Hood> GetById(int hoodId)
        {
            return new SuccessDataResult<Hood>(true, "Listed", _hoodDal.Get(p => p.Id == hoodId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(HoodValidator))]
        [TransactionScopeAspect]
        public IResult Add(Hood hood)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(hood), CheckIfDescriptionExists(hood));

            if (result != null)
                return result;

            _hoodDal.Add(hood);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(HoodValidator))]
        [TransactionScopeAspect]
        public IResult Update(Hood hood)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(hood), CheckIfDescriptionExists(hood));

            if (result != null)
                return result;

            _hoodDal.Add(hood);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Hood hood)
        {
            _hoodDal.Delete(hood);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Hood hood)
        {
            var result = _hoodDal.GetAll(x => x.Description == hood.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Hood hood)
        {
            var result = _hoodDal.GetAll(x => x.Code == hood.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
