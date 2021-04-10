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
    public class OutletSeasonManager : IOutletSeasonService
    {
        private IOutletSeasonDal _outletSeasonDal;

        public OutletSeasonManager(IOutletSeasonDal outletSeasonDal)
        {
            _outletSeasonDal = outletSeasonDal;
        }

        public IDataResult<List<OutletSeason>> GetAll()
        {
            return new SuccessDataResult<List<OutletSeason>>(true, "Listed", _outletSeasonDal.GetAll());
        }

        public IDataResult<OutletSeason> GetById(int outletSeasonId)
        {
            return new SuccessDataResult<OutletSeason>(true, "Listed", _outletSeasonDal.Get(p => p.Id == outletSeasonId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(OutletSeasonValidator))]
        [TransactionScopeAspect]
        public IResult Add(OutletSeason outletSeason)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(outletSeason), CheckIfDescriptionExists(outletSeason));

            if (result != null)
                return result;

            _outletSeasonDal.Add(outletSeason);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(OutletSeasonValidator))]
        [TransactionScopeAspect]
        public IResult Update(OutletSeason outletSeason)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(outletSeason), CheckIfDescriptionExists(outletSeason));

            if (result != null)
                return result;

            _outletSeasonDal.Add(outletSeason);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(OutletSeason outletSeason)
        {
            _outletSeasonDal.Delete(outletSeason);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(OutletSeason outletSeason)
        {
            var result = _outletSeasonDal.GetAll(x => x.Description == outletSeason.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(OutletSeason outletSeason)
        {
            var result = _outletSeasonDal.GetAll(x => x.Code == outletSeason.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
