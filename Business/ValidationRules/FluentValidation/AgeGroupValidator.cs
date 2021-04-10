using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AgeGroupValidator : AbstractValidator<AgeGroup>
    {
        public AgeGroupValidator()
        {
            RuleFor(x => x.ShortDescription).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
