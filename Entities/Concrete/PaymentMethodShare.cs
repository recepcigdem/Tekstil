using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PaymentMethodShare : BaseEntity
    {
        public int SeasonId { get; set; }
        public int PaymentMethodId { get; set; }
        public int SeasonCurrencyId { get; set; }
        public decimal CenterShareTL { get; set; }
        public decimal CenterShareEURO { get; set; }
        public decimal AccessoryCenterShareEURO { get; set; }

        public PaymentMethodShare()
        {
            SeasonId = 0;
            PaymentMethodId = 0;
            SeasonCurrencyId = 0;
            CenterShareTL = 0;
            CenterShareTL = 0;
            AccessoryCenterShareEURO = 0;
        }
    }
}
