using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CountryShippingMultiplierValidator : AbstractValidator<CountryShippingMultiplier>
    {
        public CountryShippingMultiplierValidator()
        {
            RuleFor(x => x.CountryId).GreaterThan(0);
            RuleFor(x => x.SeasonId).GreaterThan(0);
            RuleFor(x => x.SeasonCurrencyId).GreaterThan(0);
            RuleFor(x => x.ShippingMethodId).GreaterThan(0);
            RuleFor(x => x.Multiplier).GreaterThan(0);
            RuleFor(x => x.TestPrice).GreaterThan(0);
        }
    }
}
