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
            RuleFor(x => x.SeasonId).GreaterThan(0);
            RuleFor(x => x.SeasonCurrencyId).GreaterThan(0);
            RuleFor(x => x.PaymentMethodId).GreaterThan(0);
            RuleFor(x => x.CenterShareEuro).GreaterThan(0);
            RuleFor(x => x.CenterShare).GreaterThan(0);
        }
    }
}
