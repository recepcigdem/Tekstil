using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CardCode).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Kdv).GreaterThan(0);
        }
    }
}
