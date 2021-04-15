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
    public class PhoneManager : IPhoneService
    {
        private IPhoneDal _phoneDal;

        public PhoneManager(IPhoneDal phoneDal)
        {
            _phoneDal = phoneDal;
        }

        public IDataResult<List<Phone>> GetAll()
        {
            return new SuccessDataResult<List<Phone>>(true, "Listed", _phoneDal.GetAll());
        }

        public IDataResult<Phone> GetById(int phoneId)
        {
            return new SuccessDataResult<Phone>(true, "Listed", _phoneDal.Get(p => p.Id == phoneId));
        }

        [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(PhoneValidator))]
        [TransactionScopeAspect]
        public IResult Add(Phone phone)
        {
            IResult result = BusinessRules.Run(CheckIfPhoneNumberExists(phone));

            if (result != null)
                return result;

            _phoneDal.Add(phone);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(PhoneValidator))]
        [TransactionScopeAspect]
        public IResult Update(Phone phone)
        {
            IResult result = BusinessRules.Run( CheckIfPhoneNumberExists(phone));

            if (result != null)
                return result;

            _phoneDal.Add(phone);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Phone phone)
        {
            _phoneDal.Delete(phone);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfPhoneNumberExists(Phone phone)
        {
            var result = _phoneDal.GetAll(x => x.PhoneNumber == phone.PhoneNumber).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult DeleteByPhoneId(int phoneId)
        {
            _phoneDal.DeleteByFilter(p=>p.Id==phoneId);
            return new SuccessResult("Deleted");
        }
    }
}
