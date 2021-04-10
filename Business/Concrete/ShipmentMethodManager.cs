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
    public class ShipmentMethodManager : IShipmentMethodService
    {
        private IShipmentMethodDal _shipmentMethodDal;

        public ShipmentMethodManager(IShipmentMethodDal shipmentMethodDal)
        {
            _shipmentMethodDal = shipmentMethodDal;
        }

        public IDataResult<List<ShipmentMethod>> GetAll()
        {
            return new SuccessDataResult<List<ShipmentMethod>>(true, "Listed", _shipmentMethodDal.GetAll());
        }

        public IDataResult<ShipmentMethod> GetById(int shipmentMethodId)
        {
            return new SuccessDataResult<ShipmentMethod>(true, "Listed", _shipmentMethodDal.Get(p => p.Id == shipmentMethodId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ShipmentMethodValidator))]
        [TransactionScopeAspect]
        public IResult Add(ShipmentMethod shipmentMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(shipmentMethod), CheckIfDescriptionExists(shipmentMethod));

            if (result != null)
                return result;

            _shipmentMethodDal.Add(shipmentMethod);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ShipmentMethodValidator))]
        [TransactionScopeAspect]
        public IResult Update(ShipmentMethod shipmentMethod)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(shipmentMethod), CheckIfDescriptionExists(shipmentMethod));

            if (result != null)
                return result;

            _shipmentMethodDal.Add(shipmentMethod);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(ShipmentMethod shipmentMethod)
        {
            _shipmentMethodDal.Delete(shipmentMethod);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(ShipmentMethod shipmentMethod)
        {
            var result = _shipmentMethodDal.GetAll(x => x.Description == shipmentMethod.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(ShipmentMethod shipmentMethod)
        {
            var result = _shipmentMethodDal.GetAll(x => x.Code == shipmentMethod.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
