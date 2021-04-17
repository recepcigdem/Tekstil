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

namespace Business.Concrete
{
    public class DepartmentManager : IDepartmentService
    {
        private IDepartmentDal _departmentDal;

        public DepartmentManager(IDepartmentDal departmentDal)
        {
            _departmentDal = departmentDal;
        }

        public IDataResult<List<Department>> GetAll()
        {
            return new SuccessDataResult<List<Department>>(true, "Listed", _departmentDal.GetAll());
        }

        public IDataResult<Department> GetById(int departmentId)
        {
            return new SuccessDataResult<Department>(true, "Listed", _departmentDal.Get(p => p.Id == departmentId));
        }

        [SecuredOperation("admin,department.add")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IResult Add(Department department)
        {
            IResult result = BusinessRules.Run(CheckIfDepartmentNameExists(department));

            if (result != null)
                return result;

            _departmentDal.Add(department);

            return new SuccessResult(true, "Added");

        }

        [SecuredOperation("admin,department.updated")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IResult Update(Department department)
        {
            IResult result = BusinessRules.Run(CheckIfDepartmentNameExists(department));

            if (result != null)
                return result;

            _departmentDal.Update(department);

            return new SuccessResult(true, "Updated");
        }

        [SecuredOperation("admin,department.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Department department)
        {
            _departmentDal.Delete(department);

            return new SuccessResult(true, "Deleted");
        }

        private IResult CheckIfDepartmentNameExists(Department department)
        {
            var result = _departmentDal.GetAll(x => x.DepartmentName == department.DepartmentName).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        [SecuredOperation("admin,department.saved")]
        [ValidationAspect(typeof(DepartmentValidator))]
        [TransactionScopeAspect]
        public IDataResult<Department> Save(Department department)
        {
            if (department.Id>0)
            {
                Update(department);
            }
            else
            {
                Add(department);
            }
            return new SuccessDataResult<Department>(true,"Saved",department);
        }
    }
}
