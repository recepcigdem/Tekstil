using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Customer
{
    public class Customer : BaseModel
    {
        public int EntityId { get; set; }
        public bool IsActive { get; set; }
        public string CustomerName { get; set; }

        public Customer()
        {
            IsActive = false;
            CustomerName = string.Empty;
        }
        public Customer(HttpRequest request, Entities.Concrete.Customer customer, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = customer.Id;
            IsActive = customer.IsActive;
            CustomerName = customer.CustomerName;
        }

        public Entities.Concrete.Customer GetBusinessModel()
        {
            Entities.Concrete.Customer customer = new Entities.Concrete.Customer();
            if (EntityId > 0)
            {
                customer.Id = EntityId;
            }

            customer.IsActive = IsActive;
            customer.CustomerName = CustomerName;

            return customer;
        }
    }
}
