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

        public IDataServiceResult<TariffNoDetail> GetById(int tariffNoDetailId)
        {
            var dbResult = _tariffNoDetailDal.Get(p => p.Id == tariffNoDetailId);
            if (dbResult == null)
                return new SuccessDataServiceResult<TariffNoDetail>(false, "SystemError");

            return new SuccessDataServiceResult<TariffNoDetail>(dbResult, true, "Listed");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [ValidationAspect(typeof(TariffNoDetailValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(TariffNoDetail tariffNoDetail)
        {
            ServiceResult result = BusinessRules.Run(CheckIfSeasonCountryExists(tariffNoDetail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _tariffNoDetailDal.Add(tariffNoDetail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [ValidationAspect(typeof(TariffNoDetailValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(TariffNoDetail tariffNoDetail)
        {
            ServiceResult result = BusinessRules.Run(CheckIfSeasonCountryExists(tariffNoDetail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _tariffNoDetailDal.Update(tariffNoDetail);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [TransactionScopeAspect]
        public IServiceResult Delete(TariffNoDetail tariffNoDetail)
        {
            ServiceResult result = BusinessRules.Run(CheckIfTariffNoDetailIsUsed(tariffNoDetail));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _tariffNoDetailDal.Delete(tariffNoDetail);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [TransactionScopeAspect]
        public IServiceResult DeleteByTariffNo(TariffNo tariffNo)
        {
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
                        return new ErrorServiceResult(false, "SystemError");
                }
            }

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<TariffNoDetail> Save(int tariffNoId,int customerId, List<TariffNoDetail> tariffNoDetails)
        {
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
                new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfTariffNoDetailIsUsed(TariffNoDetail tariffNoDetail)
        {
            var result = GetById(tariffNoDetail.Id);
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "TariffNoDetailIsUsed");

            return new ServiceResult(true, "");
        }


    }
}
