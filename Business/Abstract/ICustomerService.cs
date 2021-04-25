using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICustomerService
    {
        IDataServiceResult<List<Customer>> GetAll();
        IDataServiceResult<Customer> GetById(int customerId);
        IServiceResult Add(Customer customer);
        IServiceResult Update(Customer customer);
        IServiceResult Delete(Customer customer);
        IDataServiceResult<Customer> Save(Customer customer);
    }
}
