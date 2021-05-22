using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Interceptors;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class DefinitionManager : IDefinitionService
    {
        private IDefinitionDal _definitionDal;
        private ICountryShippingMultiplierService _countryShippingMultiplierService;
        private IHierarchyService _hierarchyService;
        private IModelSeasonRowNumberService _modelSeasonRowNumberService;
        private IPaymentMethodShareService _paymentMethodShareService;
        private ISeasonPlaningService _seasonPlaningService;
        private ITariffNoDetailService _tariffNoDetailService;

        public DefinitionManager(IDefinitionDal definitionDal, ICountryShippingMultiplierService countryShippingMultiplierService, IHierarchyService hierarchyService, IModelSeasonRowNumberService modelSeasonRowNumberService, IPaymentMethodShareService paymentMethodShareService, ISeasonPlaningService seasonPlaningService, ITariffNoDetailService tariffNoDetailService)
        {
            _definitionDal = definitionDal;
            _countryShippingMultiplierService = countryShippingMultiplierService;
            _hierarchyService = hierarchyService;
            _modelSeasonRowNumberService = modelSeasonRowNumberService;
            _paymentMethodShareService = paymentMethodShareService;
            _seasonPlaningService = seasonPlaningService;
            _tariffNoDetailService = tariffNoDetailService;
        }

        public IDataServiceResult<List<Definition>> GetAll(int customerId)
        {
            var dbResult = _definitionDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Definition>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<Definition>> GetAllByCustomerIdAndDefinitionTitleId(int customerId, int definitionTitleId)
        {
            var dbResult = _definitionDal.GetAll(x => x.CustomerId == customerId && x.DefinitionTitleId == definitionTitleId);

            return new SuccessDataServiceResult<List<Definition>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Definition> GetById(int definitionId)
        {
            var dbResult = _definitionDal.Get(p => p.Id == definitionId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Definition>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Definition>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Definition> GetByDefinitionTitleId(int definitionTitleId)
        {
            var dbResult = _definitionDal.Get(p => p.DefinitionTitleId == definitionTitleId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Definition>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Definition>(dbResult, true, "Listed");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IDataServiceResult<Definition> Save(Definition definition)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Definition>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            if (definition.Id > 0)
            {
                var result = _definitionDal.Update(definition);
                if (result == null)
                    return new DataServiceResult<Definition>(false, "Error_SystemError");
            }
            else
            {
                var result = _definitionDal.Add(definition);
                if (result == null)
                    return new DataServiceResult<Definition>(false, "Error_SystemError");
            }

            return new SuccessDataServiceResult<Definition>(true, "Saved");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Definition definition)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<Definition>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            IServiceResult isUsedResult = BusinessRules.Run(CheckIfDefinitionIsUsed(definition));
            if (isUsedResult.Result == false)
                return new ErrorServiceResult(false, isUsedResult.Message);


            var result = _definitionDal.Delete(definition);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        private ServiceResult CheckIfDefinitionIsUsed(Definition definition)
        {
            var category = _definitionDal.GetAll(x => x.CategoryId == definition.Id);
            if (category.Count > 0)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedDefinitionCategory");

            var productGroup = _definitionDal.GetAll(x => x.ProductGroupId == definition.Id);
            if (productGroup.Count > 0)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedDefinitionProductGroup");

            var countryShippingMultiplierShippingMethod = _countryShippingMultiplierService.GetAllByShippingMethodId(definition.CustomerId, definition.Id);
            if (countryShippingMultiplierShippingMethod.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedCountryShippingMultiplierShippingMethod");

            var countryShippingMultiplierCountry = _countryShippingMultiplierService.GetAllByCountryId(definition.CustomerId, definition.Id);
            if (countryShippingMultiplierCountry.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedCountryShippingMultiplierCountry");

            var hierarchyBrand = _hierarchyService.GetAllByBrandId(definition.CustomerId, definition.Id);
            if (hierarchyBrand.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedHierarchyBrand");

            var hierarchyGender = _hierarchyService.GetAllByGenderId(definition.CustomerId, definition.Id);
            if (hierarchyGender.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedHierarchyGender");

            var hierarchyMainProductGroup = _hierarchyService.GetAllByMainProductGroupId(definition.CustomerId, definition.Id);
            if (hierarchyMainProductGroup.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedHierarchyMainProductGroup");

            var hierarchyDetail = _hierarchyService.GetAllByDetailId(definition.CustomerId, definition.Id);
            if (hierarchyDetail.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedHierarchyDetail");

            var hierarchyProductGroup = _hierarchyService.GetAllByProductGroupId(definition.CustomerId, definition.Id);
            if (hierarchyProductGroup.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedHierarchyProductGroup");

            var hierarchySubProductGroup = _hierarchyService.GetAllBySubProductGroupId(definition.CustomerId, definition.Id);
            if (hierarchySubProductGroup.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedHierarchySubProductGroup");

            var modelSeasonRowNumberProductGroup = _modelSeasonRowNumberService.GetAllByProductGroupId(definition.CustomerId, definition.Id);
            if (modelSeasonRowNumberProductGroup.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedModelSeasonRowNumberProductGroup");

            var paymentMethodSharePaymentMethod = _paymentMethodShareService.GetAllByPaymentMethodId(definition.CustomerId, definition.Id);
            if (paymentMethodSharePaymentMethod.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedPaymentMethodSharePaymentMethod");

            var seasonPlanningProductGroup = _seasonPlaningService.GetAllByProductGroupId(definition.CustomerId, definition.Id);
            if (seasonPlanningProductGroup.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedSeasonPlanningProductGroup");

            var tariffNoDetailCountry = _tariffNoDetailService.GetAllByCountryId(definition.CustomerId, definition.Id);
            if (tariffNoDetailCountry.Result)
                return new ErrorServiceResult(false, "Message_DefinitionIsUsedTariffNoDetailCountry");

            return new ServiceResult(true, "");
        }

    }
}
