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
using Entities.Concrete.Dtos;

namespace Business.Concrete
{
    public class StaffManager : IStaffService
    {
        private IStaffDal _staffDal;
        private IStaffAuthorizationService _staffAuthorizationService;
        private IStaffEmailService _staffEmailService;
        private IStaffPhoneService _staffPhoneService;

        public StaffManager(IStaffDal staffDal, IStaffAuthorizationService staffAuthorizationService, IStaffEmailService staffEmailService, IStaffPhoneService staffPhoneService)
        {
            _staffDal = staffDal;
            _staffAuthorizationService = staffAuthorizationService;
            _staffEmailService = staffEmailService;
            _staffPhoneService = staffPhoneService;
        }

        public IDataResult<List<Staff>> GetAll()
        {
            return new SuccessDataResult<List<Staff>>(true, "Listed", _staffDal.GetAll());
        }

        public IDataResult<Staff> GetById(int staffId)
        {
            var result = _staffDal.Get(s => s.Id == staffId);
            if (result==null)
                return new ErrorDataResult<Staff>("StaffNotFound");
            
            return new SuccessDataResult<Staff>(true, "Listed", result );
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

        [SecuredOperation("admin,staff.saved")]
        [ValidationAspect(typeof(StaffValidator))]
        [TransactionScopeAspect]
        public IDataResult<Staff> SaveAll(Staff staff, List<StaffEmailDto> staffEmailDtos, List<StaffPhoneDto> staffPhoneDtos, List<StaffAuthorization> staffAuthorizations)
        {
            if (staff.Id > 0)
            {
                Update(staff);
            }
            else
            {
                Add(staff);
            }

            foreach (var staffEmailDto in staffEmailDtos)
            {
                _staffEmailService.Save(staffEmailDto);
            }

            foreach (var staffPhoneDto in staffPhoneDtos)
            {
                _staffPhoneService.Save(staffPhoneDto);
            }

            foreach (var staffAuthorization in staffAuthorizations)
            {
                _staffAuthorizationService.Save(staffAuthorization);
            }

            return new SuccessDataResult<Staff>(true,"Saved" ,staff);
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
