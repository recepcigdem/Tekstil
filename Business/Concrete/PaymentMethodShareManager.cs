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


        public IDataServiceResult<List<PaymentMethodShare>> GetAll(int customerId)
        {
            var dbResult = _paymentMethodShareDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<PaymentMethodShare>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<PaymentMethodShare>> GetAllBySeasonId(int seasonId)
        {
            var dbResult = _paymentMethodShareDal.GetAll(x => x.SeasonId == seasonId);

            return new SuccessDataServiceResult<List<PaymentMethodShare>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<PaymentMethodShare> GetById(int paymentMethodShareId)
        {
            var dbResult = _paymentMethodShareDal.Get(p => p.Id == paymentMethodShareId);
            if (dbResult == null)
                return new SuccessDataServiceResult<PaymentMethodShare>(false, "SystemError");

            return new SuccessDataServiceResult<PaymentMethodShare>(dbResult, true, "Listed");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(PaymentMethodShareValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(PaymentMethodShare paymentMethodShare)
        {
            ServiceResult result = BusinessRules.Run(CheckIfPaymentMethodExists(paymentMethodShare));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _paymentMethodShareDal.Add(paymentMethodShare);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [ValidationAspect(typeof(PaymentMethodShareValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(PaymentMethodShare paymentMethodShare)
        {
            ServiceResult result = BusinessRules.Run(CheckIfPaymentMethodExists(paymentMethodShare));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _paymentMethodShareDal.Update(paymentMethodShare);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }
        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult Delete(PaymentMethodShare paymentMethodShare)
        {
            var result = _paymentMethodShareDal.Delete(paymentMethodShare);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        public IServiceResult DeleteBySeason(Season season)
        {
            var paymentMethodShares = GetAllBySeasonId(season.Id);
            if (paymentMethodShares.Result == false)
                return new ErrorServiceResult(false, "PaymentMethodShareNotFound");

            foreach (var paymentMethodShare in paymentMethodShares.Data)
            {
                var deletePaymentMethodShare = Delete(paymentMethodShare);
                if (deletePaymentMethodShare.Result == false)
                    return new ErrorServiceResult(false, "PaymentMethodShareNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<PaymentMethodShare> Save(int customerId, List<PaymentMethodShare> paymentMethodShares)
        {
            var dbPaymentMethodShares = GetAll(customerId).Data;
            foreach (var dbPaymentMethodShare in dbPaymentMethodShares)
            {
                var control = dbPaymentMethodShares.Any(x => x.Id == dbPaymentMethodShare.Id);
                if (control != true)
                {
                    Delete(dbPaymentMethodShare);
                }
            }

            foreach (var paymentMethodShare in paymentMethodShares)
            {
                paymentMethodShare.CustomerId = customerId;
                if (paymentMethodShare.Id > 0)
                {
                    Update(paymentMethodShare);
                }
                else
                {
                    Add(paymentMethodShare);
                }
            }
            return new SuccessDataServiceResult<PaymentMethodShare>(true, "Saved");
        }

        private ServiceResult CheckIfPaymentMethodExists(PaymentMethodShare paymentMethodShare)
        {
            var result = _paymentMethodShareDal.GetAll(x => x.SeasonId == paymentMethodShare.SeasonId && x.PaymentMethodId == paymentMethodShare.PaymentMethodId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "PaymentMethodAlreadyExists");

            return new ServiceResult(true,"");
        }
    }
}
