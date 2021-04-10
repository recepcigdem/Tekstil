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
    public class FurManager : IFurService
    {
        private IFurDal _furDal;

        public FurManager(IFurDal furDal)
        {
            _furDal = furDal;
        }

        public IDataResult<List<Fur>> GetAll()
        {
            return new SuccessDataResult<List<Fur>>(true, "Listed", _furDal.GetAll());
        }

        public IDataResult<Fur> GetById(int furId)
        {
            return new SuccessDataResult<Fur>(true, "Listed", _furDal.Get(p => p.Id == furId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(FurValidator))]
        [TransactionScopeAspect]
        public IResult Add(Fur fur)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(fur), CheckIfDescriptionExists(fur));

            if (result != null)
                return result;

            _furDal.Add(fur);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(FurValidator))]
        [TransactionScopeAspect]
        public IResult Update(Fur fur)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(fur), CheckIfDescriptionExists(fur));

            if (result != null)
                return result;

            _furDal.Add(fur);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Fur fur)
        {
            _furDal.Delete(fur);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Fur fur)
        {
            var result = _furDal.GetAll(x => x.Description == fur.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Fur fur)
        {
            var result = _furDal.GetAll(x => x.Code == fur.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
