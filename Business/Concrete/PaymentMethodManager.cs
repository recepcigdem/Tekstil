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
    public class PaymentMethodManager : IPaymentMethodService
    {
        private IPaymentMethodDal _paymentMethodDal;

        public PaymentMethodManager(IPaymentMethodDal paymentMethodDal)
        {
            _paymentMethodDal = paymentMethodDal;
        }

        public IDataResult<List<PaymentMethod>> GetAll()
        {
            return new SuccessDataResult<List<PaymentMethod>>(true, "Listed", _paymentMethodDal.GetAll());
        }

        public IDataResult<PaymentMethod> GetById(int paymentMethodId)
        {
            return new SuccessDataResult<PaymentMethod>(true, "Listed", _paymentMethodDal.Get(p => p.Id == paymentMethodId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(PaymentMethodValidator))]
        [TransactionScopeAspect]
        public IResult Add(PaymentMethod paymentMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(paymentMethod), CheckIfDescriptionExists(paymentMethod));

            if (result != null)
                return result;

            _paymentMethodDal.Add(paymentMethod);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(PaymentMethodValidator))]
        [TransactionScopeAspect]
        public IResult Update(PaymentMethod paymentMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(paymentMethod), CheckIfDescriptionExists(paymentMethod));

            if (result != null)
                return result;

            _paymentMethodDal.Add(paymentMethod);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(PaymentMethod paymentMethod)
        {
            _paymentMethodDal.Delete(paymentMethod);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(PaymentMethod paymentMethod)
        {
            var result = _paymentMethodDal.GetAll(x => x.Description == paymentMethod.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(PaymentMethod paymentMethod)
        {
            var result = _paymentMethodDal.GetAll(x => x.Code == paymentMethod.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
