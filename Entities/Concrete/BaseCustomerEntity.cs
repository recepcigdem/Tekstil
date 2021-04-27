using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class BaseCustomerEntity :BaseEntity
    {
        [Column("customerId")]
        public int CustomerId { get; set; }

        public BaseCustomerEntity()
        {
            CustomerId = 0;
        }
    }
}
