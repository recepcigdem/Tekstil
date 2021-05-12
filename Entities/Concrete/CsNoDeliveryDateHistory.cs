using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("CsNoDeliveryDateHistory", Schema = "definition")]
    public class CsNoDeliveryDateHistory : BaseCustomerEntity
    {
        [Column("csNoDeliveryDateId")]
        public int CsNoDeliveryDateId { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("description")]
        public string Description { get; set; }

        public CsNoDeliveryDateHistory()
        {
            CsNoDeliveryDateId = 0;
            Datetime = DateTime.UtcNow;
            Description = string.Empty;
        }
    }
}
