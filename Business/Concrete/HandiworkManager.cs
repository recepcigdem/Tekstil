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
    public class HandiworkManager : IHandiworkService
    {
        private IHandiworkDal _handiworkDal;

        public HandiworkManager(IHandiworkDal handiworkDal)
        {
            _handiworkDal = handiworkDal;
        }

        public IDataResult<List<Handiwork>> GetAll()
        {
            return new SuccessDataResult<List<Handiwork>>(true, "Listed", _handiworkDal.GetAll());
        }

        public IDataResult<Handiwork> GetById(int handiworkId)
        {
            return new SuccessDataResult<Handiwork>(true, "Listed", _handiworkDal.Get(p => p.Id == handiworkId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(HandiworkValidator))]
        [TransactionScopeAspect]
        public IResult Add(Handiwork handiwork)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(handiwork), CheckIfDescriptionExists(handiwork));

            if (result != null)
                return result;

            _handiworkDal.Add(handiwork);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(HandiworkValidator))]
        [TransactionScopeAspect]
        public IResult Update(Handiwork handiwork)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(handiwork), CheckIfDescriptionExists(handiwork));

            if (result != null)
                return result;

            _handiworkDal.Add(handiwork);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Handiwork handiwork)
        {
            _handiworkDal.Delete(handiwork);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Handiwork handiwork)
        {
            var result = _handiworkDal.GetAll(x => x.Description == handiwork.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Handiwork handiwork)
        {
            var result = _handiworkDal.GetAll(x => x.Code == handiwork.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
