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
    public class SupplierManager : ISupplierService
    {
        private ISupplierDal _supplierDal;

        public SupplierManager(ISupplierDal supplierDal)
        {
            _supplierDal = supplierDal;
        }

        public IDataResult<List<Supplier>> GetAll()
        {
            return new SuccessDataResult<List<Supplier>>(true, "Listed", _supplierDal.GetAll());
        }

        public IDataResult<Supplier> GetById(int supplierId)
        {
            return new SuccessDataResult<Supplier>(true, "Listed", _supplierDal.Get(p => p.Id == supplierId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(SupplierValidator))]
        [TransactionScopeAspect]
        public IResult Add(Supplier supplier)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(supplier), CheckIfDescriptionExists(supplier));

            if (result != null)
                return result;

            _supplierDal.Add(supplier);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(SupplierValidator))]
        [TransactionScopeAspect]
        public IResult Update(Supplier supplier)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(supplier), CheckIfDescriptionExists(supplier));

            if (result != null)
                return result;

            _supplierDal.Add(supplier);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Supplier supplier)
        {
            _supplierDal.Delete(supplier);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Supplier supplier)
        {
            var result = _supplierDal.GetAll(x => x.Description == supplier.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Supplier supplier)
        {
            var result = _supplierDal.GetAll(x => x.Code == supplier.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
