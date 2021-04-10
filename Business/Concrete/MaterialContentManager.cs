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
    public class MaterialContentManager : IMaterialContentService
    {
        private IMaterialContentDal _materialContentDal;

        public MaterialContentManager(IMaterialContentDal materialContentDal)
        {
            _materialContentDal = materialContentDal;
        }

        public IDataResult<List<MaterialContent>> GetAll()
        {
            return new SuccessDataResult<List<MaterialContent>>(true, "Listed", _materialContentDal.GetAll());
        }

        public IDataResult<MaterialContent> GetById(int materialContentId)
        {
            return new SuccessDataResult<MaterialContent>(true, "Listed", _materialContentDal.Get(p => p.Id == materialContentId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(MaterialContentValidator))]
        [TransactionScopeAspect]
        public IResult Add(MaterialContent materialContent)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(materialContent), CheckIfDescriptionExists(materialContent));

            if (result != null)
                return result;

            _materialContentDal.Add(materialContent);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(MaterialContentValidator))]
        [TransactionScopeAspect]
        public IResult Update(MaterialContent materialContent)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(materialContent), CheckIfDescriptionExists(materialContent));

            if (result != null)
                return result;

            _materialContentDal.Add(materialContent);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(MaterialContent materialContent)
        {
            _materialContentDal.Delete(materialContent);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(MaterialContent materialContent)
        {
            var result = _materialContentDal.GetAll(x => x.Description == materialContent.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(MaterialContent materialContent)
        {
            var result = _materialContentDal.GetAll(x => x.Code == materialContent.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
