using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class FabricValidator : AbstractValidator<Fabric>
    {
        public FabricValidator()
        {
            RuleFor(x => x.SeasonId).NotEmpty().WithMessage("SeasonNotEmpty");
            RuleFor(x => x.Definition).NotEmpty().WithMessage("DefinitionNotEmpty");
            RuleFor(x => x.FabricCode).NotEmpty().WithMessage("FabricCodeNotEmpty");
            RuleFor(x => x.FabricNameTr).NotEmpty().WithMessage("FabricNameTrNotEmpty");
            RuleFor(x => x.FabricNameEn).NotEmpty().WithMessage("FabricNameEnNotEmpty");
        }
    }
}
