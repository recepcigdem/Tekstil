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
    public class SubBrandManager : ISubBrandService
    {
        private ISubBrandDal _subBrandDal;

        public SubBrandManager(ISubBrandDal subBrandDal)
        {
            _subBrandDal = subBrandDal;
        }

        public IDataResult<List<SubBrand>> GetAll()
        {
            return new SuccessDataResult<List<SubBrand>>(true, "Listed", _subBrandDal.GetAll());
        }

        public IDataResult<SubBrand> GetById(int subBrandId)
        {
            return new SuccessDataResult<SubBrand>(true, "Listed", _subBrandDal.Get(p => p.Id == subBrandId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(SubBrandValidator))]
        [TransactionScopeAspect]
        public IResult Add(SubBrand subBrand)
        {
            IResult result = BusinessRules.Run(CheckIfSubBrandNameExists(subBrand));

            if (result != null)
                return result;

            _subBrandDal.Add(subBrand);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(SubBrandValidator))]
        [TransactionScopeAspect]
        public IResult Update(SubBrand subBrand)
        {
            IResult result = BusinessRules.Run(CheckIfSubBrandNameExists(subBrand));

            if (result != null)
                return result;

            _subBrandDal.Add(subBrand);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(SubBrand subBrand)
        {
            _subBrandDal.Delete(subBrand);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfSubBrandNameExists(SubBrand subBrand)
        {
            var result = _subBrandDal.GetAll(x => x.BrandId == subBrand.BrandId && x.SubBrandName == subBrand.SubBrandName).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
