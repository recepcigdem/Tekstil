using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CsNoDeliveryDate : BaseEntity
    {
        public int SeasonId { get; set; }
        public string Csno { get; set; }
        public DateTime Date { get; set; }

        public CsNoDeliveryDate()
        {
            SeasonId = 0;

            Csno = string.Empty;
            Date = DateTime.UtcNow;
        }
    }
}
