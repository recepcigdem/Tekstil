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
    public class CsNoDeliveryDateManager : ICsNoDeliveryDateService
    {
        private ICsNoDeliveryDateDal _csNoDeliveryDateDal;

        public CsNoDeliveryDateManager(ICsNoDeliveryDateDal csNoDeliveryDateDal)
        {
            _csNoDeliveryDateDal = csNoDeliveryDateDal;
        }

        public IDataResult<List<CsNoDeliveryDate>> GetAll()
        {
            return new SuccessDataResult<List<CsNoDeliveryDate>>(true, "Listed", _csNoDeliveryDateDal.GetAll());
        }

        public IDataResult<CsNoDeliveryDate> GetById(int csNoDeliveryDateId)
        {
            return new SuccessDataResult<CsNoDeliveryDate>(true, "Listed", _csNoDeliveryDateDal.Get(p => p.Id == csNoDeliveryDateId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(CsNoDeliveryDateValidator))]
        [TransactionScopeAspect]
        public IResult Add(CsNoDeliveryDate csNoDeliveryDate)
        {
            IResult result = BusinessRules.Run(CheckIfCsNoExists(csNoDeliveryDate));

            if (result != null)
                return result;

            _csNoDeliveryDateDal.Add(csNoDeliveryDate);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(CsNoDeliveryDateValidator))]
        [TransactionScopeAspect]
        public IResult Update(CsNoDeliveryDate csNoDeliveryDate)
        {
            IResult result = BusinessRules.Run(CheckIfCsNoExists(csNoDeliveryDate));

            if (result != null)
                return result;

            _csNoDeliveryDateDal.Add(csNoDeliveryDate);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(CsNoDeliveryDate csNoDeliveryDate)
        {
            _csNoDeliveryDateDal.Delete(csNoDeliveryDate);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfCsNoExists(CsNoDeliveryDate csNoDeliveryDate)
        {
            var result = _csNoDeliveryDateDal.GetAll(x => x.SeasonId == csNoDeliveryDate.SeasonId && x.Csno==csNoDeliveryDate.Csno).Any();

            if (result)
                new ErrorResult("CsNoAlreadyExists");

            return new SuccessResult();
        }
        
    }
}
