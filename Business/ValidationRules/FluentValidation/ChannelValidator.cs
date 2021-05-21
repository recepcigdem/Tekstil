using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ChannelValidator : AbstractValidator<Channel>
    {
        public ChannelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("CodeNotEmpty");
            RuleFor(x => x.ChannelName).NotEmpty().WithMessage("ChannelNameNotEmpty");
            RuleFor(x => x.CurrencyType).NotEmpty().WithMessage("CurrencyTypeNotEmpty");
        }
    }
}
