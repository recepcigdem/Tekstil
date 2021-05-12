using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.CsNoDeliveryDate
{
    public class CsNoDeliveryDateListLine
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public string Csno { get; set; }
        public DateTime Date { get; set; }

        public string Season { get; set; }

        public CsNoDeliveryDateListLine()
        {
            Id = 0;
            SeasonId = 0;
            Csno = string.Empty;
            Date = DateTime.UtcNow;
        }

        public CsNoDeliveryDateListLine(Entities.Concrete.CsNoDeliveryDate csNoDeliveryDate)
        {
            Id = csNoDeliveryDate.Id;
            SeasonId = csNoDeliveryDate.SeasonId;
            Csno = csNoDeliveryDate.Csno;
            Date = csNoDeliveryDate.Date;           
        }
    }
}
