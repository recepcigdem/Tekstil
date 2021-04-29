using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PaymentMethodShare : BaseCustomerEntity
    {
        public int SeasonId { get; set; }
        public int PaymentMethodId { get; set; }
        public int SeasonCurrencyId { get; set; }
        public decimal CenterShareTl { get; set; }
        public decimal CenterShareEuro { get; set; }
        public decimal AccessoryCenterShareEuro { get; set; }

        public PaymentMethodShare()
        {
            SeasonId = 0;
            PaymentMethodId = 0;
            SeasonCurrencyId = 0;
            CenterShareTl = 0;
            CenterShareEuro = 0;
            AccessoryCenterShareEuro = 0;
        }
    }
}
