using System;
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
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Current;

namespace Business.Concrete
{
    public class CurrentPhoneManager : ICurrentPhoneService
    {
        private ICurrentPhoneDal _currentPhoneDal;
        private IPhoneService _phoneService;

        public CurrentPhoneManager(ICurrentPhoneDal currentPhoneDal, IPhoneService phoneService)
        {
            _currentPhoneDal = currentPhoneDal;
            _phoneService = phoneService;
        }

        public IDataServiceResult<List<CurrentPhone>> GetAll(int customerId)
        {
            var dbResult = _currentPhoneDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<CurrentPhone>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CurrentPhone> GetById(int currentPhoneId)
        {
            var dbResult = _currentPhoneDal.Get(p => p.Id == currentPhoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CurrentPhone>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CurrentPhone>(dbResult, true, "Listed");
        }

        public IDataServiceResult<CurrentPhone> GetByPhoneId(int phoneId)
        {
            var dbResult = _currentPhoneDal.Get(p => p.PhoneId == phoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<CurrentPhone>(false, "Error_SystemError");

            return new SuccessDataServiceResult<CurrentPhone>(dbResult, true, "Listed");
        }

        public IDataServiceResult<List<CurrentPhone>> GetAllByCurrentId(int currentId)
        {
            var dbResult = _currentPhoneDal.GetAll(x => x.CurrentId == currentId);

            return new SuccessDataServiceResult<List<CurrentPhone>>(dbResult, true, "Listed");
        }

        public IServiceResult Add(CurrentPhone currentPhone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(currentPhone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _currentPhoneDal.Add(currentPhone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");

        }

        public IServiceResult Update(CurrentPhone currentPhone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(currentPhone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _currentPhoneDal.Update(currentPhone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");

        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult Delete(CurrentPhone currentPhone)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new ErrorServiceResult(false, message);
            }

            #endregion

            var result = _currentPhoneDal.Delete(currentPhone);
            if (result == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [TransactionScopeAspect]
        public IServiceResult DeleteByCurrent(Customer customer)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new ErrorServiceResult(false, message);
            }

            #endregion

            var currentPhoneList = GetAllByCurrentId(customer.Id);
            if (currentPhoneList.Result == false)
                return new ErrorServiceResult(false, "CurrentPhoneNotFound");

            foreach (var currentPhone in currentPhoneList.Data)
            {
                var deleteCurrentPhone = Delete(currentPhone);
                if (deleteCurrentPhone.Result == false)
                    return new ErrorServiceResult(false, "CurrentPhoneNotDeleted");

                var phone = _phoneService.GetById(currentPhone.PhoneId);
                if (phone.Result == false)
                    return new ErrorServiceResult(false, "CurrentPhoneNotFound");

                var deletePhone = _phoneService.Delete(phone.Data);
                if (deletePhone.Result == false)
                    return new ErrorServiceResult(false, "CurrentPhoneNotDeleted");
            }

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(CurrentPhoneValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<CurrentPhone> Save(Customer customer, List<CurrentPhoneDto> currentPhoneDtos)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<CurrentPhone>(false, message);
            }

            #endregion

            DeleteByCurrent(customer);

            int customerId = (int)customer.CustomerId;
            if (customer.CustomerId == 0)
                customerId = 0;

            foreach (var currentPhoneDto in currentPhoneDtos)
            {
                Phone phone = new Phone
                {
                    CustomerId = customerId,
                    IsActive = currentPhoneDto.IsActive,
                    CountryCode = currentPhoneDto.CountryCode,
                    AreaCode = currentPhoneDto.AreaCode,
                    PhoneNumber = currentPhoneDto.PhoneNumber
                };

                _phoneService.Add(phone);

                CurrentPhone currentPhone = new CurrentPhone
                {
                    CustomerId = customerId,
                    CurrentId = customer.Id,
                    PhoneId = phone.Id,
                    IsMain = currentPhoneDto.IsMain
                };

                Add(currentPhone);
            }


            return new SuccessDataServiceResult<CurrentPhone>(true, "Saved");
        }

        private ServiceResult CheckIfPhoneExists(CurrentPhone currentPhone)
        {
            var result = _currentPhoneDal.GetAll(x => x.CurrentId == currentPhone.CurrentId && x.PhoneId == currentPhone.PhoneId);

            if (result.Count > 1)
                new ErrorServiceResult(false, "PhoneAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
