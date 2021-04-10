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
    public class WaistManager : IWaistService
    {
        private IWaistDal _waistDal;

        public WaistManager(IWaistDal waistDal)
        {
            _waistDal = waistDal;
        }

        public IDataResult<List<Waist>> GetAll()
        {
            return new SuccessDataResult<List<Waist>>(true, "Listed", _waistDal.GetAll());
        }

        public IDataResult<Waist> GetById(int waistId)
        {
            return new SuccessDataResult<Waist>(true, "Listed", _waistDal.Get(p => p.Id == waistId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(WaistValidator))]
        [TransactionScopeAspect]
        public IResult Add(Waist waist)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(waist), CheckIfDescriptionExists(waist));

            if (result != null)
                return result;

            _waistDal.Add(waist);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(WaistValidator))]
        [TransactionScopeAspect]
        public IResult Update(Waist waist)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(waist), CheckIfDescriptionExists(waist));

            if (result != null)
                return result;

            _waistDal.Add(waist);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Waist waist)
        {
            _waistDal.Delete(waist);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Waist waist)
        {
            var result = _waistDal.GetAll(x => x.Description == waist.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Waist waist)
        {
            var result = _waistDal.GetAll(x => x.Code == waist.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
