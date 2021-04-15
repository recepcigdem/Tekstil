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
    public class EmailManager : IEmailService
    {
        private IEmailDal _emailDal;

        public EmailManager(IEmailDal emailDal)
        {
            _emailDal = emailDal;
        }

        public IDataResult<List<Email>> GetAll()
        {
            return new SuccessDataResult<List<Email>>(true, "Listed", _emailDal.GetAll());
        }

        public IDataResult<Email> GetById(int emailId)
        {
            return new SuccessDataResult<Email>(true, "Listed", _emailDal.Get(p => p.Id == emailId));
        }

        [SecuredOperation("admin,staff.add")]
        [ValidationAspect(typeof(EmailValidator))]
        [TransactionScopeAspect]
        public IResult Add(Email email)
        {
            IResult result = BusinessRules.Run( CheckIfDescriptionExists(email));

            if (result != null)
                return result;

            _emailDal.Add(email);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,staff.updated")]
        [ValidationAspect(typeof(EmailValidator))]
        [TransactionScopeAspect]
        public IResult Update(Email email)
        {
            IResult result = BusinessRules.Run( CheckIfDescriptionExists(email));

            if (result != null)
                return result;

            _emailDal.Add(email);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Email email)
        {
            _emailDal.Delete(email);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Email email)
        {
            var result = _emailDal.GetAll(x => x.EmailAddress == email.EmailAddress).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        [SecuredOperation("admin,staff.deleted")]
        [TransactionScopeAspect]
        public IResult DeleteByEmailId(int emailId)
        {
            _emailDal.DeleteByFilter(e=>e.Id==emailId);

            return new SuccessResult("Deleted");
        }
    }
}
