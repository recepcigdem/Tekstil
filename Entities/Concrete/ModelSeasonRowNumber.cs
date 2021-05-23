using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("ModelSeasonRowNumber", Schema = "season")]
    public class ModelSeasonRowNumber : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("productGroupId")]
        public int ProductGroupId { get; set; }
        [Column("rowNumber")]
        public int RowNumber { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("isUsed")]
        public bool IsUsed { get; set; }

        public ModelSeasonRowNumber()
        {
            SeasonId = 0;
            ProductGroupId = 0;
            RowNumber = 0;
            IsActive = true;
            IsUsed = false;
        }
    }
}
