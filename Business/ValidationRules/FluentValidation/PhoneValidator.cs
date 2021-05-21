using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PhoneValidator : AbstractValidator<Phone>
    {
        public PhoneValidator()
        {
            RuleFor(x => x.CountryCode).NotEmpty().WithMessage("CountryCodeNotEmpty");
            RuleFor(x => x.AreaCode).NotEmpty().WithMessage("AreaCodeNotEmpty");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumberNotEmpty");
        }
    }
}
