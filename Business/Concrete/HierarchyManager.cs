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
    public class HierarchyManager : IHierarchyService
    {
        private IHierarchyDal _hierarchyDal;
        private IDefinitionService _definitionService;

        public HierarchyManager(IHierarchyDal hierarchyDal, IDefinitionService definitionService)
        {
            _hierarchyDal = hierarchyDal;
            _definitionService = definitionService;
        }

        public IDataServiceResult<List<Hierarchy>> GetAll(int customerId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Hierarchy>> GetAllByBrandId(int customerId, int brandId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId && x.BrandId == brandId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Hierarchy>> GetAllByGenderId(int customerId, int genderId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId && x.GenderId == genderId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Hierarchy>> GetAllByMainProductGroupId(int customerId, int mainProductGroupId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId && x.MainProductGroupId == mainProductGroupId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Hierarchy>> GetAllByDetailId(int customerId, int detailId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId && x.DetailId == detailId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Hierarchy>> GetAllByProductGroupId(int customerId, int productGroupId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId && x.ProductGroupId == productGroupId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Hierarchy>> GetAllBySubProductGroupId(int customerId, int subProductGroupId)
        {
            var dbResult = _hierarchyDal.GetAll(x => x.CustomerId == customerId && x.SubProductGroupId == subProductGroupId);

            return new SuccessDataServiceResult<List<Hierarchy>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Hierarchy> GetById(int hierarchyId)
        {
            var dbResult = _hierarchyDal.Get(p => p.Id == hierarchyId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Hierarchy>(false, "SystemError");

            return new SuccessDataServiceResult<Hierarchy>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Hierarchy hierarchy)
        {
            ServiceResult result = BusinessRules.Run(CheckIfHierarchyExists(hierarchy));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _hierarchyDal.Add(hierarchy);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(Hierarchy hierarchy)
        {
            ServiceResult result = BusinessRules.Run(CheckIfHierarchyExists(hierarchy));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _hierarchyDal.Update(hierarchy);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Hierarchy hierarchy)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Hierarchy>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            ServiceResult result = BusinessRules.Run(CheckIfHierarchyIsUsed(hierarchy));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _hierarchyDal.Delete(hierarchy);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(HierarchyValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Hierarchy> Save(Hierarchy hierarchy)
        {

            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Hierarchy>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var brand = _definitionService.GetById(hierarchy.BrandId);
            var gender = _definitionService.GetById(hierarchy.GenderId);
            var mainProductGroupId = _definitionService.GetById(hierarchy.MainProductGroupId);
            var detail = _definitionService.GetById(hierarchy.DetailId);
            var productGroupId = _definitionService.GetById(hierarchy.ProductGroupId);
            var subProductGroupId = _definitionService.GetById(hierarchy.SubProductGroupId);

            if (subProductGroupId.Result)
                hierarchy.TotalDescription = hierarchy.TotalDescription + subProductGroupId.Data.DescriptionTr + "-";
            if (productGroupId.Result)
                hierarchy.TotalDescription = hierarchy.TotalDescription + productGroupId.Data.DescriptionTr + "-";
            if (detail.Result)
                hierarchy.TotalDescription = hierarchy.TotalDescription + detail.Data.DescriptionTr + "-";
            if (mainProductGroupId.Result)
                hierarchy.TotalDescription = hierarchy.TotalDescription + mainProductGroupId.Data.DescriptionTr + "-";
            if (gender.Result)
                hierarchy.TotalDescription = hierarchy.TotalDescription + gender.Data.DescriptionTr + "-";
            if (brand.Result)
                hierarchy.TotalDescription = hierarchy.TotalDescription + brand.Data.DescriptionTr + "-";
            if (!string.IsNullOrEmpty(hierarchy.Description1))
                hierarchy.TotalDescription = hierarchy.TotalDescription + hierarchy.Description1 + "-";
            if (!string.IsNullOrEmpty(hierarchy.Description2))
                hierarchy.TotalDescription = hierarchy.TotalDescription + hierarchy.Description2 + "-";
            if (!string.IsNullOrEmpty(hierarchy.Description3))
                hierarchy.TotalDescription = hierarchy.TotalDescription + hierarchy.Description3 + "-";
            if (!string.IsNullOrEmpty(hierarchy.Description4))
                hierarchy.TotalDescription = hierarchy.TotalDescription + hierarchy.Description4 + "-";

            hierarchy.TotalDescription = hierarchy.TotalDescription.Substring(0, hierarchy.TotalDescription.Length - 1);

            if (hierarchy.Id > 0)
            {
                Update(hierarchy);
            }
            else
            {
                Add(hierarchy);
            }

            return new SuccessDataServiceResult<Hierarchy>(true, "Saved");
        }

        private ServiceResult CheckIfHierarchyExists(Hierarchy hierarchy)
        {
            var result = _hierarchyDal.GetAll(x => x.CustomerId == hierarchy.CustomerId && x.Code == hierarchy.Code);

            if (result.Count > 1)
                new ErrorServiceResult(false, "HierarchyAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfHierarchyIsUsed(Hierarchy hierarchy)
        {
            var result = GetById(hierarchy.Id);
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "HierarchyIsUsed");

            return new ServiceResult(true, "");
        }


    }
}
