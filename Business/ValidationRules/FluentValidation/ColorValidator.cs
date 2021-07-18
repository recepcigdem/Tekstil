using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ColorValidator : AbstractValidator<Color>
    {
        public ColorValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("DateNotEmpty");
            RuleFor(x => x.Code).NotEmpty().WithMessage("CodeNotEmpty");
            RuleFor(x => x.DescriptionTr).NotEmpty().WithMessage("DescriptionTrNotEmpty");
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage("DescriptionEnNotEmpty");
        }
    }
}
