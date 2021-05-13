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
    public class TestManager : ITestService
    {
        private ITestDal _testDal;

        public TestManager(ITestDal testDal)
        {
            _testDal = testDal;
        }

        public IDataServiceResult<List<Test>> GetAll(int customerId)
        {
            var dbResult = _testDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Test>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Test> GetById(int testId)
        {
            var dbResult = _testDal.Get(p => p.Id == testId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Test>(false, "SystemError");

            return new SuccessDataServiceResult<Test>(dbResult, true, "Listed");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [ValidationAspect(typeof(TestValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Test test)
        {
            ServiceResult result = BusinessRules.Run(CheckIfDescriptionExists(test), CheckIfCodeExists(test));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _testDal.Add(test);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [ValidationAspect(typeof(TestValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Test test)
        {
            ServiceResult result = BusinessRules.Run(CheckIfDescriptionExists(test), CheckIfCodeExists(test));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _testDal.Update(test);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Test test)
        {
            ServiceResult result = BusinessRules.Run(CheckIfTestIsUsed(test));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _testDal.Delete(test);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<Test> Save(Test test)
        {
            if (test.Id > 0)
            {
                Update(test);
            }
            else
            {
                Add(test);
            }

            return new SuccessDataServiceResult<Test>(true, "Saved");
        }

        private ServiceResult CheckIfDescriptionExists(Test test)
        {
            var result = _testDal.GetAll(x => x.CustomerId==test.CustomerId && x.Description == test.Description);

            if (result.Count > 1)
                new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfCodeExists(Test test)
        {
            var result = _testDal.GetAll(x => x.CustomerId == test.CustomerId && x.Code == test.Code);

            if (result.Count > 1)
                new ErrorServiceResult(false, "CodeAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfTestIsUsed(Test test)
        {
            var result = GetById(test.Id);
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "TestIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
