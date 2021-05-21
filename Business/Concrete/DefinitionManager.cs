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
using Core.Utilities.Interceptors;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class DefinitionManager : IDefinitionService
    {
        private IDefinitionDal _definitionDal;

        public DefinitionManager(IDefinitionDal definitionDal)
        {
            _definitionDal = definitionDal;
        }
        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IDataServiceResult<List<Definition>> GetAll(int customerId)
        {
            var dbResult = _definitionDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Definition>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Definition>> GetAllByCustomerIdAndDefinitionTitleId(int customerId,int definitionTitleId)
        {
            var dbResult = _definitionDal.GetAll(x => x.CustomerId == customerId && x.DefinitionTitleId == definitionTitleId);

            return new SuccessDataServiceResult<List<Definition>>(dbResult, true, "Listed");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IDataServiceResult<Definition> GetById(int definitionId)
        {
            var dbResult = _definitionDal.Get(p => p.Id == definitionId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Definition>(false, "SystemError");

            return new SuccessDataServiceResult<Definition>(dbResult, true, "Listed");
        }
        
        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IDataServiceResult<Definition> Save(Definition definition)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Definition>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            if (definition.Id > 0)
            {
                var result = _definitionDal.Update(definition);
                if (result == null)
                    return new DataServiceResult<Definition>(false, "SystemError");
            }
            else
            {
                var result = _definitionDal.Add(definition);
                if (result == null)
                    return new DataServiceResult<Definition>(false, "SystemError");
            }

            return new SuccessDataServiceResult<Definition>(true, "Saved");
        }
        
        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Definition definition)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Definition>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _definitionDal.Delete(definition);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }
    }
}
