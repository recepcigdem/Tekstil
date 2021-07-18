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
    public class FabricDetailManager : IFabricDetailService
    {
        private IFabricDetailDal _fabricDetailDal;

        public FabricDetailManager(IFabricDetailDal fabricDetailDal)
        {
            _fabricDetailDal = fabricDetailDal;
        }

        public IDataServiceResult<List<FabricDetail>> GetAll(int customerId)
        {
            var dbResult = _fabricDetailDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<FabricDetail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<FabricDetail> GetById(int fabricDetailId)
        {
            var dbResult = _fabricDetailDal.Get(p => p.Id == fabricDetailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<FabricDetail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<FabricDetail>(dbResult, true, "Listed");
        }

        public IServiceResult Add(FabricDetail fabricDetail)
        {

            var dbResult = _fabricDetailDal.Add(fabricDetail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(FabricDetail fabricDetail)
        {
          
            var dbResult = _fabricDetailDal.Update(fabricDetail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(FabricDetail fabricDetail)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricDetail>(false, message);
            }

            #endregion

            var deleteResult = _fabricDetailDal.Delete(fabricDetail);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.saved")]
        [ValidationAspect(typeof(FabricDetailValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<FabricDetail> Save(FabricDetail fabricDetail)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricDetail>(false, message);
            }

            #endregion

            if (fabricDetail.Id > 0)
            {
                Update(fabricDetail);
            }
            else
            {
                Add(fabricDetail);
            }

            return new SuccessDataServiceResult<FabricDetail>(true, "Saved");
        }

    }
}
