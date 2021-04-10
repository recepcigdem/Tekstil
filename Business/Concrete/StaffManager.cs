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
    public class StaffManager : IStaffService
    {
        private IStaffDal _staffDal;

        public StaffManager(IStaffDal staffDal)
        {
            _staffDal = staffDal;
        }

        public IDataResult<List<Staff>> GetAll()
        {
            return new SuccessDataResult<List<Staff>>(true, "Listed", _staffDal.GetAll());
        }

        public IDataResult<Staff> GetById(int staffId)
        {
            return new SuccessDataResult<Staff>(true, "Listed", _staffDal.Get(p => p.Id == staffId));
        }

        [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffValidator))]
        [TransactionScopeAspect]
        public IResult Add(Staff staff)
        {
            IResult result = BusinessRules.Run(CheckIfExists(staff));

            if (result != null)
                return result;

            _staffDal.Add(staff);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffValidator))]
        [TransactionScopeAspect]
        public IResult Update(Staff staff)
        {
            IResult result = BusinessRules.Run(CheckIfExists(staff));

            if (result != null)
                return result;

            _staffDal.Add(staff);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Staff staff)
        {
            _staffDal.Delete(staff);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfExists(Staff staff)
        {
            var result = _staffDal.GetAll(x => x.FirstName == staff.FirstName && x.LastName == staff.LastName).Any();

            if (result)
                new ErrorResult("AlreadyExists");

            return new SuccessResult();
        }
    }
}
