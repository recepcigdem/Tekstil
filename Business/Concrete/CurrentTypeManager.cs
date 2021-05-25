using System;
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
    public class CurrentTypeManager : ICurrentTypeService
    {
        private ICurrentTypeDal _currentTypeDal;

        public CurrentTypeManager(ICurrentTypeDal currentTypeDal)
        {
            _currentTypeDal = currentTypeDal;
        }

        public IDataServiceResult<List<CurrentType>> GetAll(int customerId)
        {
            var dbResult = _currentTypeDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<CurrentType>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CurrentType> GetById(int currentTypeId)
        {
            var dbResult = _currentTypeDal.Get(p => p.Id == currentTypeId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CurrentType>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CurrentType>(dbResult, true, "Listed");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(CurrentType currentType)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CurrentType>(false, message);
            }

            #endregion

            var result = _currentTypeDal.Delete(currentType);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(CurrentTypeValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<CurrentType> Save(CurrentType currentType)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CurrentType>(false, message);
            }

            #endregion

            IServiceResult resultControl = BusinessRules.Run(CheckIfCurrentTypeExists(currentType));
            if (resultControl.Result == false)
                return new DataServiceResult<CurrentType>(false, resultControl.Message);

            if (currentType.Id > 0)
            {
                var dbResult = _currentTypeDal.Update(currentType);
                if (dbResult == null)
                    return new DataServiceResult<CurrentType>(false, "Error_SystemError");
            }
            else
            {
                var dbResult = _currentTypeDal.Add(currentType);
                if (dbResult == null)
                    return new DataServiceResult<CurrentType>(false, "Error_SystemError");
            }

            return new SuccessDataServiceResult<CurrentType>(true, "Saved");
        }

        private ServiceResult CheckIfCurrentTypeExists(CurrentType currentType)
        {
            var result = _currentTypeDal.GetAll(x => x.CustomerId == currentType.CustomerId && x.Description == currentType.Description);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "CurrentTypeAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
