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
    public class EmailManager : IEmailService
    {
        private IEmailDal _emailDal;

        public EmailManager(IEmailDal emailDal)
        {
            _emailDal = emailDal;
        }
        public IDataServiceResult<List<Email>> GetAll(int customerId)
        {
            var dbResult = _emailDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<Email>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Email> GetById(int emailId)
        {
            var dbResult = _emailDal.Get(p => p.Id == emailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Email>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Email>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Email> GetByEmail(string email)
        {
            var dbResult = _emailDal.Get(p => p.EmailAddress == email);
            if (dbResult == null)
                return new SuccessDataServiceResult<Email>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Email>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Email email)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(email));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _emailDal.Add(email);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(Email email)
        {
            
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(email));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _emailDal.Update(email);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(Email email)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Email>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _emailDal.Delete(email);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(EmailValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Email> Save(Email email)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Email>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            if (email.Id > 0)
            {
                var result = Update(email);
                if (result.Result == false)
                    return new DataServiceResult<Email>(false, result.Message);
            }
            else
            {
                var result = Add(email);
                if (result.Result == false)
                    return new DataServiceResult<Email>(false, result.Message);
            }

            return new SuccessDataServiceResult<Email>(true, "Saved");
        }

        private ServiceResult CheckIfEmailExists(Email email)
        {
            var result = _emailDal.GetAll(x => x.EmailAddress == email.EmailAddress);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "EmailAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
