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
    public class ColorManager : IColorService
    {
        private IColorDal _colorDal;

        public ColorManager(IColorDal colorDal)
        {
            _colorDal = colorDal;
        }

        public IDataServiceResult<List<Color>> GetAll(int customerId)
        {
            var dbResult = _colorDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Color>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Color> GetById(int colorId)
        {
            var dbResult = _colorDal.Get(p => p.Id == colorId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Color>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Color>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Color color)
        {
            ServiceResult result = BusinessRules.Run(CheckIfColorExists(color));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _colorDal.Add(color);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(Color color)
        {
            ServiceResult result = BusinessRules.Run(CheckIfColorExists(color));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _colorDal.Update(color);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Color color)
        {


            ServiceResult result = BusinessRules.Run(CheckIfColorIsUsed(color));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _colorDal.Delete(color);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(ColorValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Color> Save(Color color)
        {
            if (color.Id > 0)
            {
                Update(color);
            }
            else
            {
                Add(color);
            }

            return new SuccessDataServiceResult<Color>(true, "Saved");
        }

        private ServiceResult CheckIfColorExists(Color color)
        {
            var result = _colorDal.GetAll(x => x.CustomerId == color.CustomerId && x.Code == color.Code && x.DescriptionTr == color.DescriptionTr);

            if (result.Count > 1)
                new ErrorServiceResult(false, "ColorAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfColorIsUsed(Color color)
        {
            var result = GetById(color.Id);
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "ColorIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
