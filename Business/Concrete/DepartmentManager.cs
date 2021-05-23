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

namespace Business.Concrete
{
    public class DepartmentManager : IDepartmentService
    {
        private IDepartmentDal _departmentDal;
        private IStaffService _staffService;

        public DepartmentManager(IDepartmentDal departmentDal, IStaffService staffService)
        {
            _departmentDal = departmentDal;
            _staffService = staffService;
        }

        public IDataServiceResult<List<Department>> GetAll(int customerId)
        {
            var dbResult = _departmentDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Department>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Department>> GetAllByActiveCustomerId(int customerId)
        {
            var dbResult = _departmentDal.GetAll(x => x.CustomerId == customerId && x.IsActive == true);

            return new SuccessDataServiceResult<List<Department>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Department> GetById(int departmentId)
        {
            var dbResult = _departmentDal.Get(p => p.Id == departmentId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Department>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Department>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Department department)
        {
            IServiceResult result = BusinessRules.Run(CheckIfDepartmentExists(department));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _departmentDal.Add(department);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(Department department)
        {
            IServiceResult result = BusinessRules.Run(CheckIfDepartmentExists(department));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _departmentDal.Update(department);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Department department)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Department>(false, message);
            }

            #endregion

            IServiceResult isUsedResult = BusinessRules.Run(CheckIfDepartmentIsUsed(department));
            if (isUsedResult.Result == false)
                return new ErrorServiceResult(false, isUsedResult.Message);

            var result = _departmentDal.Delete(department);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Department> Save(Department department)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Department>(false, message);
            }

            #endregion

            if (department.Id > 0)
            {
                var result = Update(department);
                if (result.Result == false)
                    return new DataServiceResult<Department>(false, result.Message);
            }
            else
            {
                var result = Add(department);
                if (result.Result == false)
                    return new DataServiceResult<Department>(false, result.Message);
            }

            return new SuccessDataServiceResult<Department>(true, "Saved");
        }

        private ServiceResult CheckIfDepartmentExists(Department department)
        {
            var result = _departmentDal.GetAll(x => x.DepartmentName == department.DepartmentName);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "DepartmentAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfDepartmentIsUsed(Department department)
        {
            var result = _staffService.GetAllByDepartmentId(department.CustomerId, department.Id);
            if (result.Result)
                return new ErrorServiceResult(false, "Message_DepartmentIsUsedStaff");

            return new ServiceResult(true, "");
        }
    }
}
