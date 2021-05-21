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
    public class CsNoDeliveryDateHistoryManager : ICsNoDeliveryDateHistoryService
    {
        private ICsNoDeliveryDateHistoryDal _csNoDeliveryDateHistoryDal;

        public CsNoDeliveryDateHistoryManager(ICsNoDeliveryDateHistoryDal csNoDeliveryDateHistoryDal)
        {
            _csNoDeliveryDateHistoryDal = csNoDeliveryDateHistoryDal;
        }

        public IDataServiceResult<List<CsNoDeliveryDateHistory>> GetAll(int customerId)
        {
            var dbResult = _csNoDeliveryDateHistoryDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<CsNoDeliveryDateHistory>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CsNoDeliveryDateHistory>> GetAllByCsNoDeliveryDate(int csNoDeliveryDateId)
        {
            var dbResult = _csNoDeliveryDateHistoryDal.GetAll(x => x.CsNoDeliveryDateId == csNoDeliveryDateId);

            return new SuccessDataServiceResult<List<CsNoDeliveryDateHistory>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CsNoDeliveryDateHistory> GetById(int csNoDeliveryDateHistoryId)
        {
            var dbResult = _csNoDeliveryDateHistoryDal.Get(p => p.Id == csNoDeliveryDateHistoryId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CsNoDeliveryDateHistory>(false, "SystemError");

            return new SuccessDataServiceResult<CsNoDeliveryDateHistory>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CsNoDeliveryDateHistory> GetByCsNoDeliveryDateId(int csNoDeliveryDateId)
        {
            var dbResult = _csNoDeliveryDateHistoryDal.Get(p => p.CsNoDeliveryDateId == csNoDeliveryDateId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CsNoDeliveryDateHistory>(false, "SystemError");

            return new SuccessDataServiceResult<CsNoDeliveryDateHistory>(dbResult, true, "Listed");
        }
        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(CsNoDeliveryDateHistoryValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(CsNoDeliveryDateHistory csNoDeliveryDateHistory)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<CsNoDeliveryDateHistory>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var dbResult = _csNoDeliveryDateHistoryDal.Add(csNoDeliveryDateHistory);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(CsNoDeliveryDateHistory csNoDeliveryDateHistory)
        {

            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<CsNoDeliveryDateHistory>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _csNoDeliveryDateHistoryDal.Delete(csNoDeliveryDateHistory);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteByCsNoDeliveryDateId(int csNoDeliveryDateId)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<CsNoDeliveryDateHistory>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var list = GetAllByCsNoDeliveryDate(csNoDeliveryDateId);
            if (list.Result == true)
            {
                foreach (var item in list.Data)
                {
                    var result = _csNoDeliveryDateHistoryDal.Delete(item);
                    if (result == false)
                        return new ErrorServiceResult(false, "SystemError");
                }
            }

            return new ServiceResult(true, "Delated");
        }
    }
}
