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
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IDataResult<List<Category>> GetAll()
        {
            return new SuccessDataResult<List<Category>>(true, "Listed", _categoryDal.GetAll());
        }

        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>(true, "Listed", _categoryDal.Get(p => p.Id == categoryId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(CategoryValidator))]
        [TransactionScopeAspect]
        public IResult Add(Category category)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(category), CheckIfDescriptionExists(category));

            if (result != null)
                return result;

            _categoryDal.Add(category);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(CategoryValidator))]
        [TransactionScopeAspect]
        public IResult Update(Category category)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(category), CheckIfDescriptionExists(category));

            if (result != null)
                return result;

            _categoryDal.Add(category);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Category category)
        {
            _categoryDal.Delete(category);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Category category)
        {
            var result = _categoryDal.GetAll(x => x.Description == category.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Category category)
        {
            var result = _categoryDal.GetAll(x => x.CardCode == category.CardCode).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
