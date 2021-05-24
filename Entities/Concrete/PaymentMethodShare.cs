using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("PaymentMethodShare", Schema = "season")]
    public class PaymentMethodShare : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("paymentMethodId")]
        public int PaymentMethodId { get; set; }
        [Column("seasonCurrencyId")]
        public int SeasonCurrencyId { get; set; }
        [Column("centerShare")]
        public decimal? CenterShare { get; set; }
        [Column("centerShareEuro")]
        public decimal? CenterShareEuro { get; set; }
        [Column("accessoryCenterShareEuro")]
        public decimal? AccessoryCenterShareEuro { get; set; }

        public PaymentMethodShare()
        {
            SeasonId = 0;
            PaymentMethodId = 0;
            SeasonCurrencyId = 0;
            CenterShare = 0;
            CenterShareEuro = 0;
            AccessoryCenterShareEuro = 0;
        }
    }
}
