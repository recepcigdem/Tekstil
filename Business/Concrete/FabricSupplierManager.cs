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
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;

namespace Business.Concrete
{
    public class FabricSupplierManager : IFabricSupplierService
    {
        private IFabricSupplierDal _fabricSupplierDal;

        public FabricSupplierManager(IFabricSupplierDal fabricSupplierDal)
        {
            _fabricSupplierDal = fabricSupplierDal;
        }

        public IDataServiceResult<List<FabricSupplier>> GetAll(int customerId)
        {
            var dbResult = _fabricSupplierDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<FabricSupplier>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<FabricSupplier> GetById(int fabricSupplierId)
        {
            var dbResult = _fabricSupplierDal.Get(p => p.Id == fabricSupplierId);
            if (dbResult == null)
                return new SuccessDataServiceResult<FabricSupplier>(false, "Error_SystemError");

            return new SuccessDataServiceResult<FabricSupplier>(dbResult, true, "Listed");
        }

        public IServiceResult Add(FabricSupplier fabricSupplier)
        {

            var dbResult = _fabricSupplierDal.Add(fabricSupplier);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(FabricSupplier fabricSupplier)
        {
          
            var dbResult = _fabricSupplierDal.Update(fabricSupplier);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(FabricSupplier fabricSupplier)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricSupplier>(false, message);
            }

            #endregion

            var deleteResult = _fabricSupplierDal.Delete(fabricSupplier);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.saved")]
        [ValidationAspect(typeof(FabricSupplierValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<FabricSupplier> Save(FabricSupplier fabricSupplier)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricSupplier>(false, message);
            }

            #endregion

            if (fabricSupplier.Id > 0)
            {
                Update(fabricSupplier);
            }
            else
            {
                Add(fabricSupplier);
            }

            return new SuccessDataServiceResult<FabricSupplier>(true, "Saved");
        }

    }
}
