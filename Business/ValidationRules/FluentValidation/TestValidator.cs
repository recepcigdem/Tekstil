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
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Norm).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}
