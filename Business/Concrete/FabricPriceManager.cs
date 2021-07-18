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
    public class FabricPriceManager : IFabricPriceService
    {
        private IFabricPriceDal _fabricPriceDal;

        public FabricPriceManager(IFabricPriceDal fabricPriceDal)
        {
            _fabricPriceDal = fabricPriceDal;
        }

        public IDataServiceResult<List<FabricPrice>> GetAll(int customerId)
        {
            var dbResult = _fabricPriceDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<FabricPrice>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<FabricPrice> GetById(int fabricPriceId)
        {
            var dbResult = _fabricPriceDal.Get(p => p.Id == fabricPriceId);
            if (dbResult == null)
                return new SuccessDataServiceResult<FabricPrice>(false, "Error_SystemError");

            return new SuccessDataServiceResult<FabricPrice>(dbResult, true, "Listed");
        }

        public IServiceResult Add(FabricPrice fabricPrice)
        {

            var dbResult = _fabricPriceDal.Add(fabricPrice);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(FabricPrice fabricPrice)
        {
          
            var dbResult = _fabricPriceDal.Update(fabricPrice);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(FabricPrice fabricPrice)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricPrice>(false, message);
            }

            #endregion

            var deleteResult = _fabricPriceDal.Delete(fabricPrice);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.saved")]
        [ValidationAspect(typeof(FabricPriceValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<FabricPrice> Save(FabricPrice fabricPrice)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricPrice>(false, message);
            }

            #endregion

            if (fabricPrice.Id > 0)
            {
                Update(fabricPrice);
            }
            else
            {
                Add(fabricPrice);
            }

            return new SuccessDataServiceResult<FabricPrice>(true, "Saved");
        }

    }
}
