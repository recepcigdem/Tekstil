using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class FabricDetailValidator : AbstractValidator<FabricDetail>
    {
        public FabricDetailValidator()
        {
            RuleFor(x => x.ColorId).NotEmpty().WithMessage("ColorNotEmpty");
            RuleFor(x => x.CsNoId).NotEmpty().WithMessage("CsNoNotEmpty");
            RuleFor(x => x.ProductGroupId).NotEmpty().WithMessage("ProductGroupNotEmpty");
            RuleFor(x => x.RowNumber).NotEmpty().WithMessage("RowNumberNotEmpty");
        }
    }
}
