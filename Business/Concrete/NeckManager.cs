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
    public class NeckManager : INeckService
    {
        private INeckDal _neckDal;

        public NeckManager(INeckDal neckDal)
        {
            _neckDal = neckDal;
        }

        public IDataResult<List<Neck>> GetAll()
        {
            return new SuccessDataResult<List<Neck>>(true, "Listed", _neckDal.GetAll());
        }

        public IDataResult<Neck> GetById(int neckId)
        {
            return new SuccessDataResult<Neck>(true, "Listed", _neckDal.Get(p => p.Id == neckId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(NeckValidator))]
        [TransactionScopeAspect]
        public IResult Add(Neck neck)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(neck), CheckIfDescriptionExists(neck));

            if (result != null)
                return result;

            _neckDal.Add(neck);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(NeckValidator))]
        [TransactionScopeAspect]
        public IResult Update(Neck neck)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(neck), CheckIfDescriptionExists(neck));

            if (result != null)
                return result;

            _neckDal.Add(neck);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Neck neck)
        {
            _neckDal.Delete(neck);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Neck neck)
        {
            var result = _neckDal.GetAll(x => x.Description == neck.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Neck neck)
        {
            var result = _neckDal.GetAll(x => x.Code == neck.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
