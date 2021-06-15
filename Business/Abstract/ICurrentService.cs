using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos.Current;

namespace Business.Abstract
{
    public interface ICurrentService
    {
        IDataServiceResult<List<Customer>> GetAll(bool isCurrent, int customerId);
        IDataServiceResult<Customer> GetById(int customerId);
        IServiceResult Add(Customer customer);
        IServiceResult Update(Customer customer);
        IServiceResult Delete(Customer customer);
        IServiceResult DeleteAll(Customer customer);
        IDataServiceResult<Customer> Save(Customer customer);
        IDataServiceResult<Customer> SaveAll(Customer customer, List<CurrentEmailDto> currentEmailDtos, List<CurrentPhoneDto> currentPhoneDtos);
    }
}
