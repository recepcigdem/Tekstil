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
    public class CollectionManager : ICollectionService
    {
        private ICollectionDal _collectionDal;

        public CollectionManager(ICollectionDal collectionDal)
        {
            _collectionDal = collectionDal;
        }

        public IDataResult<List<Collection>> GetAll()
        {
            return new SuccessDataResult<List<Collection>>(true, "Listed", _collectionDal.GetAll());
        }

        public IDataResult<Collection> GetById(int collectionId)
        {
            return new SuccessDataResult<Collection>(true, "Listed", _collectionDal.Get(p => p.Id == collectionId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(CollectionValidator))]
        [TransactionScopeAspect]
        public IResult Add(Collection collection)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(collection), CheckIfDescriptionExists(collection));

            if (result != null)
                return result;

            _collectionDal.Add(collection);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(CollectionValidator))]
        [TransactionScopeAspect]
        public IResult Update(Collection collection)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(collection), CheckIfDescriptionExists(collection));

            if (result != null)
                return result;

            _collectionDal.Add(collection);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Collection collection)
        {
            _collectionDal.Delete(collection);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Collection collection)
        {
            var result = _collectionDal.GetAll(x => x.Description == collection.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Collection collection)
        {
            var result = _collectionDal.GetAll(x => x.Code == collection.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
