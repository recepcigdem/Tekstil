using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Customer
{
    public class CustomerListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string CustomerName { get; set; }

        public CustomerListLine() : base()
        {
            Id = 0;
            IsActive = false;
            CustomerName = string.Empty;
        }
        public CustomerListLine(Entities.Concrete.Customer customer)
        {
            Id = customer.Id;
            IsActive = customer.IsActive;
            CustomerName = customer.CustomerName;
        }
    }
}
