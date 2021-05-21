using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class StaffEmailValidator : AbstractValidator<StaffEmail>
    {
        public StaffEmailValidator()
        {
            RuleFor(x => x.EmailId).GreaterThan(0).WithMessage("EmailNotEmpty");
            RuleFor(x => x.StaffId).GreaterThan(0).WithMessage("StaffNotEmpty");
        }
    }
}
