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
    public class CustomerManager : ICustomerService
    {
        private ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IDataServiceResult<List<Customer>> GetAll()
        {
            var dbResult = _customerDal.GetAll();

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
            IServiceResult result = BusinessRules.Run(CheckIfCustomerExists(customer));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _customerDal.Add(customer);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(Customer customer)
        {
            IServiceResult result = BusinessRules.Run(CheckIfCustomerExists(customer));
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
        [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(CustomerValidator))]
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

        private ServiceResult CheckIfCustomerExists(Customer customer)
        {
            var result = _customerDal.GetAll(x => x.CustomerName == customer.CustomerName);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "CustomerAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
