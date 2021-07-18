using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("FabricColorLabel", Schema = "fabric")]
    public class FabricColorLabel : BaseCustomerEntity
    {
        [Column("fabricId")]
        public int FabricId { get; set; }
        [Column("colorId")]
        public int ColorId { get; set; }
        [Column("labelId")]
        public int LabelId { get; set; }


        public FabricColorLabel()
        {
            FabricId = 0;
            ColorId = 0;
            LabelId = 0;

        }
    }
}
