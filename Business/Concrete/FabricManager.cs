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
    public class FabricManager : IFabricService
    {
        private IFabricDal _fabricDal;

        public FabricManager(IFabricDal fabricDal)
        {
            _fabricDal = fabricDal;
        }

        public IDataServiceResult<List<Fabric>> GetAll(int customerId)
        {
            var dbResult = _fabricDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Fabric>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Fabric> GetById(int fabricId)
        {
            var dbResult = _fabricDal.Get(p => p.Id == fabricId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Fabric>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Fabric>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Fabric fabric)
        {
            ServiceResult result = BusinessRules.Run(CheckIfFabricExists(fabric));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _fabricDal.Add(fabric);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(Fabric fabric)
        {
            ServiceResult result = BusinessRules.Run(CheckIfFabricExists(fabric));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _fabricDal.Update(fabric);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Fabric fabric)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Fabric>(false, message);
            }

            #endregion

            ServiceResult result = BusinessRules.Run(CheckIfFabricIsUsed(fabric));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _fabricDal.Delete(fabric);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.saved")]
        [ValidationAspect(typeof(FabricValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Fabric> Save(Fabric fabric)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Fabric>(false, message);
            }

            #endregion

            if (fabric.Id > 0)
            {
                Update(fabric);
            }
            else
            {
                Add(fabric);
            }

            return new SuccessDataServiceResult<Fabric>(true, "Saved");
        }

        private ServiceResult CheckIfFabricExists(Fabric fabric)
        {
            var result = _fabricDal.GetAll(x => x.CustomerId == fabric.CustomerId && x.SeasonId == fabric.SeasonId && x.Definition == fabric.Definition);

            if (result.Count > 1)
                new ErrorServiceResult(false, "FabricAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfFabricIsUsed(Fabric fabric)
        {
            var result = GetById(fabric.Id);
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "FabricIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
