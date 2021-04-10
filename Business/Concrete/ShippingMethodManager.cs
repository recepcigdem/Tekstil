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
    public class ShippingMethodManager : IShippingMethodService
    {
        private IShippingMethodDal _shippingMethodDal;

        public ShippingMethodManager(IShippingMethodDal shippingMethodDal)
        {
            _shippingMethodDal = shippingMethodDal;
        }

        public IDataResult<List<ShippingMethod>> GetAll()
        {
            return new SuccessDataResult<List<ShippingMethod>>(true, "Listed", _shippingMethodDal.GetAll());
        }

        public IDataResult<ShippingMethod> GetById(int shippingMethodId)
        {
            return new SuccessDataResult<ShippingMethod>(true, "Listed", _shippingMethodDal.Get(p => p.Id == shippingMethodId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ShippingMethodValidator))]
        [TransactionScopeAspect]
        public IResult Add(ShippingMethod shippingMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(shippingMethod), CheckIfDescriptionExists(shippingMethod));

            if (result != null)
                return result;

            _shippingMethodDal.Add(shippingMethod);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ShippingMethodValidator))]
        [TransactionScopeAspect]
        public IResult Update(ShippingMethod shippingMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(shippingMethod), CheckIfDescriptionExists(shippingMethod));

            if (result != null)
                return result;

            _shippingMethodDal.Add(shippingMethod);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(ShippingMethod shippingMethod)
        {
            _shippingMethodDal.Delete(shippingMethod);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(ShippingMethod shippingMethod)
        {
            var result = _shippingMethodDal.GetAll(x => x.Description == shippingMethod.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(ShippingMethod shippingMethod)
        {
            var result = _shippingMethodDal.GetAll(x => x.Code == shippingMethod.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
