using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;

namespace Business.Concrete
{
    public class CsNoDeliveryDateManager : ICsNoDeliveryDateService
    {
        private ICsNoDeliveryDateDal _csNoDeliveryDateDal;
        private ICsNoDeliveryDateHistoryService _csNoDeliveryDateHistoryService;
        private IStaffService _staffService;
        public CsNoDeliveryDateManager(ICsNoDeliveryDateDal csNoDeliveryDateDal, IStaffService staffService, ICsNoDeliveryDateHistoryService csNoDeliveryDateHistoryService)
        {
            _csNoDeliveryDateDal = csNoDeliveryDateDal;
            _staffService = staffService;
            _csNoDeliveryDateHistoryService = csNoDeliveryDateHistoryService;
        }

        public IDataServiceResult<List<CsNoDeliveryDate>> GetAll(int customerId)
        {
            var dbResult = _csNoDeliveryDateDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<CsNoDeliveryDate>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CsNoDeliveryDate>> GetAllBySeasonId(int customerId, int seasonId)
        {
            var dbResult = _csNoDeliveryDateDal.GetAll(x => x.CustomerId == customerId && x.SeasonId == seasonId);

            return new SuccessDataServiceResult<List<CsNoDeliveryDate>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CsNoDeliveryDate> GetById(int csNoDeliveryDateId)
        {
            var dbResult = _csNoDeliveryDateDal.Get(p => p.Id == csNoDeliveryDateId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CsNoDeliveryDate>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CsNoDeliveryDate>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CsNoDeliveryDate> GetBySeasonId(int seasonId)
        {
            var dbResult = _csNoDeliveryDateDal.Get(p => p.SeasonId == seasonId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CsNoDeliveryDate>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CsNoDeliveryDate>(dbResult, true, "Listed");
        }

        public IServiceResult Add(CsNoDeliveryDate csNoDeliveryDate)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCsNoExists(csNoDeliveryDate));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _csNoDeliveryDateDal.Add(csNoDeliveryDate);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(CsNoDeliveryDate csNoDeliveryDate)
        {
            ServiceResult result = BusinessRules.Run(CheckIfCsNoExists(csNoDeliveryDate));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _csNoDeliveryDateDal.Update(csNoDeliveryDate);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(CsNoDeliveryDate csNoDeliveryDate)
        {

            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CsNoDeliveryDate>(false, message);
            }

            #endregion

            var result = _csNoDeliveryDateDal.Delete(csNoDeliveryDate);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            var historyResult = _csNoDeliveryDateHistoryService.DeleteByCsNoDeliveryDateId(csNoDeliveryDate.Id);
            if (historyResult.Result == false)
                return new ErrorServiceResult(false, historyResult.Message);

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(CsNoDeliveryDateValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<CsNoDeliveryDate> Save(int staffId, CsNoDeliveryDate csNoDeliveryDate)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CsNoDeliveryDate>(false, message);
            }

            #endregion

            if (csNoDeliveryDate.Id > 0)
            {
                Update(csNoDeliveryDate);
            }
            else
            {
                Add(csNoDeliveryDate);
            }

            #region CsNoDeliveryDateHistory

            CsNoDeliveryDateHistory csNoDeliveryDateHistory = new CsNoDeliveryDateHistory();

            csNoDeliveryDateHistory.CsNoDeliveryDateId = csNoDeliveryDate.Id;
            csNoDeliveryDateHistory.CustomerId = csNoDeliveryDate.CustomerId;
            csNoDeliveryDateHistory.Datetime = DateTime.UtcNow;

            var date = String.Format("{0:dd.MM.yyyy}", csNoDeliveryDate.Date);
            var staff = _staffService.GetById(staffId);
            if (staff.Result == true)
            {
                csNoDeliveryDateHistory.Description = ("Date: " + csNoDeliveryDateHistory.Datetime.ToString("dd.MM.yyyy HH:mm:ss") + " Person: " + staff.Data.FirstName + " " + staff.Data.LastName + " CsNo: " + csNoDeliveryDate.Csno + " DeliveryDate: " + date);
            }

            var history = _csNoDeliveryDateHistoryService.Add(csNoDeliveryDateHistory);
            if (history.Result == false)
                return new DataServiceResult<CsNoDeliveryDate>(false, history.Message);
            #endregion



            return new SuccessDataServiceResult<CsNoDeliveryDate>(true, "Saved");
        }

        private ServiceResult CheckIfCsNoExists(CsNoDeliveryDate csNoDeliveryDate)
        {
            var result = _csNoDeliveryDateDal.GetAll(x => x.CustomerId == csNoDeliveryDate.CustomerId && x.SeasonId == csNoDeliveryDate.SeasonId && x.Csno == csNoDeliveryDate.Csno);

            if (result.Count > 1)
                new ErrorServiceResult(false, "CsNoDeliveryDateAlreadyExists");

            return new ServiceResult(true, "");
        }

    }
}
