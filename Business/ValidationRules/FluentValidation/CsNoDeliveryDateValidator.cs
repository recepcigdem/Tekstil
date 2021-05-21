using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CsNoDeliveryDateValidator : AbstractValidator<CsNoDeliveryDate>
    {
        public CsNoDeliveryDateValidator()
        {
            RuleFor(x => x.SeasonId).GreaterThan(0).WithMessage("SeasonNotEmpty"); 
            RuleFor(x => x.Csno).NotEmpty().WithMessage("CsNoNotEmpty");
            RuleFor(x => x.Date).NotEmpty().WithMessage("DateNotEmpty");
        }
    }
}
