using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Customer", Schema = "staff")]
    public class Customer : BaseEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("customerName")]
        public string CustomerName { get; set; }

        public Customer()
        {
            IsActive = true;
            CustomerName = string.Empty;
        }
    }
}
