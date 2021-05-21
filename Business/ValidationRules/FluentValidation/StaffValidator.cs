using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerNotEmpty");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("DepartmentNotEmpty");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstNameNotEmpty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastNameNotEmpty");
            RuleFor(x => x.Title).NotEmpty().WithMessage("TitleNotEmpty");
        }
    }
}
