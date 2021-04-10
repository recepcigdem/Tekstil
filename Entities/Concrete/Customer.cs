using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Customer : BaseEntity
    {
        public bool IsActive { get; set; }
        public string CustomerName { get; set; }

        public Customer()
        {
            IsActive = false;
            CustomerName = string.Empty;
        }
    }
}
