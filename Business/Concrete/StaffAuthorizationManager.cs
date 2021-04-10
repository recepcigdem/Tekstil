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
    public class StaffAuthorizationManager : IStaffAuthorizationService
    {
        private IStaffAuthorizationDal _staffAuthorizationDal;

        public StaffAuthorizationManager(IStaffAuthorizationDal staffAuthorizationDal)
        {
            _staffAuthorizationDal = staffAuthorizationDal;
        }

        public IDataResult<List<StaffAuthorization>> GetAll()
        {
            return new SuccessDataResult<List<StaffAuthorization>>(true, "Listed", _staffAuthorizationDal.GetAll());
        }

        public IDataResult<StaffAuthorization> GetById(int staffAuthorizationId)
        {
            return new SuccessDataResult<StaffAuthorization>(true, "Listed", _staffAuthorizationDal.Get(p => p.Id == staffAuthorizationId));
        }

        [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffAuthorizationValidator))]
        [TransactionScopeAspect]
        public IResult Add(StaffAuthorization staffAuthorization)
        {
            IResult result = BusinessRules.Run(CheckIfAuthorizationExists(staffAuthorization));

            if (result != null)
                return result;

            _staffAuthorizationDal.Add(staffAuthorization);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffAuthorizationValidator))]
        [TransactionScopeAspect]
        public IResult Update(StaffAuthorization staffAuthorization)
        {
            IResult result = BusinessRules.Run(CheckIfAuthorizationExists(staffAuthorization));

            if (result != null)
                return result;

            _staffAuthorizationDal.Add(staffAuthorization);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(StaffAuthorization staffAuthorization)
        {
            _staffAuthorizationDal.Delete(staffAuthorization);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfAuthorizationExists(StaffAuthorization staffAuthorization)
        {
            var result = _staffAuthorizationDal.GetAll(x => x.StaffId == staffAuthorization.StaffId && x.AuthorizationId == staffAuthorization.AuthorizationId).Any();

            if (result)
                new ErrorResult("AuthorizationAlreadyExists");

            return new SuccessResult();
        }
    }
}
