using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class TestValidator : AbstractValidator<Test>
    {
        public TestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("CodeNotEmpty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("DescriptionNotEmpty");
            RuleFor(x => x.Norm).NotEmpty().WithMessage("NormNotEmpty");
            RuleFor(x => x.Value).NotEmpty().WithMessage("ValueNotEmpty");
        }
    }
}
