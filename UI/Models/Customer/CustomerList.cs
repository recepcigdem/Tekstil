using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;

namespace UI.Models.Customer
{
    public class CustomerList
    {
        public List<CustomerListLine> data { get; set; }

        private ICustomerService _customerService;

        public CustomerList(HttpRequest request, ICustomerService customerService)
        {
            _customerService = customerService;

            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;

            var listGrid = _customerService.GetAll(false, customerId).Data;
            if (listGrid != null)
            {
                data = new List<CustomerListLine>();
                foreach (var item in listGrid)
                {
                    CustomerListLine line = new CustomerListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}

