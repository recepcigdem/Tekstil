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
using Entities.Concrete.Dtos.Staff;

namespace Business.Concrete
{
    public class StaffAuthorizationManager : IStaffAuthorizationService
    {
        private IStaffAuthorizationDal _staffAuthorizationDal;

        public StaffAuthorizationManager(IStaffAuthorizationDal staffAuthorizationDal)
        {
            _staffAuthorizationDal = staffAuthorizationDal;
        }

        public IDataServiceResult<List<StaffAuthorization>> GetAll(int customerId)
        {
            var dbResult = _staffAuthorizationDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<StaffAuthorization>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<StaffAuthorization>> GetAllByStaffId(int staffId)
        {
            var dbResult = _staffAuthorizationDal.GetAll(x => x.StaffId == staffId);

            return new SuccessDataServiceResult<List<StaffAuthorization>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<StaffAuthorization>> GetAllByAuthorizationId(int authorizationId)
        {
            var dbResult = _staffAuthorizationDal.GetAll(x => x.AuthorizationId == authorizationId);

            return new SuccessDataServiceResult<List<StaffAuthorization>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<StaffAuthorization> GetById(int staffAuthorizationId)
        {
            var dbResult = _staffAuthorizationDal.Get(p => p.Id == staffAuthorizationId);
            if (dbResult == null)
                return new SuccessDataServiceResult<StaffAuthorization>(false, "Error_SystemError");

            return new SuccessDataServiceResult<StaffAuthorization>(dbResult, true, "Listed");
        }

        public IServiceResult GetByStaffId(int staffId)
        {
            var dbResult = _staffAuthorizationDal.Get(p => p.StaffId == staffId);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult( true, "Listed", dbResult.Id);
        }

        public IServiceResult Add(StaffAuthorization staffAuthorization)
        {
            IServiceResult result = BusinessRules.Run(CheckIfAuthorizationExists(staffAuthorization));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffAuthorizationDal.Add(staffAuthorization);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(StaffAuthorization staffAuthorization)
        {
            IServiceResult result = BusinessRules.Run(CheckIfAuthorizationExists(staffAuthorization));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _staffAuthorizationDal.Update(staffAuthorization);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(StaffAuthorization staffAuthorization)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<StaffAuthorization>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _staffAuthorizationDal.Delete(staffAuthorization);
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
                return new DataServiceResult<StaffAuthorization>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var staffAuth = GetAllByStaffId(staff.Id);
            if (staffAuth.Result == false)
                return new ErrorServiceResult(false, "StaffAuthorizationNotFound");

            foreach (var authorization in staffAuth.Data)
            {
                var delete = Delete(authorization);
                if (delete.Result == false)
                    return new ErrorServiceResult(false, "StaffAuthorizationNotFound");
            }

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(StaffAuthorizationValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<StaffAuthorization> Save(Staff staff, List<StaffAuthorization> staffAuthorizations)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<StaffAuthorization>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            DeleteByStaff(staff);

            foreach (var staffAuthorization in staffAuthorizations)
            {
                staffAuthorization.CustomerId = staff.CustomerId;
                staffAuthorization.StaffId = staff.Id;

                if (staffAuthorization.Id > 0)
                {
                    var result = Update(staffAuthorization);
                    if (result.Result == false)
                        return new DataServiceResult<StaffAuthorization>(false, result.Message);
                }
                else
                {
                    var result = Add(staffAuthorization);
                    if (result.Result == false)
                        return new DataServiceResult<StaffAuthorization>(false, result.Message);
                }
            }

            return new SuccessDataServiceResult<StaffAuthorization>(true, "Saved");
        }

        private ServiceResult CheckIfAuthorizationExists(StaffAuthorization staffAuthorization)
        {
            var result = _staffAuthorizationDal.GetAll(x => x.StaffId == staffAuthorization.StaffId && x.AuthorizationId == staffAuthorization.AuthorizationId);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "AuthorizationAlreadyExists");

            return new ServiceResult(true, "");
        }

    }
}
