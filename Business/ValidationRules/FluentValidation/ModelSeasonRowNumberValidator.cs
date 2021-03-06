using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ModelSeasonRowNumberValidator : AbstractValidator<ModelSeasonRowNumber>
    {
        public ModelSeasonRowNumberValidator()
        {
            RuleFor(x => x.ProductGroupId).GreaterThan(0).WithMessage("ProductNotEmpty"); 
            RuleFor(x => x.SeasonId).GreaterThan(0).WithMessage("SeasonNotEmpty");
            RuleFor(x => x.RowNumber).GreaterThan(0).WithMessage("RowNumberMustBeGreaterThanZero");
        }
    }
}
