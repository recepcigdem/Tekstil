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
    public class ModelSeasonRowNumberManager : IModelSeasonRowNumberService
    {
        private IModelSeasonRowNumberDal _modelSeasonRowNumberDal;

        public ModelSeasonRowNumberManager(IModelSeasonRowNumberDal modelSeasonRowNumberDal)
        {
            _modelSeasonRowNumberDal = modelSeasonRowNumberDal;
        }


        public IDataServiceResult<List<ModelSeasonRowNumber>> GetAll(int customerId)
        {
            var dbResult = _modelSeasonRowNumberDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<ModelSeasonRowNumber>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<ModelSeasonRowNumber>> GetAllBySeasonId(int seasonId)
        {
            var dbResult = _modelSeasonRowNumberDal.GetAll(x => x.SeasonId == seasonId);

            return new SuccessDataServiceResult<List<ModelSeasonRowNumber>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<ModelSeasonRowNumber> GetById(int modelSeasonRowNumbersId)
        {
            var dbResult = _modelSeasonRowNumberDal.Get(p => p.Id == modelSeasonRowNumbersId);
            if (dbResult == null)
                return new SuccessDataServiceResult<ModelSeasonRowNumber>(false, "SystemError");

            return new SuccessDataServiceResult<ModelSeasonRowNumber>(dbResult, true, "Listed");
        }

        public IDataServiceResult<ModelSeasonRowNumber> GetByProductGroupIdAndRowNumber(int productGroupId, int rowNumber)
        {
            var dbResult = _modelSeasonRowNumberDal.Get(p => p.ProductGroupId == productGroupId && p.RowNumber == rowNumber);
            if (dbResult == null)
                return new SuccessDataServiceResult<ModelSeasonRowNumber>(false, "SystemError");

            return new SuccessDataServiceResult<ModelSeasonRowNumber>(dbResult, true, "Listed");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult Delete(ModelSeasonRowNumber modelSeasonRowNumbers)
        {
            var result = _modelSeasonRowNumberDal.Delete(modelSeasonRowNumbers);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IServiceResult DeleteBySeason(Season season)
        {
            var modelSeasonRowNumbers = GetAllBySeasonId(season.Id);
            if (modelSeasonRowNumbers.Result == false)
                return new ErrorServiceResult(false, "ModelSeasonRowNumberNotFound");

            foreach (var modelSeasonRowNumber in modelSeasonRowNumbers.Data)
            {
                var deleteModelSeasonRowNumber = Delete(modelSeasonRowNumber);
                if (deleteModelSeasonRowNumber.Result == false)
                    return new ErrorServiceResult(false, "ModelSeasonRowNumberNotDeleted");
            }

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("SuperAdmin,CompanyAdmin,seasonPlaning")]
        [TransactionScopeAspect]
        public IDataServiceResult<ModelSeasonRowNumber> Save(int seasonId , int customerId, List<ModelSeasonRowNumber> modelSeasonRowNumbers)
        {
            foreach (var modelSeasonRowNumber in modelSeasonRowNumbers)
            {
                if (modelSeasonRowNumber.Id < 1)
                {
                    var idControl = GetByProductGroupIdAndRowNumber(modelSeasonRowNumber.ProductGroupId, modelSeasonRowNumber.RowNumber);
                    if (idControl.Result)
                    {
                        modelSeasonRowNumber.Id = idControl.Data.Id;
                    }

                    var usedControl = GetById(modelSeasonRowNumber.Id);
                    if (usedControl.Result)
                    {
                        if (usedControl.Data.IsUsed)
                        {
                            return new DataServiceResult<ModelSeasonRowNumber>(false, "ProductGroupRowNumberUsed");
                        }
                    }
                }
            }

            var dbModelSeasonRowNumbers = GetAllBySeasonId(seasonId).Data;
            foreach (var dbModelSeasonRowNumber in dbModelSeasonRowNumbers)
            {
                var idControl = modelSeasonRowNumbers.Any(x => x.Id == dbModelSeasonRowNumber.Id);
                if (idControl != true)
                {
                    Delete(dbModelSeasonRowNumber);
                }
                
            }

            foreach (var modelSeasonRowNumber in modelSeasonRowNumbers)
            {
                modelSeasonRowNumber.CustomerId = customerId;
                if (modelSeasonRowNumber.Id > 0)
                {
                    var dbResult = _modelSeasonRowNumberDal.Update(modelSeasonRowNumber);
                    if (dbResult == null)
                        return new DataServiceResult<ModelSeasonRowNumber>(false, "SystemError");
                }
                else
                {
                    var dbResult = _modelSeasonRowNumberDal.Add(modelSeasonRowNumber);
                    if (dbResult == null)
                        return new DataServiceResult<ModelSeasonRowNumber>(false, "SystemError");
                }
            }
            return new SuccessDataServiceResult<ModelSeasonRowNumber>(true, "Saved");
        }


    }
}
