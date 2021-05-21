using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class SeasonValidator : AbstractValidator<Season>
    {
        public SeasonValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("CodeNotEmpty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("DescriptionNotEmpty");
        }
    }
}
