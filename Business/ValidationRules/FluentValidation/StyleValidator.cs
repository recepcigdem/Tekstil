using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class StyleValidator : AbstractValidator<Style>
    {
        public StyleValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ProductGroupId).GreaterThan(0);
        }
    }
}
