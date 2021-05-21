using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class DefinitionTitleValidator:AbstractValidator<DefinitionTitle>
    {
        public DefinitionTitleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("TitleNotEmpty");
            RuleFor(x => x.Value).NotEmpty().WithMessage("ValueNotEmpty");
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("ValueMustBeGreaterThanZero");
        }
    }
}
