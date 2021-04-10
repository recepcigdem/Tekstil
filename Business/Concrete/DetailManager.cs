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
    public class DetailManager : IDetailService
    {
        private IDetailDal _detailDal;

        public DetailManager(IDetailDal detailDal)
        {
            _detailDal = detailDal;
        }

        public IDataResult<List<Detail>> GetAll()
        {
            return new SuccessDataResult<List<Detail>>(true, "Listed", _detailDal.GetAll());
        }

        public IDataResult<Detail> GetById(int detailId)
        {
            return new SuccessDataResult<Detail>(true, "Listed", _detailDal.Get(p => p.Id == detailId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(DetailValidator))]
        [TransactionScopeAspect]
        public IResult Add(Detail detail)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(detail), CheckIfDescriptionExists(detail));

            if (result != null)
                return result;

            _detailDal.Add(detail);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(DetailValidator))]
        [TransactionScopeAspect]
        public IResult Update(Detail detail)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(detail), CheckIfDescriptionExists(detail));

            if (result != null)
                return result;

            _detailDal.Add(detail);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Detail detail)
        {
            _detailDal.Delete(detail);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Detail detail)
        {
            var result = _detailDal.GetAll(x => x.Description == detail.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Detail detail)
        {
            var result = _detailDal.GetAll(x => x.Code == detail.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
