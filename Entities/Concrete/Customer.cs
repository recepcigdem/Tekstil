using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Customer", Schema = "customer")]
    public class Customer : BaseEntity
    {
        [Column("customerId")]
        public int? CustomerId { get; set; }
        [Column("customerTypeId")]
        public int? CustomerTypeId { get; set; }
        [Column("isCurrent")]
        public bool IsCurrent { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("customerName")]
        public string CustomerName { get; set; }

        public Customer()
        {
            CustomerId = 0;
            CustomerTypeId = 0;
            IsCurrent = true;
            IsActive = true; 
            Code = string.Empty;
            CustomerName = string.Empty;
           
        }
    }
}
