using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("CurrentType", Schema = "customer")]
    public class CurrentType : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("description")]
        public string Description { get; set; }

        public CurrentType()
        {
            IsActive = true;
            Description = string.Empty;
           
        }
    }
}
