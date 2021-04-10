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
    public class CsNoDeliveryDateHistoryManager : ICsNoDeliveryDateHistoryService
    {
        private ICsNoDeliveryDateHistoryDal _csNoDeliveryDateHistoryDal;

        public CsNoDeliveryDateHistoryManager(ICsNoDeliveryDateHistoryDal csNoDeliveryDateHistoryDal)
        {
            _csNoDeliveryDateHistoryDal = csNoDeliveryDateHistoryDal;
        }

        public IDataResult<List<CsNoDeliveryDateHistory>> GetAll()
        {
            return new SuccessDataResult<List<CsNoDeliveryDateHistory>>(true, "Listed", _csNoDeliveryDateHistoryDal.GetAll());
        }

        public IDataResult<CsNoDeliveryDateHistory> GetByCsNoDeliveryDateId(int csNoDeliveryDateId)
        {
            return new SuccessDataResult<CsNoDeliveryDateHistory>(true, "Listed", _csNoDeliveryDateHistoryDal.Get(p => p.CsNoDeliveryDateId == csNoDeliveryDateId));
        }

        [SecuredOperation("admin,csno.add")]
        [ValidationAspect(typeof(CsNoDeliveryDateHistoryValidator))]
        [TransactionScopeAspect]
        public IResult Add(CsNoDeliveryDateHistory csNoDeliveryDateHistory)
        {
            _csNoDeliveryDateHistoryDal.Add(csNoDeliveryDateHistory);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,csno.updated")]
        [ValidationAspect(typeof(CsNoDeliveryDateHistoryValidator))]
        [TransactionScopeAspect]
        public IResult Update(CsNoDeliveryDateHistory csNoDeliveryDateHistory)
        {
            _csNoDeliveryDateHistoryDal.Add(csNoDeliveryDateHistory);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,csno.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(CsNoDeliveryDateHistory csNoDeliveryDateHistory)
        {
            _csNoDeliveryDateHistoryDal.Delete(csNoDeliveryDateHistory);

            return new SuccessResult("Deleted");
        }

    }
}
