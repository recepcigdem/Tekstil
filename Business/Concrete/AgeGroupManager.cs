using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public IDataServiceResult<List<AgeGroup>> GetAll()
        {
            var dbResult = _ageGroupDal.GetAll();

            return new SuccessDataServiceResult<List<AgeGroup>>(dbResult,true, "Listed");
        }

        public IDataServiceResult<AgeGroup> GetById(int ageGroupId)
        {
            var dbResult = _ageGroupDal.Get(p => p.Id == ageGroupId);
            if (dbResult == null)
                return new SuccessDataServiceResult<AgeGroup>( false, "SystemError");

            return new SuccessDataServiceResult<AgeGroup>(dbResult, true, "Listed" );
        }

        //[SecuredOperation("admin,definition.add")]
        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(AgeGroupValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(AgeGroup ageGroup)
        {
            //IServiceResult result = BusinessRules.Run(CheckIfCodeExists(ageGroup), CheckIfShortDescriptionExists(ageGroup), CheckIfDescriptionExists(ageGroup));
            
            var result = CheckIfDescriptionExists(ageGroup);
            if (result.Result == false)
                return new ErrorServiceResult(false, "ControlErrorAdded");

            var dbResult = _ageGroupDal.Add(ageGroup);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult( true, "Added");

        }

        //[SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(AgeGroupValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(AgeGroup ageGroup)
        {
            //IServiceResult result = BusinessRules.Run(CheckIfCodeExists(ageGroup), CheckIfShortDescriptionExists(ageGroup), CheckIfDescriptionExists(ageGroup));
            var result = CheckIfDescriptionExists(ageGroup);

            if (result.Result == false)
                return new ErrorServiceResult(false, "ControlErrorUpdated");

            var dbResult = _ageGroupDal.Update(ageGroup);
            if (dbResult==null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");

        }
        //[SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(AgeGroupValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<AgeGroup> Save(AgeGroup ageGroup)
        {
            if (ageGroup.Id > 0)
            {
                Update(ageGroup);
            }
            else
            {
                Add(ageGroup);
            }
            return new SuccessDataServiceResult<AgeGroup>(true,"Saved");
        }

        //[SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(AgeGroup ageGroup)
        {
            _ageGroupDal.Delete(ageGroup);

            return new ServiceResult(true, "Delated");
        }

        private IServiceResult CheckIfDescriptionExists(AgeGroup ageGroup)
        {
            var result = _ageGroupDal.GetAll(x => x.Description == ageGroup.Description);

            if (result.Count>1)
                new ErrorServiceResult(false,"DescriptionAlreadyExists");

            return new ServiceResult();
            
        }

        private IServiceResult CheckIfShortDescriptionExists(AgeGroup ageGroup)
        {
            var result = _ageGroupDal.GetAll(x => x.ShortDescription == ageGroup.ShortDescription).Any();

            if (result)
                new ErrorServiceResult(false,"ShortDescriptionAlreadyExists");

            return new ServiceResult(true, "");
        }
        private IServiceResult CheckIfCodeExists(AgeGroup ageGroup)
        {
            var result = _ageGroupDal.GetAll(x => x.CardCode == ageGroup.CardCode).Any();

            if (result)
                new ErrorServiceResult(false,"CodeAlreadyExists");

            return new ServiceResult(true, "");
        }
    }
}
