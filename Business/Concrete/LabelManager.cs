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
    public class LabelManager : ILabelService
    {
        private ILabelDal _labelDal;

        public LabelManager(ILabelDal labelDal)
        {
            _labelDal = labelDal;
        }

        public IDataServiceResult<List<Label>> GetAll(int customerId)
        {
            var dbResult = _labelDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Label>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Label> GetById(int labelId)
        {
            var dbResult = _labelDal.Get(p => p.Id == labelId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Label>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Label>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Label label)
        {
            ServiceResult result = BusinessRules.Run(CheckIfDescriptionExists(label), CheckIfCodeExists(label));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _labelDal.Add(label);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }
        
        public IServiceResult Update(Label label)
        {
            ServiceResult result = BusinessRules.Run(CheckIfDescriptionExists(label), CheckIfCodeExists(label));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _labelDal.Update(label);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Label label)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Label>(false, message);
            }

            #endregion

            ServiceResult result = BusinessRules.Run(CheckIfLabelIsUsed(label));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _labelDal.Delete(label);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(LabelValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Label> Save(Label label)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Label>(false, message);
            }

            #endregion

            if (label.Id > 0)
            {
                Update(label);
            }
            else
            {
                Add(label);
            }

            return new SuccessDataServiceResult<Label>(true, "Saved");
        }

        private ServiceResult CheckIfDescriptionExists(Label label)
        {
            var result = _labelDal.GetAll(x => x.CustomerId == label.CustomerId && x.Description == label.Description);

            if (result.Count > 1)
                new ErrorServiceResult(false, "DescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfCodeExists(Label label)
        {
            var result = _labelDal.GetAll(x => x.CustomerId == label.CustomerId && x.Code == label.Code);

            if (result.Count > 1)
                new ErrorServiceResult(false, "CodeAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfLabelIsUsed(Label label)
        {
            var result = GetById(label.Id);
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "LabelIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
