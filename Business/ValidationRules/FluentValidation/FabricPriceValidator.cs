using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class FabricPriceValidator : AbstractValidator<FabricPrice>
    {
        public FabricPriceValidator()
        {
            RuleFor(x => x.FabricSupplierId).NotEmpty().WithMessage("SupplierNotEmpty");
            RuleFor(x => x.CurrencyTypeId).NotEmpty().WithMessage("CurrencyTypeNotEmpty");
            RuleFor(x => x.TranslationCurrencyTypeId).NotEmpty().WithMessage("TranslationCurrencyTypeNotEmpty");
        }
    }
}
