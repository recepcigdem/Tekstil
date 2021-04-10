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

        public IDataResult<List<Customer>> GetAll()
        {
            return new SuccessDataResult<List<Customer>>(true, "Listed", _customerDal.GetAll());
        }

        public IDataResult<Customer> GetById(int customerId)
        {
            return new SuccessDataResult<Customer>(true, "Listed", _customerDal.Get(p => p.Id == customerId));
        }

        [SecuredOperation("admin,customer.add")]
        [ValidationAspect(typeof(CustomerValidator))]
        [TransactionScopeAspect]
        public IResult Add(Customer customer)
        {
            IResult result = BusinessRules.Run(CheckIfCustomerNameExists(customer));

            if (result != null)
                return result;

            _customerDal.Add(customer);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,customer.updated")]
        [ValidationAspect(typeof(CustomerValidator))]
        [TransactionScopeAspect]
        public IResult Update(Customer customer)
        {
            IResult result = BusinessRules.Run(CheckIfCustomerNameExists(customer));

            if (result != null)
                return result;

            _customerDal.Add(customer);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,customer.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Customer customer)
        {
            _customerDal.Delete(customer);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfCustomerNameExists(Customer customer)
        {
            var result = _customerDal.GetAll(x => x.CustomerName == customer.CustomerName).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
