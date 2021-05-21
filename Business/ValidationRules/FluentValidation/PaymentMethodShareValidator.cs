using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PaymentMethodShareValidator : AbstractValidator<PaymentMethodShare>
    {
        public PaymentMethodShareValidator()
        {
            RuleFor(x => x.SeasonId).GreaterThan(0).WithMessage("SeasonNotEmpty");
            RuleFor(x => x.SeasonCurrencyId).GreaterThan(0).WithMessage("SeasonCurrencyNotEmpty");
            RuleFor(x => x.PaymentMethodId).GreaterThan(0).WithMessage("PaymentMethodNotEmpty");
            RuleFor(x => x.CenterShareEuro).GreaterThan(0).WithMessage("CenterShareEuroMustBeGreaterThanZero");
            RuleFor(x => x.CenterShare).GreaterThan(0).WithMessage("CenterShareMustBeGreaterThanZero");
        }
    }
}
