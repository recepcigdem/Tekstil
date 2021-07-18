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
    public class FabricColorLabelManager : IFabricColorLabelService
    {
        private IFabricColorLabelDal _fabricColorLabelDal;

        public FabricColorLabelManager(IFabricColorLabelDal fabricColorLabelDal)
        {
            _fabricColorLabelDal = fabricColorLabelDal;
        }

        public IDataServiceResult<List<FabricColorLabel>> GetAll(int customerId)
        {
            var dbResult = _fabricColorLabelDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<FabricColorLabel>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<FabricColorLabel> GetById(int fabricColorLabelId)
        {
            var dbResult = _fabricColorLabelDal.Get(p => p.Id == fabricColorLabelId);
            if (dbResult == null)
                return new SuccessDataServiceResult<FabricColorLabel>(false, "Error_SystemError");

            return new SuccessDataServiceResult<FabricColorLabel>(dbResult, true, "Listed");
        }

        public IServiceResult Add(FabricColorLabel fabricColorLabel)
        {

            var dbResult = _fabricColorLabelDal.Add(fabricColorLabel);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(FabricColorLabel fabricColorLabel)
        {
          
            var dbResult = _fabricColorLabelDal.Update(fabricColorLabel);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(FabricColorLabel fabricColorLabel)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricColorLabel>(false, message);
            }

            #endregion

            var deleteResult = _fabricColorLabelDal.Delete(fabricColorLabel);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,fabric.saved")]
        [ValidationAspect(typeof(FabricColorLabelValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<FabricColorLabel> Save(FabricColorLabel fabricColorLabel)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<FabricColorLabel>(false, message);
            }

            #endregion

            if (fabricColorLabel.Id > 0)
            {
                Update(fabricColorLabel);
            }
            else
            {
                Add(fabricColorLabel);
            }

            return new SuccessDataServiceResult<FabricColorLabel>(true, "Saved");
        }

    }
}
