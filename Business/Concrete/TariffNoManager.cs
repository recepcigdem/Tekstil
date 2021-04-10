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
    public class TariffNoManager : ITariffNoService
    {
        private ITariffNoDal _tariffNoDal;

        public TariffNoManager(ITariffNoDal tariffNoDal)
        {
            _tariffNoDal = tariffNoDal;
        }

        public IDataResult<List<TariffNo>> GetAll()
        {
            return new SuccessDataResult<List<TariffNo>>(true, "Listed", _tariffNoDal.GetAll());
        }

        public IDataResult<TariffNo> GetById(int tariffNoId)
        {
            return new SuccessDataResult<TariffNo>(true, "Listed", _tariffNoDal.Get(p => p.Id == tariffNoId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(TariffNoValidator))]
        [TransactionScopeAspect]
        public IResult Add(TariffNo tariffNo)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(tariffNo), CheckIfDescriptionExists(tariffNo));

            if (result != null)
                return result;

            _tariffNoDal.Add(tariffNo);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(TariffNoValidator))]
        [TransactionScopeAspect]
        public IResult Update(TariffNo tariffNo)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(tariffNo), CheckIfDescriptionExists(tariffNo));

            if (result != null)
                return result;

            _tariffNoDal.Add(tariffNo);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(TariffNo tariffNo)
        {
            _tariffNoDal.Delete(tariffNo);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(TariffNo tariffNo)
        {
            var result = _tariffNoDal.GetAll(x => x.Description == tariffNo.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(TariffNo tariffNo)
        {
            var result = _tariffNoDal.GetAll(x => x.Code == tariffNo.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
