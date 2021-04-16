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

        public IDataResult<List<StaffPhone>> GetAll()
        {
            return new SuccessDataResult<List<StaffPhone>>(true, "Listed", _staffPhoneDal.GetAll());
        }

        public IDataResult<StaffPhone> GetById(int staffPhoneId)
        {
            return new SuccessDataResult<StaffPhone>(true, "Listed", _staffPhoneDal.Get(p => p.Id == staffPhoneId));
        }

        [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IResult Add(StaffPhone staffPhone)
        {
            IResult result = BusinessRules.Run(CheckIfPhoneExists(staffPhone));

            if (result != null)
                return result;

            _staffPhoneDal.Add(staffPhone);

            return new SuccessResult(true, "Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IResult Update(StaffPhone staffPhone)
        {
            IResult result = BusinessRules.Run(CheckIfPhoneExists(staffPhone));

            if (result != null)
                return result;

            _staffPhoneDal.Update(staffPhone);

            return new SuccessResult(true, "Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(StaffPhone staffPhone)
        {
            _staffPhoneDal.Delete(staffPhone);

            return new SuccessResult(true, "Deleted");
        }

        private IResult CheckIfPhoneExists(StaffPhone staffPhone)
        {
            var result = _staffPhoneDal.GetAll(x => x.StaffId == staffPhone.StaffId && x.PhoneId == staffPhone.PhoneId).Any();

            if (result)
                new ErrorResult( "PhoneAlreadyExists");

            return new SuccessResult();
        }
        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult DeleteByStaffIdWithPhone(int staffId)
        {
            var result = _staffPhoneDal.Get(sp => sp.StaffId == staffId);
            if (result==null)
                new ErrorResult(false, "PhoneNotFound");

            _staffPhoneDal.DeleteByFilter(sp=>sp.StaffId==staffId);

            _phoneService.DeleteByPhoneId(result.PhoneId);

            return new SuccessResult(true, "Deleted");
        }
        [SecuredOperation("admin,staff.saved")]
        [ValidationAspect(typeof(StaffPhoneValidator))]
        [TransactionScopeAspect]
        public IResult Save(StaffPhoneDto staffPhoneDto)
        {
            StaffPhone staffPhone = new StaffPhone
            {
                Id=staffPhoneDto.Id,
                StaffId = staffPhoneDto.StaffId,
                PhoneId = staffPhoneDto.PhoneId,
                IsMain = staffPhoneDto.IsMain
            };

            Phone phone = new Phone
            {
                Id = staffPhoneDto.PhoneId,
                IsActive = staffPhoneDto.IsActive,
                CountryCode = staffPhoneDto.CountryCode,
                AreaCode = staffPhoneDto.AreaCode,
                PhoneNumber = staffPhoneDto.PhoneNumber
            };

            if (staffPhone.Id>0)
            {
                Update(staffPhone);
            }
            else
            {
                Add(staffPhone);
            }

            _phoneService.Save(phone);

            return new SuccessResult(true,"Saved");
        }
    }
}
