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
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerNotEmpty");
            RuleFor(x => x.TariffNoId).NotEmpty().WithMessage("TariffNoNotEmpty");
            RuleFor(x => x.SeasonId).NotEmpty().WithMessage("SeasonNotEmpty");
            RuleFor(x => x.IsUsed).NotEmpty().WithMessage("IsUsedsNotEmpty");
        }
    }
}
