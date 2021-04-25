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

        public IDataServiceResult<List<StaffEmail>> GetAll()
        {
            var dbResult = _staffEmailDal.GetAll();

            return new SuccessDataServiceResult<List<StaffEmail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffEmail> GetById(int staffEmailId)
        {
            var dbResult = _staffEmailDal.Get(p => p.Id == staffEmailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffEmail>(false, "SystemError");

            return new SuccessDataServiceResult<StaffEmail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffEmail> GetByEmailId(int emailId)
        {
            var dbResult = _staffEmailDal.Get(p => p.EmailId == emailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffEmail>(false, "SystemError");

            return new SuccessDataServiceResult<StaffEmail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<StaffEmail>> GetAllByStaffId(int staffId)
        {
            var dbResult = _staffEmailDal.GetAll(x => x.StaffId == staffId);

            return new SuccessDataServiceResult<List<StaffEmail>>(dbResult, true, "Listed");
        }

        //[SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(StaffEmail staffEmail)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffEmailDal.Add(staffEmail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");

        }

        //[SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(StaffEmail staffEmail)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffEmailDal.Update(staffEmail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }

        //[SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(StaffEmail staffEmail)
        {
            var result = _staffEmailDal.Delete(staffEmail);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

       // [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IServiceResult DeleteByStaff(Staff staff)
        {
            var staffEmailList = GetAllByStaffId(staff.Id);
            if (staffEmailList.Result == false)
                return new ErrorServiceResult(false, "StaffEmailNotFound");

            foreach (var staffEmail in staffEmailList.Data)
            {
                var email = _emailService.GetById(staffEmail.EmailId);
                if (email.Result == false)
                    return new ErrorServiceResult(false, "StaffEmailNotFound");

                var deleteEmail = _emailService.Delete(email.Data);
                if (deleteEmail.Result == false)
                    return new ErrorServiceResult(false, "StaffEmailNotDeleted");

                var deleteStaffEmail = Delete(staffEmail);
                if (deleteStaffEmail.Result == false)
                    return new ErrorServiceResult(false, "StaffEmailNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        private ServiceResult CheckIfEmailExists(StaffEmail staffEmail)
        {
            var result = _staffEmailDal.GetAll(x => x.StaffId == staffEmail.StaffId && x.EmailId == staffEmail.EmailId);

            if (result.Count>1)
                new ErrorServiceResult(false,"EmailAlreadyExists");

            return new ServiceResult(true, "");
        }

       // [SecuredOperation("admin,staff.saved")]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<StaffEmail> Save(StaffEmailDto staffEmailDto)
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

            return new SuccessDataServiceResult<StaffEmail>(true, "Saved");
        }
    }
}
