﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class AgeGroupManager : IAgeGroupService
    {
        private IAgeGroupDal _ageGroupDal;

        public AgeGroupManager(IAgeGroupDal ageGroupDal)
        {
            _ageGroupDal = ageGroupDal;
        }

        public IDataResult<List<AgeGroup>> GetAll()
        {
            return new SuccessDataResult<List<AgeGroup>>(true, "Listed", _ageGroupDal.GetAll());
        }

        public IDataResult<AgeGroup> GetById(int ageGroupId)
        {
            return new SuccessDataResult<AgeGroup>(true, "Listed", _ageGroupDal.Get(p => p.Id == ageGroupId));
        }

        //[SecuredOperation("admin,definition.add")]
        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(AgeGroupValidator))]
        [TransactionScopeAspect]
        public IDataResult<AgeGroup> Add(AgeGroup ageGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(ageGroup), CheckIfShortDescriptionExists(ageGroup), CheckIfDescriptionExists(ageGroup));

            if (result != null)
                return new ErrorDataResult<AgeGroup>("ControlErrorAdded");

            return new SuccessDataResult<AgeGroup>(true, "Added", _ageGroupDal.Add(ageGroup));

        }

        //[SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(AgeGroupValidator))]
        [TransactionScopeAspect]
        public IDataResult<AgeGroup> Update(AgeGroup ageGroup)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(ageGroup), CheckIfShortDescriptionExists(ageGroup), CheckIfDescriptionExists(ageGroup));
            if (result != null)
                return new ErrorDataResult<AgeGroup>("ControlErrorUpdated");

            return new SuccessDataResult<AgeGroup>(true, "Updated", _ageGroupDal.Update(ageGroup));

        }
        //[SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(AgeGroupValidator))]
        [TransactionScopeAspect]
        public IDataResult<AgeGroup> Save(AgeGroup ageGroup)
        {
            if (ageGroup.Id > 0)
            {
                Update(ageGroup);
            }
            else
            {
                Add(ageGroup);
            }
            return new SuccessDataResult<AgeGroup>(true, "Saved", ageGroup);
        }

        //[SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(AgeGroup ageGroup)
        {
            _ageGroupDal.Delete(ageGroup);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(AgeGroup ageGroup)
        {
            var result = _ageGroupDal.GetAll(x => x.Description == ageGroup.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfShortDescriptionExists(AgeGroup ageGroup)
        {
            var result = _ageGroupDal.GetAll(x => x.ShortDescription == ageGroup.ShortDescription).Any();

            if (result)
                new ErrorResult("ShortDescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(AgeGroup ageGroup)
        {
            var result = _ageGroupDal.GetAll(x => x.CardCode == ageGroup.CardCode).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
