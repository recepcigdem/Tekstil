using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;

namespace UI.Models.Customer
{
    public class CustomerList
    {
        public List<CustomerListLine> data { get; set; }

        private ICustomerService _customerService;

        public CustomerList(ICustomerService customerService)
        {
            _customerService = customerService;

            var listGrid = _customerService.GetAll().Data;
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

