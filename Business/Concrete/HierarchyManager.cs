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
    public class HierarchyManager : IHierarchyService
    {
        private IHierarchyDal _hierarchyDal;

        public HierarchyManager(IHierarchyDal hierarchyDal)
        {
            _hierarchyDal = hierarchyDal;
        }

        public IDataResult<List<Hierarchy>> GetAll()
        {
            return new SuccessDataResult<List<Hierarchy>>(true, "Listed", _hierarchyDal.GetAll());
        }

        public IDataResult<Hierarchy> GetById(int hierarchyId)
        {
            return new SuccessDataResult<Hierarchy>(true, "Listed", _hierarchyDal.Get(p => p.Id == hierarchyId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(HierarchyValidator))]
        [TransactionScopeAspect]
        public IResult Add(Hierarchy hierarchy)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(hierarchy));

            if (result != null)
                return result;

            _hierarchyDal.Add(hierarchy);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(HierarchyValidator))]
        [TransactionScopeAspect]
        public IResult Update(Hierarchy hierarchy)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(hierarchy));

            if (result != null)
                return result;

            _hierarchyDal.Add(hierarchy);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Hierarchy hierarchy)
        {
            _hierarchyDal.Delete(hierarchy);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfCodeExists(Hierarchy hierarchy)
        {
            var result = _hierarchyDal.GetAll(x => x.Code == hierarchy.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
