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
    public class SupplierMethodManager : ISupplierMethodService
    {
        private ISupplierMethodDal _supplierMethodDal;

        public SupplierMethodManager(ISupplierMethodDal supplierMethodDal)
        {
            _supplierMethodDal = supplierMethodDal;
        }

        public IDataResult<List<SupplierMethod>> GetAll()
        {
            return new SuccessDataResult<List<SupplierMethod>>(true, "Listed", _supplierMethodDal.GetAll());
        }

        public IDataResult<SupplierMethod> GetById(int supplierMethodId)
        {
            return new SuccessDataResult<SupplierMethod>(true, "Listed", _supplierMethodDal.Get(p => p.Id == supplierMethodId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(SupplierMethodValidator))]
        [TransactionScopeAspect]
        public IResult Add(SupplierMethod supplierMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(supplierMethod), CheckIfDescriptionExists(supplierMethod));

            if (result != null)
                return result;

            _supplierMethodDal.Add(supplierMethod);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(SupplierMethodValidator))]
        [TransactionScopeAspect]
        public IResult Update(SupplierMethod supplierMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(supplierMethod), CheckIfDescriptionExists(supplierMethod));

            if (result != null)
                return result;

            _supplierMethodDal.Add(supplierMethod);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(SupplierMethod supplierMethod)
        {
            _supplierMethodDal.Delete(supplierMethod);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(SupplierMethod supplierMethod)
        {
            var result = _supplierMethodDal.GetAll(x => x.Description == supplierMethod.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(SupplierMethod supplierMethod)
        {
            var result = _supplierMethodDal.GetAll(x => x.Code == supplierMethod.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
