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
    public class FoldedProductManager : IFoldedProductService
    {
        private IFoldedProductDal _foldedProductDal;

        public FoldedProductManager(IFoldedProductDal foldedProductDal)
        {
            _foldedProductDal = foldedProductDal;
        }

        public IDataResult<List<FoldedProduct>> GetAll()
        {
            return new SuccessDataResult<List<FoldedProduct>>(true, "Listed", _foldedProductDal.GetAll());
        }

        public IDataResult<FoldedProduct> GetById(int foldedProductId)
        {
            return new SuccessDataResult<FoldedProduct>(true, "Listed", _foldedProductDal.Get(p => p.Id == foldedProductId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(FoldedProductValidator))]
        [TransactionScopeAspect]
        public IResult Add(FoldedProduct foldedProduct)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(foldedProduct), CheckIfDescriptionExists(foldedProduct));

            if (result != null)
                return result;

            _foldedProductDal.Add(foldedProduct);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(FoldedProductValidator))]
        [TransactionScopeAspect]
        public IResult Update(FoldedProduct foldedProduct)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(foldedProduct), CheckIfDescriptionExists(foldedProduct));

            if (result != null)
                return result;

            _foldedProductDal.Add(foldedProduct);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(FoldedProduct foldedProduct)
        {
            _foldedProductDal.Delete(foldedProduct);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(FoldedProduct foldedProduct)
        {
            var result = _foldedProductDal.GetAll(x => x.Description == foldedProduct.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(FoldedProduct foldedProduct)
        {
            var result = _foldedProductDal.GetAll(x => x.Code == foldedProduct.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
