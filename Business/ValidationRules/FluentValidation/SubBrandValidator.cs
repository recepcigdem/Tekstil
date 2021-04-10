using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class SubBrandValidator : AbstractValidator<SubBrand>
    {
        public SubBrandValidator()
        {
            RuleFor(x => x.BrandId).GreaterThan(0);
            RuleFor(x => x.SubBrandName).NotEmpty();
        }
    }
}
