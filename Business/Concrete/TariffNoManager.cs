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
    public class TariffNoManager : ITariffNoService
    {
        private ITariffNoDal _tariffNoDal;
        private ITariffNoDetailService _tariffNoDetailService;
        public TariffNoManager(ITariffNoDal tariffNoDal, ITariffNoDetailService tariffNoDetailService)
        {
            _tariffNoDal = tariffNoDal;
            _tariffNoDetailService = tariffNoDetailService;
        }

        public IDataServiceResult<List<TariffNo>> GetAll(int customerId)
        {
            var dbResult = _tariffNoDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<TariffNo>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<TariffNo> GetById(int tariffNoId)
        {
            var dbResult = _tariffNoDal.Get(p => p.Id == tariffNoId);
            if (dbResult == null)
                return new SuccessDataServiceResult<TariffNo>(false, "Error_SystemError");

            return new SuccessDataServiceResult<TariffNo>(dbResult, true, "Listed");
        }

        public IServiceResult Add(TariffNo tariffNo)
        {
            ServiceResult result = BusinessRules.Run(CheckIfDescriptionExists(tariffNo), CheckIfCodeExists(tariffNo));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _tariffNoDal.Add(tariffNo);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(TariffNo tariffNo)
        {
            ServiceResult result = BusinessRules.Run(CheckIfDescriptionExists(tariffNo), CheckIfCodeExists(tariffNo));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _tariffNoDal.Update(tariffNo);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(TariffNo tariffNo)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<TariffNo>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            ServiceResult result = BusinessRules.Run(CheckIfTariffNoIsUsed(tariffNo));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteTariffNoDetail = _tariffNoDetailService.DeleteByTariffNo(tariffNo);
            if (deleteTariffNoDetail.Result == false)
                return new ErrorServiceResult(false, deleteTariffNoDetail.Message);

            var deleteResult = _tariffNoDal.Delete(tariffNo);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(TariffNoValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<TariffNo> Save(TariffNo tariffNo, List<TariffNoDetail> tariffNoDetails)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<TariffNo>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            if (tariffNo.Id > 0)
            {
                Update(tariffNo);
            }
            else
            {
                Add(tariffNo);
            }

            _tariffNoDetailService.Save(tariffNo.Id, tariffNo.CustomerId, tariffNoDetails);

            return new SuccessDataServiceResult<TariffNo>(true, "Saved");
        }

        private ServiceResult CheckIfDescriptionExists(TariffNo tariffNo)
        {
            var result = _tariffNoDal.GetAll(x => x.CustomerId == tariffNo.CustomerId && x.Description == tariffNo.Description);

            if (result.Count > 1)
                new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfCodeExists(TariffNo tariffNo)
        {
            var result = _tariffNoDal.GetAll(x => x.CustomerId == tariffNo.CustomerId && x.Code == tariffNo.Code);

            if (result.Count > 1)
                new ErrorServiceResult(false, "CodeAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfTariffNoIsUsed(TariffNo tariffNo)
        {
            //var result = GetById(tariffNo.Id);
            //if (result.Result == true)
            //    if (result.Data.IsUsed == true)
            //        new ErrorServiceResult(false, "TariffNoIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
