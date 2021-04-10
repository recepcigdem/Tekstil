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
    public class PaymentMethodShareManager : IPaymentMethodShareService
    {
        private IPaymentMethodShareDal _paymentMethodShareDal;

        public PaymentMethodShareManager(IPaymentMethodShareDal paymentMethodShareDal)
        {
            _paymentMethodShareDal = paymentMethodShareDal;
        }

        public IDataResult<List<PaymentMethodShare>> GetAll()
        {
            return new SuccessDataResult<List<PaymentMethodShare>>(true, "Listed", _paymentMethodShareDal.GetAll());
        }

        public IDataResult<PaymentMethodShare> GetById(int paymentMethodShareId)
        {
            return new SuccessDataResult<PaymentMethodShare>(true, "Listed", _paymentMethodShareDal.Get(p => p.Id == paymentMethodShareId));
        }

        [SecuredOperation("admin,season.add")]
        [ValidationAspect(typeof(PaymentMethodShareValidator))]
        [TransactionScopeAspect]
        public IResult Add(PaymentMethodShare paymentMethodShare)
        {
            IResult result = BusinessRules.Run(CheckIfPaymentMethodExists(paymentMethodShare));

            if (result != null)
                return result;

            _paymentMethodShareDal.Add(paymentMethodShare);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,season.updated")]
        [ValidationAspect(typeof(PaymentMethodShareValidator))]
        [TransactionScopeAspect]
        public IResult Update(PaymentMethodShare paymentMethodShare)
        {
            IResult result = BusinessRules.Run(CheckIfPaymentMethodExists(paymentMethodShare));

            if (result != null)
                return result;

            _paymentMethodShareDal.Add(paymentMethodShare);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,season.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(PaymentMethodShare paymentMethodShare)
        {
            _paymentMethodShareDal.Delete(paymentMethodShare);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfPaymentMethodExists(PaymentMethodShare paymentMethodShare)
        {
            var result = _paymentMethodShareDal.GetAll(x => x.SeasonId == paymentMethodShare.SeasonId && x.PaymentMethodId == paymentMethodShare.PaymentMethodId).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
