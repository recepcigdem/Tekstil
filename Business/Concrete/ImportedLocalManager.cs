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
    public class ImportedLocalManager : IImportedLocalService
    {
        private IImportedLocalDal _importedLocalDal;

        public ImportedLocalManager(IImportedLocalDal importedLocalDal)
        {
            _importedLocalDal = importedLocalDal;
        }

        public IDataResult<List<ImportedLocal>> GetAll()
        {
            return new SuccessDataResult<List<ImportedLocal>>(true, "Listed", _importedLocalDal.GetAll());
        }

        public IDataResult<ImportedLocal> GetById(int importedLocalId)
        {
            return new SuccessDataResult<ImportedLocal>(true, "Listed", _importedLocalDal.Get(p => p.Id == importedLocalId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ImportedLocalValidator))]
        [TransactionScopeAspect]
        public IResult Add(ImportedLocal importedLocal)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(importedLocal), CheckIfDescriptionExists(importedLocal));

            if (result != null)
                return result;

            _importedLocalDal.Add(importedLocal);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ImportedLocalValidator))]
        [TransactionScopeAspect]
        public IResult Update(ImportedLocal importedLocal)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(importedLocal), CheckIfDescriptionExists(importedLocal));

            if (result != null)
                return result;

            _importedLocalDal.Add(importedLocal);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(ImportedLocal importedLocal)
        {
            _importedLocalDal.Delete(importedLocal);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(ImportedLocal importedLocal)
        {
            var result = _importedLocalDal.GetAll(x => x.Description == importedLocal.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(ImportedLocal importedLocal)
        {
            var result = _importedLocalDal.GetAll(x => x.Code == importedLocal.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
