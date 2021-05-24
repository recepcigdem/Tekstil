using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("SeasonPlaning", Schema = "season")]
    public class SeasonPlaning : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("productGroupId")]
        public int ProductGroupId { get; set; }
        [Column("planUnitQuantity")]
        public int? PlanUnitQuantity { get; set; }
        [Column("planModelMain")]
        public int? PlanModelMain { get; set; }
        [Column("planModelMini")]
        public int? PlanModelMini { get; set; }
        [Column("planModelMidi")]
        public int? PlanModelMidi { get; set; }
        [Column("planOptionMain")]
        public int? PlanOptionMain { get; set; }
        [Column("planOptionMini")]
        public int? PlanOptionMini { get; set; }
        [Column("planOptionMidi")]
        public int? PlanOptionMidi { get; set; }
        [Column("planQuantityMain")]
        public int? PlanQuantityMain { get; set; }
        [Column("planQuantityMini")]
        public int? PlanQuantityMini { get; set; }
        [Column("planQuantityMidi")]
        public int? PlanQuantityMidi { get; set; }
        [Column("planTotalQuantity")]
        public int? PlanTotalQuantity { get; set; }
        [Column("realModelMain")]
        public int? RealModelMain { get; set; }
        [Column("realModelMini")]
        public int? RealModelMini { get; set; }
        [Column("realModelMidi")]
        public int? RealModelMidi { get; set; }
        [Column("realOptionMain")]
        public int? RealOptionMain { get; set; }
        [Column("realOptionMini")]
        public int? RealOptionMini { get; set; }
        [Column("realOptionMidi")]
        public int? RealOptionMidi { get; set; }
        [Column("realQuantityMain")]
        public int? RealQuantityMain { get; set; }
        [Column("realQuantityMini")]
        public int? RealQuantityMini { get; set; }
        [Column("realQuantityMidi")]
        public int? RealQuantityMidi { get; set; }
        [Column("realTotalQuantity")]
        public int? RealTotalQuantity { get; set; }

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
