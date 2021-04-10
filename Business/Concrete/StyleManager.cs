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
    public class StyleManager : IStyleService
    {
        private IStyleDal _styleDal;

        public StyleManager(IStyleDal styleDal)
        {
            _styleDal = styleDal;
        }

        public IDataResult<List<Style>> GetAll()
        {
            return new SuccessDataResult<List<Style>>(true, "Listed", _styleDal.GetAll());
        }

        public IDataResult<Style> GetById(int styleId)
        {
            return new SuccessDataResult<Style>(true, "Listed", _styleDal.Get(p => p.Id == styleId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(StyleValidator))]
        [TransactionScopeAspect]
        public IResult Add(Style style)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(style), CheckIfDescriptionExists(style));

            if (result != null)
                return result;

            _styleDal.Add(style);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(StyleValidator))]
        [TransactionScopeAspect]
        public IResult Update(Style style)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(style), CheckIfDescriptionExists(style));

            if (result != null)
                return result;

            _styleDal.Add(style);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Style style)
        {
            _styleDal.Delete(style);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Style style)
        {
            var result = _styleDal.GetAll(x => x.Description == style.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Style style)
        {
            var result = _styleDal.GetAll(x => x.Code == style.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
