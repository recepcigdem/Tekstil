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
    public class SizeManager : ISizeService
    {
        private ISizeDal _sizeDal;

        public SizeManager(ISizeDal sizeDal)
        {
            _sizeDal = sizeDal;
        }

        public IDataResult<List<Size>> GetAll()
        {
            return new SuccessDataResult<List<Size>>(true, "Listed", _sizeDal.GetAll());
        }

        public IDataResult<Size> GetById(int sizeId)
        {
            return new SuccessDataResult<Size>(true, "Listed", _sizeDal.Get(p => p.Id == sizeId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(SizeValidator))]
        [TransactionScopeAspect]
        public IResult Add(Size size)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(size), CheckIfDescriptionExists(size));

            if (result != null)
                return result;

            _sizeDal.Add(size);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(SizeValidator))]
        [TransactionScopeAspect]
        public IResult Update(Size size)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(size), CheckIfDescriptionExists(size));

            if (result != null)
                return result;

            _sizeDal.Add(size);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Size size)
        {
            _sizeDal.Delete(size);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Size size)
        {
            var result = _sizeDal.GetAll(x => x.Description == size.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Size size)
        {
            var result = _sizeDal.GetAll(x => x.Code == size.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
