﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
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

        public IDataServiceResult<List<Staff>> GetAll()
        {
            var dbResult = _staffDal.GetAll();

            return new SuccessDataServiceResult<List<Staff>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Staff> GetById(int staffId)
        {
            var dbResult = _staffDal.Get(p => p.Id == staffId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Staff>(false, "SystemError");

            return new SuccessDataServiceResult<Staff>(dbResult, true, "Listed");
        }

        //   [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(StaffValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Staff staff)
        {
            ServiceResult result = BusinessRules.Run(CheckIfExists(staff));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffDal.Add(staff);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }

        //   [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(StaffValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Staff staff)
        {
            ServiceResult result = BusinessRules.Run(CheckIfExists(staff));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffDal.Update(staff);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        //[SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Staff staff)
        {
            var result = _staffDal.Delete(staff);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        public IServiceResult DeleteAll(Staff staff)
        {

            var staffAuth = _staffAuthorizationService.DeleteByStaff(staff);
            if (staffAuth.Result == false)
                return new ErrorServiceResult(false, "StaffAuthorizationNotFound");

            var staffEmail = _staffEmailService.DeleteByStaff(staff);
            if (staffEmail.Result == false)
                return new ErrorServiceResult(false, "StaffEmailNotFound");

            var staffPhone = _staffPhoneService.DeleteByStaff(staff);
            if (staffPhone.Result == false)
                return new ErrorServiceResult(false, "StaffPhoneNotFound");


            var staffDelete = _staffDal.Delete(staff);
            if (staffDelete == false)
                return new ErrorServiceResult(false, "SystemError");



            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<Staff> Save(Staff staff)
        {
            if (staff.Id > 0)
            {
                var result = Update(staff);
                if (result.Result == false)
                    return new DataServiceResult<Staff>(false, result.Message);
            }
            else
            {
                var result = Add(staff);
                if (result.Result == false)
                    return new DataServiceResult<Staff>(false, result.Message);
            }

            return new SuccessDataServiceResult<Staff>(true, "Saved");
        }

        public IDataServiceResult<Staff> SaveAll(Staff staff, List<StaffEmailDto> staffEmailDtos, List<StaffPhoneDto> staffPhoneDtos, List<StaffAuthorization> staffAuthorizations, string password)
        {
            if (staff.Id > 0)
            {
                Update(staff);
            }
            else
            {
                Add(staff);
            }

            _staffEmailService.Save(staff, staffEmailDtos);

            _staffPhoneService.Save(staff, staffPhoneDtos);

            _staffAuthorizationService.Save(staff, staffAuthorizations);


            return new SuccessDataServiceResult<Staff>(staff, true, "Saved");
        }

        private ServiceResult CheckIfExists(Staff staff)
        {
            var result = _staffDal.GetAll(x => x.FirstName == staff.FirstName && x.LastName == staff.LastName);

            if (result.Count > 1)
                new ErrorServiceResult(false, "AlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
