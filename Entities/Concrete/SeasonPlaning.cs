using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SeasonPlaning : BaseEntity
    {
        public int SeasonId { get; set; }
        public int ProductGroupId { get; set; }
        public int PlanUnitQuantity { get; set; }
        public int PlanModelMain { get; set; }
        public int PlanModelMini { get; set; }
        public int PlanModelMidi { get; set; }
        public int PlanOptionMain { get; set; }
        public int PlanOptionMini { get; set; }
        public int PlanOptionMidi { get; set; }
        public int PlanQuantityMain { get; set; }
        public int PlanQuantityMini { get; set; }
        public int PlanQuantityMidi { get; set; }
        public int PlanTotalQuantity { get; set; }
        public int RealModelMain { get; set; }
        public int RealModelMini { get; set; }
        public int RealModelMidi { get; set; }
        public int RealOptionMain { get; set; }
        public int RealOptionMini { get; set; }
        public int RealOptionMidi { get; set; }
        public int RealQuantityMain { get; set; }
        public int RealQuantityMini { get; set; }
        public int RealQuantityMidi { get; set; }
        public int RealTotalQuantity { get; set; }

        public SeasonPlaning()
        {
            SeasonId = 0;
            ProductGroupId = 0;
            PlanUnitQuantity = 0;
            PlanModelMain = 0;
            PlanModelMini = 0;
            PlanModelMidi = 0;
            PlanOptionMain = 0;
            PlanOptionMini = 0;
            PlanOptionMidi = 0;
            PlanQuantityMain = 0;
            PlanQuantityMini = 0;
            PlanQuantityMidi = 0;
            PlanTotalQuantity = 0;
            RealModelMain = 0;
            RealModelMini = 0;
            RealModelMidi = 0;
            RealOptionMain = 0;
            RealOptionMini = 0;
            RealOptionMidi = 0;
            RealQuantityMain = 0;
            RealQuantityMini = 0;
            RealQuantityMidi = 0; 
            RealTotalQuantity = 0;
        }
    }
}
