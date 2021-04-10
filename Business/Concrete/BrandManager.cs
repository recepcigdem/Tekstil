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
    public class BrandManager : IBrandService
    {
        private IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public IDataResult<List<Brand>> GetAll()
        {
            return new SuccessDataResult<List<Brand>>(true, "Listed", _brandDal.GetAll());
        }

        public IDataResult<Brand> GetById(int brandId)
        {
            return new SuccessDataResult<Brand>(true, "Listed", _brandDal.Get(p => p.Id == brandId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(BrandValidator))]
        [TransactionScopeAspect]
        public IResult Add(Brand brand)
        {
            IResult result = BusinessRules.Run(CheckIfBrandNameExists(brand));

            if (result != null)
                return result;

            _brandDal.Add(brand);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(BrandValidator))]
        [TransactionScopeAspect]
        public IResult Update(Brand brand)
        {
            IResult result = BusinessRules.Run(CheckIfBrandNameExists(brand));

            if (result != null)
                return result;

            _brandDal.Add(brand);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Brand brand)
        {
            _brandDal.Delete(brand);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfBrandNameExists(Brand brand)
        {
            var result = _brandDal.GetAll(x => x.BrandName == brand.BrandName).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
