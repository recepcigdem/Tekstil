using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Current
{
    public class CurrentListLine
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerTypeId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }

        public CurrentListLine() : base()
        {
            Id = 0;
            CustomerId = 0;
            CustomerTypeId = 0;
            IsActive = false;
            Code = string.Empty;
            CustomerName = string.Empty;
        }
        public CurrentListLine(Entities.Concrete.Customer customer)
        {
            Id = customer.Id;
            CustomerId = customer.CustomerId;
            CustomerTypeId = customer.CustomerTypeId;
            IsActive = customer.IsActive;
            Code = customer.Code;
            CustomerName = customer.CustomerName;
        }
    }
}