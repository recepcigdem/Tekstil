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
        public int? CustomerId { get; set; }
        public int? CustomerTypeId { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }

        public Customer()
        {
            CustomerId = 0;
            CustomerTypeId = 0;
            IsCurrent = false;
            IsActive = false;
            Code = string.Empty;
            CustomerName = string.Empty;
        }
        public Customer(HttpRequest request, Entities.Concrete.Customer customer, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = customer.Id;
            CustomerId = customer.CustomerId;
            CustomerTypeId = customer.CustomerTypeId;
            IsCurrent = customer.IsCurrent;
            IsActive = customer.IsActive;
            Code = customer.Code;
            CustomerName = customer.CustomerName;
        }

        public Entities.Concrete.Customer GetBusinessModel()
        {
            Entities.Concrete.Customer customer = new Entities.Concrete.Customer();
            if (EntityId > 0)
            {
                customer.Id = EntityId;
            }

            customer.CustomerId = 1;
            customer.CustomerTypeId = CustomerTypeId;
            if (CustomerTypeId>0)
                customer.IsCurrent = true;
            else
            customer.IsCurrent = false;

            customer.IsActive = IsActive;
            customer.Code = Code;
            customer.CustomerName = CustomerName;

            return customer;
        }
    }
}
