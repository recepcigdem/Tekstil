using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ShipmentMethodValidator : AbstractValidator<ShipmentMethod>
    {
        public ShipmentMethodValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
