using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("CsNoDeliveryDate", Schema = "definition")]
    public class CsNoDeliveryDate : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("csno")]
        public string Csno { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }

        public CsNoDeliveryDate()
        {
            SeasonId = 0;

            Csno = string.Empty;
            Date = DateTime.UtcNow;
        }
    }
}
