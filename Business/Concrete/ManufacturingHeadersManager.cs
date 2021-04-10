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
    public class ManufacturingHeadersManager : IManufacturingHeadersService
    {
        private IManufacturingHeadersDal _manufacturingHeadersDal;

        public ManufacturingHeadersManager(IManufacturingHeadersDal manufacturingHeadersDal)
        {
            _manufacturingHeadersDal = manufacturingHeadersDal;
        }

        public IDataResult<List<ManufacturingHeaders>> GetAll()
        {
            return new SuccessDataResult<List<ManufacturingHeaders>>(true, "Listed", _manufacturingHeadersDal.GetAll());
        }

        public IDataResult<ManufacturingHeaders> GetById(int manufacturingHeadersId)
        {
            return new SuccessDataResult<ManufacturingHeaders>(true, "Listed", _manufacturingHeadersDal.Get(p => p.Id == manufacturingHeadersId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ManufacturingHeadersValidator))]
        [TransactionScopeAspect]
        public IResult Add(ManufacturingHeaders manufacturingHeaders)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(manufacturingHeaders), CheckIfDescriptionExists(manufacturingHeaders));

            if (result != null)
                return result;

            _manufacturingHeadersDal.Add(manufacturingHeaders);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ManufacturingHeadersValidator))]
        [TransactionScopeAspect]
        public IResult Update(ManufacturingHeaders manufacturingHeaders)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(manufacturingHeaders), CheckIfDescriptionExists(manufacturingHeaders));

            if (result != null)
                return result;

            _manufacturingHeadersDal.Add(manufacturingHeaders);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(ManufacturingHeaders manufacturingHeaders)
        {
            _manufacturingHeadersDal.Delete(manufacturingHeaders);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(ManufacturingHeaders manufacturingHeaders)
        {
            var result = _manufacturingHeadersDal.GetAll(x => x.Description == manufacturingHeaders.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(ManufacturingHeaders manufacturingHeaders)
        {
            var result = _manufacturingHeadersDal.GetAll(x => x.Code == manufacturingHeaders.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
