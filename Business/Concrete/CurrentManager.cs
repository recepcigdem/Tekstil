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
using Entities.Concrete.Dtos.Current;

namespace Business.Concrete
{
    public class CurrentManager : ICurrentService
    {
        private ICustomerDal _customerDal;
        private ICurrentEmailService _currentEmailService;
        private ICurrentPhoneService _currentPhoneService;

        public CurrentManager(ICustomerDal customerDal, ICurrentEmailService currentEmailService, ICurrentPhoneService currentPhoneService)
        {
            _customerDal = customerDal;
            _currentEmailService = currentEmailService;
            _currentPhoneService = currentPhoneService;
        }
        public IDataServiceResult<List<Customer>> GetAll(bool isCurrent, int customerId)
        {
            var dbResult = _customerDal.GetAll(x => x.IsCurrent == isCurrent && x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Customer>>(dbResult, true, "Listed");
        }
        public IDataServiceResult<Customer> GetById(int customerId)
        {
            var dbResult = _customerDal.Get(p => p.Id == customerId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Customer>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Customer>(dbResult, true, "Listed");
        }
        public IServiceResult Add(Customer customer)
        {
            IServiceResult result = BusinessRules.Run(CheckIfCurrentExists(customer));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _customerDal.Add(customer);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }
        public IServiceResult Update(Customer customer)
        {
            IServiceResult result = BusinessRules.Run(CheckIfCurrentExists(customer));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _customerDal.Update(customer);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Customer customer)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Customer>(false, message);
            }

            #endregion

            var result = _customerDal.Delete(customer);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin")]
        [TransactionScopeAspect]
        public IServiceResult DeleteAll(Customer customer)
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

            var currentEmail = _currentEmailService.DeleteByCurrent(customer);
            if (currentEmail.Result == false)
                return new ErrorServiceResult(false, "CurrentEmailNotFound");

            var currentPhone = _currentPhoneService.DeleteByCurrent(customer);
            if (currentPhone.Result == false)
                return new ErrorServiceResult(false, "CurrentPhoneNotFound");


            var currentDelete = _customerDal.Delete(customer);
            if (currentDelete == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(CurrentValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Customer> Save(Customer customer)
        {

            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Customer>(false, message);
            }

            #endregion

            if (customer.Id > 0)
            {
                var result = Update(customer);
                if (result.Result == false)
                    return new DataServiceResult<Customer>(false, result.Message);
            }
            else
            {
                var result = Add(customer);
                if (result.Result == false)
                    return new DataServiceResult<Customer>(false, result.Message);
            }

            return new SuccessDataServiceResult<Customer>(true, "Saved");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin")]
        [ValidationAspect(typeof(CurrentValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Customer> SaveAll(Customer customer, List<CurrentEmailDto> currentEmailDtos, List<CurrentPhoneDto> currentPhoneDtos)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Customer>(false, message);
            }

            #endregion

            if (customer.Id > 0)
            {
                Update(customer);
            }
            else
            {
                Add(customer);
            }

            _currentEmailService.Save(customer, currentEmailDtos);

            _currentPhoneService.Save(customer, currentPhoneDtos);

            return new SuccessDataServiceResult<Customer>(customer, true, "Saved");
        }

        private ServiceResult CheckIfCurrentExists(Customer customer)
        {
            var result = _customerDal.GetAll(x => x.CustomerName == customer.CustomerName && x.IsCurrent == true);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "CurrentAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
