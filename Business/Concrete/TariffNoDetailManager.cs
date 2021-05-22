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
    public class TariffNoDetailManager : ITariffNoDetailService
    {
        private ITariffNoDetailDal _tariffNoDetailDal;

        public TariffNoDetailManager(ITariffNoDetailDal tariffNoDetailDal)
        {
            _tariffNoDetailDal = tariffNoDetailDal;
        }

        public IDataServiceResult<List<TariffNoDetail>> GetAll(int customerId)
        {
            var dbResult = _tariffNoDetailDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<TariffNoDetail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<TariffNoDetail>> GetAllByTariffNo(int tariffNoId)
        {
            var dbResult = _tariffNoDetailDal.GetAll(x => x.TariffNoId == tariffNoId);

            return new SuccessDataServiceResult<List<TariffNoDetail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<TariffNoDetail>> GetAllBySeasonId(int customerId, int seasonId)
        {
            var dbResult = _tariffNoDetailDal.GetAll(x => x.CustomerId == customerId && x.SeasonCurrencyId == seasonId);

            return new SuccessDataServiceResult<List<TariffNoDetail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<TariffNoDetail>> GetAllBySeasonCurrencyId(int customerId, int seasonCurrencyId)
        {
            var dbResult = _tariffNoDetailDal.GetAll(x => x.CustomerId == customerId && x.SeasonCurrencyId == seasonCurrencyId);

            return new SuccessDataServiceResult<List<TariffNoDetail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<TariffNoDetail>> GetAllByCountryId(int customerId, int countryId)
        {
            var dbResult = _tariffNoDetailDal.GetAll(x => x.CustomerId == customerId && x.CountryId == countryId);

            return new SuccessDataServiceResult<List<TariffNoDetail>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<TariffNoDetail> GetById(int tariffNoDetailId)
        {
            var dbResult = _tariffNoDetailDal.Get(p => p.Id == tariffNoDetailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<TariffNoDetail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<TariffNoDetail>(dbResult, true, "Listed");
        }

        public IDataServiceResult<TariffNoDetail> GetBySeasonId(int seasonId)
        {
            var dbResult = _tariffNoDetailDal.Get(p => p.SeasonId == seasonId);
            if (dbResult == null)
                return new SuccessDataServiceResult<TariffNoDetail>(false, "Error_SystemError");

            return new SuccessDataServiceResult<TariffNoDetail>(dbResult, true, "Listed");
        }

        public IServiceResult Add(TariffNoDetail tariffNoDetail)
        {
            ServiceResult result = BusinessRules.Run(CheckIfSeasonCountryExists(tariffNoDetail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _tariffNoDetailDal.Add(tariffNoDetail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(TariffNoDetail tariffNoDetail)
        {
            ServiceResult result = BusinessRules.Run(CheckIfSeasonCountryExists(tariffNoDetail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _tariffNoDetailDal.Update(tariffNoDetail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(TariffNoDetail tariffNoDetail)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<TariffNoDetail>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            ServiceResult result = BusinessRules.Run(CheckIfTariffNoDetailIsUsed(tariffNoDetail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _tariffNoDetailDal.Delete(tariffNoDetail);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteByTariffNo(TariffNo tariffNo)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<TariffNoDetail>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var tariffNoDetails = GetAllByTariffNo(tariffNo.Id);
            if (tariffNoDetails.Result == true)
            {
                foreach (var tariffNoDetail in tariffNoDetails.Data)
                {
                    ServiceResult result = BusinessRules.Run(CheckIfTariffNoDetailIsUsed(tariffNoDetail));
                    if (result.Result == false)
                        return new ErrorServiceResult(false, result.Message);

                }

                foreach (var tariffNoDetail in tariffNoDetails.Data)
                {

                    var deleteResult = _tariffNoDetailDal.Delete(tariffNoDetail);
                    if (deleteResult == false)
                        return new ErrorServiceResult(false, "Error_SystemError");
                }
            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(TariffNoDetailValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<TariffNoDetail> Save(int tariffNoId, int customerId, List<TariffNoDetail> tariffNoDetails)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<TariffNoDetail>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var dbTariffNoDetails = GetAllByTariffNo(tariffNoId).Data;
            foreach (var dbTariffNoDetail in dbTariffNoDetails)
            {
                var control = tariffNoDetails.Any(x => x.Id == dbTariffNoDetail.Id);
                if (control != true)
                {
                    Delete(dbTariffNoDetail);
                }
            }

            foreach (var tariffNoDetail in tariffNoDetails)
            {
                tariffNoDetail.CustomerId = customerId;
                if (tariffNoDetail.Id > 0)
                {
                    Update(tariffNoDetail);
                }
                else
                {
                    Add(tariffNoDetail);
                }
            }
            return new SuccessDataServiceResult<TariffNoDetail>(true, "Saved");
        }

        private ServiceResult CheckIfSeasonCountryExists(TariffNoDetail tariffNoDetail)
        {
            var result = _tariffNoDetailDal.GetAll(x => x.CustomerId == tariffNoDetail.CustomerId && x.SeasonId == tariffNoDetail.SeasonId && x.CountryId == tariffNoDetail.CountryId);

            if (result.Count > 1)
                return new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfTariffNoDetailIsUsed(TariffNoDetail tariffNoDetail)
        {
            //var result = GetById(tariffNoDetail.Id);
            //if (result.Result == true)
            //    if (result.Data.IsUsed == true)
            //        return new ErrorServiceResult(false, "TariffNoDetailIsUsed");

            return new ServiceResult(true, "");
        }


    }
    }
}
