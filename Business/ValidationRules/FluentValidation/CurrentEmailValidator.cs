using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CurrentEmailValidator : AbstractValidator<CurrentEmail>
    {
        public CurrentEmailValidator()
        {
            RuleFor(x => x.EmailId).GreaterThan(0).WithMessage("EmailNotEmpty");
            RuleFor(x => x.CurrentId).GreaterThan(0).WithMessage("CurrentNotEmpty");
        }
    }
}
