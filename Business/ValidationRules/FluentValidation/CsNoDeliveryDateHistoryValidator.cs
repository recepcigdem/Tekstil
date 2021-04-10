using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CsNoDeliveryDateHistoryValidator : AbstractValidator<CsNoDeliveryDateHistory>
    {
        public CsNoDeliveryDateHistoryValidator()
        {
            RuleFor(x => x.CsNoDeliveryDateId).GreaterThan(0);
            RuleFor(x => x.Datetime).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
