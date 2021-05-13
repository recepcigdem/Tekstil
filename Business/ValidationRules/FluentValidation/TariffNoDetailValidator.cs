using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class TariffNoDetailValidator : AbstractValidator<TariffNoDetail>
    {
        public TariffNoDetailValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.TariffNoId).NotEmpty();
            RuleFor(x => x.SeasonId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.IsUsed).NotEmpty();
        }
    }
}
