using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class DefinitionTitleManager : IDefinitionTitleService
    {
        private IDefinitionTitleDal _definitionTitleDal;
        private IDefinitionService _definitionService;

        public DefinitionTitleManager(IDefinitionTitleDal definitionTitleDal, IDefinitionService definitionService)
        {
            _definitionTitleDal = definitionTitleDal;
            _definitionService = definitionService;
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IDataServiceResult<List<DefinitionTitle>> GetAll(int customerId)
        {
            var dbResult = _definitionTitleDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<DefinitionTitle>>(dbResult, true, "Listed");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IDataServiceResult<DefinitionTitle> GetByValue(int customerId, int value)
        {
            var dbResult = _definitionTitleDal.Get(x => x.CustomerId == customerId && x.Value == value);

            return new SuccessDataServiceResult<DefinitionTitle>(dbResult, true, "Listed");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IDataServiceResult<DefinitionTitle> GetById(int definitionTitleId)
        {
            var dbResult = _definitionTitleDal.Get(p => p.Id == definitionTitleId);
            if (dbResult == null)
                return new SuccessDataServiceResult<DefinitionTitle>(false, "Error_SystemError");

            return new SuccessDataServiceResult<DefinitionTitle>(dbResult, true, "Listed");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(DefinitionTitleValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<DefinitionTitle> Save(DefinitionTitle definitionTitle)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<DefinitionTitle>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            if (definitionTitle.Id > 0)
            {
                var result = _definitionTitleDal.Update(definitionTitle);
                if (result == null)
                    return new DataServiceResult<DefinitionTitle>(false, "Error_SystemError");
            }
            else
            {
                var result = _definitionTitleDal.Add(definitionTitle);
                if (result == null)
                    return new DataServiceResult<DefinitionTitle>(false, "Error_SystemError");
            }

            return new SuccessDataServiceResult<DefinitionTitle>(true, "Saved");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(DefinitionTitle definitionTitle)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<DefinitionTitle>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            IServiceResult isUsedResult = BusinessRules.Run(CheckIfDefinitionTitleIsUsed(definitionTitle));
            if (isUsedResult.Result == false)
                return new ErrorServiceResult(false, isUsedResult.Message);

            var result = _definitionTitleDal.Delete(definitionTitle);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Delated");
        }

        private ServiceResult CheckIfDefinitionTitleIsUsed(DefinitionTitle definitionTitle)
        {
            var result = _definitionService.GetAllByCustomerIdAndDefinitionTitleId(definitionTitle.CustomerId, definitionTitle.Id);
            if (result.Result)
                return new ErrorServiceResult(false, "Message_DefinitionTitleIsUsedDefinition");

            return new ServiceResult(true, "");
        }
    }
}
