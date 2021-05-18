﻿using System;
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
        private IStaffAuthorizationService _staffAuthorizationService;
       //private AuthorizationValidator _authorizationValidator;

        public AuthorizationManager(IAuthorizationDal authorizationDal, IStaffAuthorizationService staffAuthorizationService)
        {
            _authorizationDal = authorizationDal;
            _staffAuthorizationService = staffAuthorizationService;
           // _authorizationValidator = authorizationValidator;
        }

        public IDataServiceResult<List<Authorization>> GetAll()
        {
            var dbResult = _authorizationDal.GetAll();

            return new SuccessDataServiceResult<List<Authorization>>(dbResult, true, "Message_Listed");
        }

        public IDataServiceResult<List<Authorization>> GetAllAuthorizationByStaffId(int staffId)
        {
            var staffAuthorizationList = _staffAuthorizationService.GetAllByStaffId(staffId);

            List<Authorization> authorizations = new List<Authorization>();
            foreach (var staffAuthorization in staffAuthorizationList.Data)
            {
                var authorization = GetById(staffAuthorization.AuthorizationId);
                if (authorization.Result)
                    authorizations.Add(authorization.Data);
            }

            return new SuccessDataServiceResult<List<Authorization>>(authorizations, true, "Listed");
        }

        public IDataServiceResult<Authorization> GetById(int authorizationId)
        {
            var dbResult = _authorizationDal.Get(p => p.Id == authorizationId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Authorization>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Authorization>(dbResult, true, "Message_Listed");
        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(AuthorizationValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Authorization authorization)
        {
            IServiceResult result = BusinessRules.Run(CheckIfAuthorizationExists(authorization));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _authorizationDal.Add(authorization);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Message_Added");

        }

        // [SecuredOperation("SuperAdmin")]
        [ValidationAspect(typeof(AuthorizationValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Authorization authorization)
        {
            IServiceResult result = BusinessRules.Run(CheckIfAuthorizationExists(authorization));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _authorizationDal.Update(authorization);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Message_Updated");

        }

        [SecuredOperation("SuperAdmin")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Authorization authorization)
        {
            //    IServiceResult result = BusinessRules.Run(CheckIfAuthorizationIsUsed(authorization));
            //    if (result.Result == false)
            //        return new ErrorServiceResult(false, result.Message);

            //try
            //{
                var dbResult = _authorizationDal.Delete(authorization);
                if (dbResult == false)
                    return new ErrorServiceResult(false, "Error_SystemError");
            //}
            //catch (Exception)
            //{
            //    return new ErrorServiceResult(false, "Error_SystemError");
            //}
          

            return new ServiceResult(true, "Message_Delated");
        }

        // [SecuredOperation("SuperAdmin")]
       
        [TransactionScopeAspect]
        //[ValidationAspect(typeof(AuthorizationValidator))]
        public IDataServiceResult<Authorization> Save(Authorization authorization)
        {

            AuthorizationValidator authorizationValidator = new AuthorizationValidator();
            var validate= authorizationValidator.Validate(authorization);
            if (!validate.IsValid)
            {
                return new DataServiceResult<Authorization>(false, validate.ToString());
            }

            if (authorization.Id > 0)
            {
                var result = Update(authorization);
                if (result.Result == false)
                    return new DataServiceResult<Authorization>(false, result.Message);
            }
            else
            {
                var result = Add(authorization);
                if (result.Result == false)
                    return new DataServiceResult<Authorization>(false, result.Message);
            }

            return new SuccessDataServiceResult<Authorization>(true, "Message_Saved");
        }

        private ServiceResult CheckIfAuthorizationExists(Authorization authorization)
        {
            var result = _authorizationDal.GetAll(x => x.AuthorizationName == authorization.AuthorizationName);
            if (result.Count > 1)
                return new ErrorServiceResult(false, "Message_AuthorizationAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfAuthorizationIsUsed(Authorization authorization)
        {
            var result = _staffAuthorizationService.GetAllByAuthorizationId(authorization.Id);
            if (result.Data.Count>0)
                return new ErrorServiceResult(false, "Message_AuthorizationIsUsed");

            return new ServiceResult(true, "");
        }
    }
}

