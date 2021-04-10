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
    public class WidthManager : IWidthService
    {
        private IWidthDal _widthDal;

        public WidthManager(IWidthDal widthDal)
        {
            _widthDal = widthDal;
        }

        public IDataResult<List<Width>> GetAll()
        {
            return new SuccessDataResult<List<Width>>(true, "Listed", _widthDal.GetAll());
        }

        public IDataResult<Width> GetById(int widthId)
        {
            return new SuccessDataResult<Width>(true, "Listed", _widthDal.Get(p => p.Id == widthId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(WidthValidator))]
        [TransactionScopeAspect]
        public IResult Add(Width width)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(width), CheckIfDescriptionExists(width));

            if (result != null)
                return result;

            _widthDal.Add(width);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(WidthValidator))]
        [TransactionScopeAspect]
        public IResult Update(Width width)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(width), CheckIfDescriptionExists(width));

            if (result != null)
                return result;

            _widthDal.Add(width);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Width width)
        {
            _widthDal.Delete(width);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Width width)
        {
            var result = _widthDal.GetAll(x => x.Description == width.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Width width)
        {
            var result = _widthDal.GetAll(x => x.Code == width.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
