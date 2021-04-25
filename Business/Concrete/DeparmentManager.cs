﻿using System;
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

namespace Business.Concrete
{
    public class DepartmentManager : IDepartmentService
    {
        private IDepartmentDal _departmentDal;

        public DepartmentManager(IDepartmentDal departmentDal)
        {
            _departmentDal = departmentDal;
        }

        public IDataServiceResult<List<Department>> GetAll()
        {
            var dbResult = _departmentDal.GetAll();

            return new SuccessDataServiceResult<List<Department>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Department> GetById(int departmentId)
        {
            var dbResult = _departmentDal.Get(p => p.Id == departmentId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Department>(false, "SystemError");

            return new SuccessDataServiceResult<Department>(dbResult, true, "Listed");
        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Department department)
        {
            IServiceResult result = BusinessRules.Run(CheckIfDepartmentExists(department));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _departmentDal.Add(department);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");

        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Department department)
        {
            IServiceResult result = BusinessRules.Run(CheckIfDepartmentExists(department));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _departmentDal.Update(department);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }

        // [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Department department)
        {
            var result = _departmentDal.Delete(department);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Department> Save(Department department)
        {
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
    }
}
