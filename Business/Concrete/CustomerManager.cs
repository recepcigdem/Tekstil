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
                return new SuccessDataServiceResult<Customer>(false, "SystemError");

            return new SuccessDataServiceResult<Customer>(dbResult, true, "Listed");
        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(CustomerValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Customer customer)
        {
            IServiceResult result = BusinessRules.Run(CheckIfCustomerExists(customer));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _customerDal.Add(customer);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");

        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(CustomerValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Customer customer)
        {
            IServiceResult result = BusinessRules.Run(CheckIfCustomerExists(customer));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _customerDal.Update(customer);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }

        // [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Customer customer)
        {
            var result = _customerDal.Delete(customer);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(CustomerValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Customer> Save(Customer customer)
        {
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
