using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CsNoDeliveryDateHistory : BaseEntity
    {
        public int CsNoDeliveryDateId { get; set; }
        public DateTime Datetime { get; set; }
        public string Description { get; set; }

        public CsNoDeliveryDateHistory()
        {
            CsNoDeliveryDateId = 0;
            Datetime = DateTime.UtcNow;
            Description = string.Empty;
        }
    }
}
