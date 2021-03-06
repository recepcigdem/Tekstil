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
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Staff;

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

        public IDataServiceResult<List<StaffEmail>> GetAll(int customerId)
        {
            var dbResult = _staffEmailDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<StaffEmail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffEmail> GetById(int staffEmailId)
        {
            var dbResult = _staffEmailDal.Get(p => p.Id == staffEmailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffEmail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<StaffEmail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffEmail> GetByEmailId(int emailId)
        {
            var dbResult = _staffEmailDal.Get(p => p.EmailId == emailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffEmail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<StaffEmail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<StaffEmail>> GetAllByStaffId(int staffId)
        {
            var dbResult = _staffEmailDal.GetAll(x => x.StaffId == staffId);

            return new SuccessDataServiceResult<List<StaffEmail>>(dbResult, true, "Listed");
        }

        public IServiceResult Add(StaffEmail staffEmail)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffEmailDal.Add(staffEmail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(StaffEmail staffEmail)
        {
            IServiceResult result = BusinessRules.Run(CheckIfEmailExists(staffEmail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffEmailDal.Update(staffEmail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(StaffEmail staffEmail)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<StaffEmail>(false, message);
            }

            #endregion

            var result = _staffEmailDal.Delete(staffEmail);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteByStaff(Staff staff)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new ErrorServiceResult(false, message);
            }

            #endregion

            var staffEmailList = GetAllByStaffId(staff.Id);
            if (staffEmailList.Result == false)
                return new ErrorServiceResult(false, "StaffEmailNotFound");

            foreach (var staffEmail in staffEmailList.Data)
            {
                var deleteStaffEmail = Delete(staffEmail);
                if (deleteStaffEmail.Result == false)
                    return new ErrorServiceResult(false, "StaffEmailNotDeleted");

                var email = _emailService.GetById(staffEmail.EmailId);
                if (email.Result == false)
                    return new ErrorServiceResult(false, "StaffEmailNotFound");

                var deleteEmail = _emailService.Delete(email.Data);
                if (deleteEmail.Result == false)
                    return new ErrorServiceResult(false, "StaffEmailNotDeleted");

            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(StaffEmailValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<StaffEmail> Save(Staff staff, List<StaffEmailDto> staffEmailDtos)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<StaffEmail>(false, message);
            }

            #endregion

            DeleteByStaff(staff);

            foreach (var staffEmailDto in staffEmailDtos)
            {
                Email email = new Email
                {
                    CustomerId = staff.CustomerId,
                    IsActive = staffEmailDto.IsActive,
                    EmailAddress = staffEmailDto.EmailAddress
                };

                _emailService.Add(email);

                StaffEmail staffEmail = new StaffEmail
                {
                    CustomerId = staff.CustomerId,
                    StaffId = staff.Id,
                    EmailId = email.Id,
                    IsMain = staffEmailDto.IsMain
                };

                Add(staffEmail);
            }


            return new SuccessDataServiceResult<StaffEmail>(true, "Saved");
        }

        private ServiceResult CheckIfEmailExists(StaffEmail staffEmail)
        {
            var result = _staffEmailDal.GetAll(x => x.StaffId == staffEmail.StaffId && x.EmailId == staffEmail.EmailId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "EmailAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
