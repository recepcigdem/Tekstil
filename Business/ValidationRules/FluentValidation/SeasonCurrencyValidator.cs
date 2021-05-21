using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class SeasonCurrencyValidator : AbstractValidator<SeasonCurrency>
    {
        public SeasonCurrencyValidator()
        {
            RuleFor(x => x.CurrencyType).NotEmpty().WithMessage("CurrencyTypeNotEmpty");
            RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRateMustBeGreaterThanZero");
            RuleFor(x => x.SeasonId).GreaterThan(0).WithMessage("SeasonNotEmpty");
        }
    }
}
