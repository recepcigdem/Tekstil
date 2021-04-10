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
    public class StaffEmailManager : IStaffEmailService
    {
        private IStaffEmailDal _staffEmailDal;

        public StaffEmailManager(IStaffEmailDal staffEmailDal)
        {
            _staffEmailDal = staffEmailDal;
        }

        public IDataResult<List<StaffEmail>> GetAll()
        {
            return new SuccessDataResult<List<StaffEmail>>(true, "Listed", _staffEmailDal.GetAll());
        }

        public IDataResult<StaffEmail> GetById(int staffEmailId)
        {
            return new SuccessDataResult<StaffEmail>(true, "Listed", _staffEmailDal.Get(p => p.Id == staffEmailId));
        }

        [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IResult Add(StaffEmail staffEmail)
        {
            IResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));

            if (result != null)
                return result;

            _staffEmailDal.Add(staffEmail);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IResult Update(StaffEmail staffEmail)
        {
            IResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));

            if (result != null)
                return result;

            _staffEmailDal.Add(staffEmail);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(StaffEmail staffEmail)
        {
            _staffEmailDal.Delete(staffEmail);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfEmailExists(StaffEmail staffEmail)
        {
            var result = _staffEmailDal.GetAll(x => x.StaffId == staffEmail.StaffId && x.EmailId == staffEmail.EmailId).Any();

            if (result)
                new ErrorResult("EmailAlreadyExists");

            return new SuccessResult();
        }
    }
}
