using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Season", Schema = "season")]
    public class Season : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("description")]
        public string Description { get; set; }

        public Season()
        {
            IsActive = true;
            Code = string.Empty;
            Description = string.Empty;
        }
    }
}
