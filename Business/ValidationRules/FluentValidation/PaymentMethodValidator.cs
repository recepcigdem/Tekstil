using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class PaymentMethodValidator : AbstractValidator<PaymentMethod>
    {
        public PaymentMethodValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Kkdf).GreaterThan(0);
        }
    }
}
