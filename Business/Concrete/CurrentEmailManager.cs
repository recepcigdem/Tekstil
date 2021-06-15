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
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Current;

namespace Business.Concrete
{
    public class CurrentEmailManager : ICurrentEmailService
    {
        private ICurrentEmailDal _currentEmailDal;
        private IEmailService _emailService;

        public CurrentEmailManager(ICurrentEmailDal currentEmailDal, IEmailService emailService)
        {
            _currentEmailDal = currentEmailDal;
            _emailService = emailService;
        }

        public IDataServiceResult<List<CurrentEmail>> GetAll(int customerId)
        {
            var dbResult = _currentEmailDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<CurrentEmail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CurrentEmail> GetById(int currentEmailId)
        {
            var dbResult = _currentEmailDal.Get(p => p.Id == currentEmailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CurrentEmail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CurrentEmail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CurrentEmail> GetByEmailId(int emailId)
        {
            var dbResult = _currentEmailDal.Get(p => p.EmailId == emailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CurrentEmail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CurrentEmail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CurrentEmail>> GetAllByCurrentId(int currentId)
        {
            var dbResult = _currentEmailDal.GetAll(x => x.CurrentId == currentId);

            return new SuccessDataServiceResult<List<CurrentEmail>>(dbResult, true, "Listed");
        }

        public IServiceResult Add(CurrentEmail currentEmail)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(currentEmail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _currentEmailDal.Add(currentEmail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(CurrentEmail currentEmail)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(currentEmail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _currentEmailDal.Update(currentEmail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(CurrentEmail currentEmail)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CurrentEmail>(false, message);
            }

            #endregion

            var result = _currentEmailDal.Delete(currentEmail);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteByCurrent(Customer customer)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new ErrorServiceResult(false, message);
            }

            #endregion

            var currentEmailList = GetAllByCurrentId(customer.Id);
            if (currentEmailList.Result == false)
                return new ErrorServiceResult(false, "CurrentEmailNotFound");

            foreach (var currentEmail in currentEmailList.Data)
            {
                var deleteCurrentEmail = Delete(currentEmail);
                if (deleteCurrentEmail.Result == false)
                    return new ErrorServiceResult(false, "CurrentEmailNotDeleted");

                var email = _emailService.GetById(currentEmail.EmailId);
                if (email.Result == false)
                    return new ErrorServiceResult(false, "CurrentEmailNotFound");

                var deleteEmail = _emailService.Delete(email.Data);
                if (deleteEmail.Result == false)
                    return new ErrorServiceResult(false, "CurrentEmailNotDeleted");

            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(CurrentEmailValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<CurrentEmail> Save(Customer customer, List<CurrentEmailDto> currentEmailDtos)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CurrentEmail>(false, message);
            }

            #endregion

            DeleteByCurrent(customer);

            int customerId = (int)customer.CustomerId;
            if (customer.CustomerId == 0)
                customerId = 0;

            foreach (var currentEmailDto in currentEmailDtos)
            {
                Email email = new Email
                {
                    CustomerId = customerId,
                    IsActive = currentEmailDto.IsActive,
                    EmailAddress = currentEmailDto.EmailAddress
                };

                _emailService.Add(email);

                CurrentEmail currentEmail = new CurrentEmail
                {
                    CustomerId = customerId,
                    CurrentId = customer.Id,
                    EmailId = email.Id,
                    IsMain = currentEmailDto.IsMain
                };

                Add(currentEmail);
            }


            return new SuccessDataServiceResult<CurrentEmail>(true, "Saved");
        }

        private ServiceResult CheckIfEmailExists(CurrentEmail currentEmail)
        {
            var result = _currentEmailDal.GetAll(x => x.CurrentId == currentEmail.CurrentId && x.EmailId == currentEmail.EmailId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "EmailAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
