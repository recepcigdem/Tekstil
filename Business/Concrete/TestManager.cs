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

        public IDataResult<List<Test>> GetAll()
        {
            return new SuccessDataResult<List<Test>>(true, "Listed", _testDal.GetAll());
        }

        public IDataResult<Test> GetById(int testId)
        {
            return new SuccessDataResult<Test>(true, "Listed", _testDal.Get(p => p.Id == testId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(TestValidator))]
        [TransactionScopeAspect]
        public IResult Add(Test test)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(test), CheckIfDescriptionExists(test));

            if (result != null)
                return result;

            _testDal.Add(test);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(TestValidator))]
        [TransactionScopeAspect]
        public IResult Update(Test test)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(test), CheckIfDescriptionExists(test));

            if (result != null)
                return result;

            _testDal.Add(test);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Test test)
        {
            _testDal.Delete(test);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Test test)
        {
            var result = _testDal.GetAll(x => x.Description == test.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Test test)
        {
            var result = _testDal.GetAll(x => x.Code == test.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
