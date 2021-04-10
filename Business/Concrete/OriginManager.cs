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
    public class OriginManager : IOriginService
    {
        private IOriginDal _originDal;

        public OriginManager(IOriginDal originDal)
        {
            _originDal = originDal;
        }

        public IDataResult<List<Origin>> GetAll()
        {
            return new SuccessDataResult<List<Origin>>(true, "Listed", _originDal.GetAll());
        }

        public IDataResult<Origin> GetById(int originId)
        {
            return new SuccessDataResult<Origin>(true, "Listed", _originDal.Get(p => p.Id == originId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(OriginValidator))]
        [TransactionScopeAspect]
        public IResult Add(Origin origin)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(origin), CheckIfDescriptionExists(origin));

            if (result != null)
                return result;

            _originDal.Add(origin);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(OriginValidator))]
        [TransactionScopeAspect]
        public IResult Update(Origin origin)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(origin), CheckIfDescriptionExists(origin));

            if (result != null)
                return result;

            _originDal.Add(origin);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Origin origin)
        {
            _originDal.Delete(origin);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Origin origin)
        {
            var result = _originDal.GetAll(x => x.Description == origin.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Origin origin)
        {
            var result = _originDal.GetAll(x => x.Code == origin.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
