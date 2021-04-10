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
    public class AuthorizationManager : IAuthorizationService
    {
        private IAuthorizationDal _authorizationDal;

        public AuthorizationManager(IAuthorizationDal authorizationDal)
        {
            _authorizationDal = authorizationDal;
        }

        public IDataResult<List<Authorization>> GetAll()
        {
            return new SuccessDataResult<List<Authorization>>(true, "Listed", _authorizationDal.GetAll());
        }

        public IDataResult<Authorization> GetById(int authorizationId)
        {
            return new SuccessDataResult<Authorization>(true, "Listed", _authorizationDal.Get(p => p.Id == authorizationId));
        }

        [SecuredOperation("admin")]
        [ValidationAspect(typeof(AuthorizationValidator))]
        [TransactionScopeAspect]
        public IResult Add(Authorization authorization)
        {
            IResult result = BusinessRules.Run(CheckIfAuthorizationNameExists(authorization));

            if (result != null)
                return result;

            _authorizationDal.Add(authorization);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin")]
        [ValidationAspect(typeof(AuthorizationValidator))]
        [TransactionScopeAspect]
        public IResult Update(Authorization authorization)
        {
            IResult result = BusinessRules.Run(CheckIfAuthorizationNameExists(authorization));

            if (result != null)
                return result;

            _authorizationDal.Add(authorization);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin")]
        [TransactionScopeAspect]
        public IResult Delete(Authorization authorization)
        {
            _authorizationDal.Delete(authorization);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfAuthorizationNameExists(Authorization authorization)
        {
            var result = _authorizationDal.GetAll(x => x.AuthorizationName == authorization.AuthorizationName).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
    }
}
