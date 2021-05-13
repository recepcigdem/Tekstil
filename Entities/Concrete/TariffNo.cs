using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("TariffNo", Schema = "definition")]
    public class TariffNo : BaseCustomerEntity
    {
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("description")]
        public string Description { get; set; }

        public TariffNo()
        {
            IsActive = false;
            IsUsed = false;

            Description = string.Empty;
            Code = string.Empty;
        }
    }
}
