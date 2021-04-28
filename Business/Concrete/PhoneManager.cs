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
using System.Security.Cryptography.X509Certificates;

namespace Business.Concrete
{
    public class PhoneManager : IPhoneService
    {
        private IPhoneDal _phoneDal;

        public PhoneManager(IPhoneDal phoneDal)
        {
            _phoneDal = phoneDal;
        }

        public IDataServiceResult<List<Phone>> GetAll(int customerId)
        {
            var dbResult = _phoneDal.GetAll(x=>x.CustomerId==customerId);

            return new SuccessDataServiceResult<List<Phone>>(dbResult, true, "Listed");
        }

        public IDataServiceResult<Phone> GetById(int phoneId)
        {
            var dbResult = _phoneDal.Get(p => p.Id == phoneId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Phone>(false, "SystemError");

            return new SuccessDataServiceResult<Phone>(dbResult, true, "Listed");
        }

        //[SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(PhoneValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Phone phone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(phone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _phoneDal.Add(phone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");

        }

       // [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(PhoneValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Phone phone)
        {
            IServiceResult result = BusinessRules.Run(CheckIfPhoneExists(phone));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _phoneDal.Update(phone);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }

       // [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Phone phone)
        {
            var result = _phoneDal.Delete(phone);
            if (result == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        //[SecuredOperation("admin,staff.saved")]
        [ValidationAspect(typeof(PhoneValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Phone> Save(Phone phone)
        {
            if (phone.Id > 0)
            {
                var result = Update(phone);
                if (result.Result == false)
                    return new DataServiceResult<Phone>(false, result.Message);
            }
            else
            {
                var result = Add(phone);
                if (result.Result == false)
                    return new DataServiceResult<Phone>(false, result.Message);
            }

            return new SuccessDataServiceResult<Phone>(true, "Saved");
        }

        private ServiceResult CheckIfPhoneExists(Phone phone)
        {
            var result = _phoneDal.GetAll(x => x.CountryCode == phone.CountryCode && x.AreaCode == phone.AreaCode && x.PhoneNumber == phone.PhoneNumber);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "PhoneAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
