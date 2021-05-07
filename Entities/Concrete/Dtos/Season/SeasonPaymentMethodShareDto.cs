using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Dtos.Season
{
    public class SeasonPaymentMethodShareDto : BaseCustomerEntity
    {
        public int SeasonId { get; set; }
        public int PaymentMethodId { get; set; }
        public int SeasonCurrencyId { get; set; }
        public decimal CenterShare { get; set; }
        public decimal CenterShareEuro { get; set; }
        public decimal AccessoryCenterShareEuro { get; set; }

        public string PaymentMethod { get; set; }
        public string SeasonCurrency { get; set; }
        public decimal ExchangeRates { get; set; }
        public decimal CenterShareTl { get; set; }
        public SeasonPaymentMethodShareDto()
        {
            SeasonId = 0;
            PaymentMethodId = 0;
            SeasonCurrencyId = 0;
            CenterShare = 0;
            CenterShareEuro = 0;
            AccessoryCenterShareEuro = 0;
            PaymentMethod = string.Empty;
            SeasonCurrency = string.Empty ;
            ExchangeRates = 0;
            CenterShareTl = 0;

        }
    }
}
