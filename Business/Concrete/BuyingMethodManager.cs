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
    public class BuyingMethodManager : IBuyingMethodService
    {
        private IBuyingMethodDal _buyingMethodDal;

        public BuyingMethodManager(IBuyingMethodDal buyingMethodDal)
        {
            _buyingMethodDal = buyingMethodDal;
        }

        public IDataResult<List<BuyingMethod>> GetAll()
        {
            return new SuccessDataResult<List<BuyingMethod>>(true, "Listed", _buyingMethodDal.GetAll());
        }

        public IDataResult<BuyingMethod> GetById(int buyingMethodId)
        {
            return new SuccessDataResult<BuyingMethod>(true, "Listed", _buyingMethodDal.Get(p => p.Id == buyingMethodId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(BuyingMethodValidator))]
        [TransactionScopeAspect]
        public IResult Add(BuyingMethod buyingMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(buyingMethod), CheckIfDescriptionExists(buyingMethod));

            if (result != null)
                return result;

            _buyingMethodDal.Add(buyingMethod);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(BuyingMethodValidator))]
        [TransactionScopeAspect]
        public IResult Update(BuyingMethod buyingMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(buyingMethod), CheckIfDescriptionExists(buyingMethod));

            if (result != null)
                return result;

            _buyingMethodDal.Add(buyingMethod);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(BuyingMethod buyingMethod)
        {
            _buyingMethodDal.Delete(buyingMethod);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(BuyingMethod buyingMethod)
        {
            var result = _buyingMethodDal.GetAll(x => x.Description == buyingMethod.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(BuyingMethod buyingMethod)
        {
            var result = _buyingMethodDal.GetAll(x => x.Code == buyingMethod.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
