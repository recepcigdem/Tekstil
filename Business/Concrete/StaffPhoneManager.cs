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
    public class StaffPhoneManager : IStaffPhoneService
    {
        private IStaffPhoneDal _staffPhoneDal;
        private IPhoneService _phoneService;

        public StaffPhoneManager(IStaffPhoneDal staffPhoneDal, IPhoneService phoneService)
        {
            _staffPhoneDal = staffPhoneDal;
            _phoneService = phoneService;
        }

        public IDataServiceResult<List<StaffPhone>> GetAll()
        {
            var dbResult = _staffPhoneDal.GetAll();

            return new SuccessDataServiceResult<List<StaffPhone>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffPhone> GetById(int staffPhoneId)
        {
            var dbResult = _staffPhoneDal.Get(p => p.Id == staffPhoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffPhone>(false, "SystemError");

            return new SuccessDataServiceResult<StaffPhone>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffPhone> GetByPhoneId(int phoneId)
        {
            var dbResult = _staffPhoneDal.Get(p => p.PhoneId == phoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffPhone>(false, "SystemError");

            return new SuccessDataServiceResult<StaffPhone>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<StaffPhone>> GetAllByStaffId(int staffId)
        {
            var dbResult = _staffPhoneDal.GetAll(x => x.StaffId == staffId);

            return new SuccessDataServiceResult<List<StaffPhone>>(dbResult, true, "Listed");
        }

        //[SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(StaffPhone staffPhone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(staffPhone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffPhoneDal.Add(staffPhone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");

        }

        //[SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(StaffPhone staffPhone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(staffPhone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffPhoneDal.Update(staffPhone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }

        //[SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(StaffPhone staffPhone)
        {
            var result = _staffPhoneDal.Delete(staffPhone);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IServiceResult DeleteByStaff(Staff staff)
        {
            var staffPhoneList = GetAllByStaffId(staff.Id);
            if (staffPhoneList.Result == false)
                return new ErrorServiceResult(false, "StaffPhoneNotFound");

            foreach (var staffPhone in staffPhoneList.Data)
            {
                var phone = _phoneService.GetById(staffPhone.PhoneId);
                if (phone.Result == false)
                    return new ErrorServiceResult(false, "StaffPhoneNotFound");

                var deletePhone = _phoneService.Delete(phone.Data);
                if (deletePhone.Result == false)
                    return new ErrorServiceResult(false, "StaffPhoneNotDeleted");

                var deleteStaffPhone = Delete(staffPhone);
                if (deleteStaffPhone.Result == false)
                    return new ErrorServiceResult(false, "StaffPhoneNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        private ServiceResult CheckIfPhoneExists(StaffPhone staffPhone)
        {
            var result = _staffPhoneDal.GetAll(x => x.StaffId == staffPhone.StaffId && x.PhoneId == staffPhone.PhoneId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "PhoneAlreadyExists");

            return new ServiceResult(true, "");
        }

       // [SecuredOperation("admin,staff.saved")]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<StaffPhone> Save(StaffPhoneDto staffPhoneDto)
        {
            StaffPhone staffPhone = new StaffPhone
            {
                Id = staffPhoneDto.Id,
                StaffId = staffPhoneDto.StaffId,
                PhoneId = staffPhoneDto.PhoneId,
                IsMain = staffPhoneDto.IsMain
            };

            Phone phone = new Phone
            {
                Id = staffPhone.PhoneId,
                IsActive = staffPhoneDto.IsActive,
                CountryCode = staffPhoneDto.CountryCode,
                AreaCode = staffPhoneDto.AreaCode,
                PhoneNumber = staffPhoneDto.PhoneNumber
            };

            if (staffPhoneDto.Id > 0)
            {
                Update(staffPhone);

            }
            else
            {
                Add(staffPhone);
            }
            _phoneService.Save(phone);

            return new SuccessDataServiceResult<StaffPhone>(true, "Saved");
        }
    }
}
