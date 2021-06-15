using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CurrentValidator : AbstractValidator<Customer>
    {
        public CurrentValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("CustomerNameNotEmpty");
            RuleFor(x => x.Code).NotEmpty().WithMessage("CodeNotEmpty");
            RuleFor(x => x.CustomerTypeId).GreaterThan(0).WithMessage("CustomerTypeNotEmpty");
        }
    }
}
