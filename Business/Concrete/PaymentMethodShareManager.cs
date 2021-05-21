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
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;

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

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(PaymentMethodShare paymentMethodShare)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<PaymentMethodShare>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _paymentMethodShareDal.Delete(paymentMethodShare);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteBySeason(Season season)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<PaymentMethodShare>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

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

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(PaymentMethodShareValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<PaymentMethodShare> Save(int seasonId, int customerId, List<PaymentMethodShare> paymentMethodShares)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<PaymentMethodShare>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var dbPaymentMethodShares = GetAllBySeasonId(seasonId).Data;
            foreach (var dbPaymentMethodShare in dbPaymentMethodShares)
            {
                var control = paymentMethodShares.Any(x => x.Id == dbPaymentMethodShare.Id);
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
