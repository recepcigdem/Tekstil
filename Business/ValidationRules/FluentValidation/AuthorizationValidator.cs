using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AuthorizationValidator : AbstractValidator<Authorization>
    {
        public AuthorizationValidator()
        {
           // RuleFor(x => x.IsActive).Must(x=>x==true);
            RuleFor(x => x.AuthorizationName).NotEmpty();
        }
    }
}
