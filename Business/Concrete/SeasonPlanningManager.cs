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
    public class SeasonPlaningManager : ISeasonPlaningService
    {
        private ISeasonPlaningDal _seasonPlaningDal;

        public SeasonPlaningManager(ISeasonPlaningDal seasonPlaningDal)
        {
            _seasonPlaningDal = seasonPlaningDal;
        }

        public IDataServiceResult<List<SeasonPlaning>> GetAll(int customerId)
        {
            var dbResult = _seasonPlaningDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<SeasonPlaning>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<SeasonPlaning>> GetAllBySeasonId(int seasonId)
        {
            var dbResult = _seasonPlaningDal.GetAll(x => x.SeasonId == seasonId);

            return new SuccessDataServiceResult<List<SeasonPlaning>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<SeasonPlaning> GetById(int seasonPlanningId)
        {
            var dbResult = _seasonPlaningDal.Get(p => p.Id == seasonPlanningId);
            if (dbResult == null)
                return new SuccessDataServiceResult<SeasonPlaning>(false, "SystemError");

            return new SuccessDataServiceResult<SeasonPlaning>(dbResult, true, "Listed");
        }

        public IServiceResult Add(SeasonPlaning seasonPlaning)
        {
            ServiceResult result = BusinessRules.Run(CheckIfPlaningTypeExists(seasonPlaning));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonPlaningDal.Add(seasonPlaning);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }
       
        public IServiceResult Update(SeasonPlaning seasonPlaning)
        {
            ServiceResult result = BusinessRules.Run(CheckIfPlaningTypeExists(seasonPlaning));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _seasonPlaningDal.Update(seasonPlaning);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(SeasonPlaning seasonPlaning)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<SeasonPlaning>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var result = _seasonPlaningDal.Delete(seasonPlaning);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteBySeason(Season season)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<SeasonPlaning>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var seasonPlanning = GetAllBySeasonId(season.Id);
            if (seasonPlanning.Result == false)
                return new ErrorServiceResult(false, "SeasonPlanningNotFound");

            foreach (var seasonPlaning in seasonPlanning.Data)
            {
                var deleteSeasonPlaning = Delete(seasonPlaning);
                if (deleteSeasonPlaning.Result == false)
                    return new ErrorServiceResult(false, "SeasonPlanningNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(SeasonPlaningValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<SeasonPlaning> Save(int seasonId, int customerId, List<SeasonPlaning> seasonPlanings)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
                return new DataServiceResult<SeasonPlaning>(false, MethodInterceptionBaseAttribute.Message);

            #endregion

            var dbSeasonPlanings = GetAllBySeasonId(seasonId).Data;
            foreach (var dbSeasonPlaning in dbSeasonPlanings)
            {
                var control = seasonPlanings.Any(x => x.Id == dbSeasonPlaning.Id);
                if (control != true)
                {
                    Delete(dbSeasonPlaning);
                }
            }

            foreach (var seasonPlaning in seasonPlanings)
            {
                seasonPlaning.CustomerId = customerId;
                if (seasonPlaning.Id > 0)
                {
                    Update(seasonPlaning);
                }
                else
                {
                    Add(seasonPlaning);
                }
            }
            return new SuccessDataServiceResult<SeasonPlaning>(true, "Saved");
        }

        private ServiceResult CheckIfPlaningTypeExists(SeasonPlaning seasonPlaning)
    {
        var result = _seasonPlaningDal.GetAll(x => x.SeasonId == seasonPlaning.SeasonId && x.ProductGroupId == seasonPlaning.ProductGroupId);

        if (result.Count > 1)
            new ErrorServiceResult(false, "DescriptionAlreadyExists");

        return new ServiceResult(true, "");
    }

}
}
