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
    public class StaffPhoneManager : IStaffPhoneService
    {
        private IStaffPhoneDal _staffPhoneDal;
        private IPhoneService _phoneService;

        public StaffPhoneManager(IStaffPhoneDal staffPhoneDal, IPhoneService phoneService)
        {
            _staffPhoneDal = staffPhoneDal;
            _phoneService = phoneService;
        }

        public IDataServiceResult<List<StaffPhone>> GetAll(int customerId)
        {
            var dbResult = _staffPhoneDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<StaffPhone>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffPhone> GetById(int staffPhoneId)
        {
            var dbResult = _staffPhoneDal.Get(p => p.Id == staffPhoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffPhone>(false, "Error_SystemError");

            return new SuccessDataServiceResult<StaffPhone>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffPhone> GetByPhoneId(int phoneId)
        {
            var dbResult = _staffPhoneDal.Get(p => p.PhoneId == phoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffPhone>(false, "Error_SystemError");

            return new SuccessDataServiceResult<StaffPhone>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<StaffPhone>> GetAllByStaffId(int staffId)
        {
            var dbResult = _staffPhoneDal.GetAll(x => x.StaffId == staffId);

            return new SuccessDataServiceResult<List<StaffPhone>>(dbResult, true, "Listed");
        }

        public IServiceResult Add(StaffPhone staffPhone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(staffPhone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffPhoneDal.Add(staffPhone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(StaffPhone staffPhone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(staffPhone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffPhoneDal.Update(staffPhone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(StaffPhone staffPhone)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<StaffPhone>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _staffPhoneDal.Delete(staffPhone);
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
                return new DataServiceResult<StaffPhone>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var staffPhoneList = GetAllByStaffId(staff.Id);
            if (staffPhoneList.Result == false)
                return new ErrorServiceResult(false, "StaffPhoneNotFound");

            foreach (var staffPhone in staffPhoneList.Data)
            {
                var deleteStaffPhone = Delete(staffPhone);
                if (deleteStaffPhone.Result == false)
                    return new ErrorServiceResult(false, "StaffPhoneNotDeleted");

                var phone = _phoneService.GetById(staffPhone.PhoneId);
                if (phone.Result == false)
                    return new ErrorServiceResult(false, "StaffPhoneNotFound");

                var deletePhone = _phoneService.Delete(phone.Data);
                if (deletePhone.Result == false)
                    return new ErrorServiceResult(false, "StaffPhoneNotDeleted");
            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<StaffPhone> Save(Staff staff, List<StaffPhoneDto> staffPhoneDtos)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<StaffPhone>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            DeleteByStaff(staff);

            foreach (var staffPhoneDto in staffPhoneDtos)
            {
                Phone phone = new Phone
                {
                    CustomerId = staff.CustomerId,
                    IsActive = staffPhoneDto.IsActive,
                    CountryCode = staffPhoneDto.CountryCode,
                    AreaCode = staffPhoneDto.AreaCode,
                    PhoneNumber = staffPhoneDto.PhoneNumber
                };

                _phoneService.Add(phone);

                StaffPhone staffPhone = new StaffPhone
                {
                    CustomerId = staff.CustomerId,
                    StaffId = staff.Id,
                    PhoneId = phone.Id,
                    IsMain = staffPhoneDto.IsMain
                };

                Add(staffPhone);
            }


            return new SuccessDataServiceResult<StaffPhone>(true, "Saved");
        }

        private ServiceResult CheckIfPhoneExists(StaffPhone staffPhone)
        {
            var result = _staffPhoneDal.GetAll(x => x.StaffId == staffPhone.StaffId && x.PhoneId == staffPhone.PhoneId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "PhoneAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
