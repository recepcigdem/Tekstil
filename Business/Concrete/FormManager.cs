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
    public class FormManager : IFormService
    {
        private IFormDal _formDal;

        public FormManager(IFormDal formDal)
        {
            _formDal = formDal;
        }

        public IDataResult<List<Form>> GetAll()
        {
            return new SuccessDataResult<List<Form>>(true, "Listed", _formDal.GetAll());
        }

        public IDataResult<Form> GetById(int formId)
        {
            return new SuccessDataResult<Form>(true, "Listed", _formDal.Get(p => p.Id == formId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(FormValidator))]
        [TransactionScopeAspect]
        public IResult Add(Form form)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(form), CheckIfDescriptionExists(form));

            if (result != null)
                return result;

            _formDal.Add(form);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(FormValidator))]
        [TransactionScopeAspect]
        public IResult Update(Form form)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(form), CheckIfDescriptionExists(form));

            if (result != null)
                return result;

            _formDal.Add(form);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Form form)
        {
            _formDal.Delete(form);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Form form)
        {
            var result = _formDal.GetAll(x => x.Description == form.Description).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Form form)
        {
            var result = _formDal.GetAll(x => x.Code == form.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
