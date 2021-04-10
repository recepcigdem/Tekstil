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
    public class ProductGroupManager : IProductGroupService
    {
        private IProductGroupDal _productGroupDal;

        public ProductGroupManager(IProductGroupDal productGroupDal)
        {
            _productGroupDal = productGroupDal;
        }

        public IDataResult<List<ProductGroup>> GetAll()
        {
            return new SuccessDataResult<List<ProductGroup>>(true, "Listed", _productGroupDal.GetAll());
        }

        public IDataResult<ProductGroup> GetById(int productGroupId)
        {
            return new SuccessDataResult<ProductGroup>(true, "Listed", _productGroupDal.Get(p => p.Id == productGroupId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ProductGroupValidator))]
        [TransactionScopeAspect]
        public IResult Add(ProductGroup productGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(productGroup), CheckIfDescriptionExists(productGroup));

            if (result != null)
                return result;

            _productGroupDal.Add(productGroup);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ProductGroupValidator))]
        [TransactionScopeAspect]
        public IResult Update(ProductGroup productGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(productGroup), CheckIfDescriptionExists(productGroup));

            if (result != null)
                return result;

            _productGroupDal.Add(productGroup);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(ProductGroup productGroup)
        {
            _productGroupDal.Delete(productGroup);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(ProductGroup productGroup)
        {
            var result = _productGroupDal.GetAll(x => x.Description == productGroup.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(ProductGroup productGroup)
        {
            var result = _productGroupDal.GetAll(x => x.Code == productGroup.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
