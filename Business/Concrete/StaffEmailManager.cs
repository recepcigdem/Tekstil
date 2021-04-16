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
using Entities.Concrete.Dtos;

namespace Business.Concrete
{
    public class StaffEmailManager : IStaffEmailService
    {
        private IStaffEmailDal _staffEmailDal;
        private IEmailService _emailService;

        public StaffEmailManager(IStaffEmailDal staffEmailDal, IEmailService emailService)
        {
            _staffEmailDal = staffEmailDal;
            _emailService = emailService;
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

            return new SuccessResult(true, "Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IResult Update(StaffEmail staffEmail)
        {
            IResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));

            if (result != null)
                return result;

            _staffEmailDal.Update(staffEmail);

            return new SuccessResult(true, "Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(StaffEmail staffEmail)
        {
            _staffEmailDal.Delete(staffEmail);

            return new SuccessResult(true, "Deleted");
        }

        private IResult CheckIfEmailExists(StaffEmail staffEmail)
        {
            var result = _staffEmailDal.GetAll(x => x.StaffId == staffEmail.StaffId && x.EmailId == staffEmail.EmailId).Any();

            if (result)
                new ErrorResult("EmailAlreadyExists");

            return new SuccessResult();
        }
        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult DeleteByStaffIdWithEmail(int staffId)
        {
            var result = _staffEmailDal.Get(se => se.StaffId == staffId);
            if (result == null)
                new ErrorResult("EmailNotFound");

            _staffEmailDal.DeleteByFilter(sp => sp.StaffId == staffId);

            _emailService.DeleteByEmailId(result.EmailId);

            return new SuccessResult(true, "Deleted");
        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IResult Save(StaffEmailDto staffEmailDto)
        {
            StaffEmail staffEmail = new StaffEmail
            {
                Id = staffEmailDto.Id,
                StaffId = staffEmailDto.StaffId,
                EmailId = staffEmailDto.EmailId,
                IsMain = staffEmailDto.IsMain
            };

            Email email = new Email
            {
                Id = staffEmail.EmailId,
                IsActive = staffEmailDto.IsActive,
                EmailAddress = staffEmailDto.EmailAddress
            };

            if (staffEmailDto.Id > 0)
            {
                Update(staffEmail);
               
            }
            else
            {
                Add(staffEmail);
            }
            _emailService.Save(email);

            return new SuccessResult(true, "Saved");
        }
    }
}
