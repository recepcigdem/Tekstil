using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class StaffPhoneValidator : AbstractValidator<StaffPhone>
    {
        public StaffPhoneValidator()
        {
            RuleFor(x => x.PhoneId).GreaterThan(0);
            RuleFor(x => x.StaffId).GreaterThan(0);
        }
    }
}
