using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("FabricSupplier", Schema = "fabric")]
    public class FabricSupplier : BaseCustomerEntity
    {
        [Column("fabricId")]
        public int FabricId { get; set; }
        [Column("supplier")]
        public string Supplier { get; set; }
        [Column("isImpotedDomestic")]
        public bool IsImpotedDomestic { get; set; }


        public FabricSupplier()
        {
            FabricId = 0;
            Supplier = string.Empty;
            IsImpotedDomestic = false;

        }
    }
}
