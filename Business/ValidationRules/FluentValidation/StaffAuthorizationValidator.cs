using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class StaffAuthorizationValidator : AbstractValidator<StaffAuthorization>
    {
        public StaffAuthorizationValidator()
        {
            RuleFor(x => x.AuthorizationId).GreaterThan(0).WithMessage("AuthorizationNotEmpty");
            RuleFor(x => x.StaffId).GreaterThan(0).WithMessage("StaffNotEmpty");
        }
    }
}
