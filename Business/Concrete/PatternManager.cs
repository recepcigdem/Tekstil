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
    public class PatternManager : IPatternService
    {
        private IPatternDal _patternDal;

        public PatternManager(IPatternDal patternDal)
        {
            _patternDal = patternDal;
        }

        public IDataResult<List<Pattern>> GetAll()
        {
            return new SuccessDataResult<List<Pattern>>(true, "Listed", _patternDal.GetAll());
        }

        public IDataResult<Pattern> GetById(int patternId)
        {
            return new SuccessDataResult<Pattern>(true, "Listed", _patternDal.Get(p => p.Id == patternId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(PatternValidator))]
        [TransactionScopeAspect]
        public IResult Add(Pattern pattern)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(pattern), CheckIfDescriptionExists(pattern));

            if (result != null)
                return result;

            _patternDal.Add(pattern);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(PatternValidator))]
        [TransactionScopeAspect]
        public IResult Update(Pattern pattern)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(pattern), CheckIfDescriptionExists(pattern));

            if (result != null)
                return result;

            _patternDal.Add(pattern);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Pattern pattern)
        {
            _patternDal.Delete(pattern);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Pattern pattern)
        {
            var result = _patternDal.GetAll(x => x.Description == pattern.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Pattern pattern)
        {
            var result = _patternDal.GetAll(x => x.Code == pattern.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
