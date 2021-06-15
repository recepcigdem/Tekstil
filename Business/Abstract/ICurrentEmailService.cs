using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Current;

namespace Business.Abstract
{
    public interface ICurrentEmailService
    {
        IDataServiceResult<List<CurrentEmail>> GetAll(int customerId);
        IDataServiceResult<CurrentEmail> GetById(int currentEmailId);
        IDataServiceResult<CurrentEmail> GetByEmailId(int emailId);
        IDataServiceResult<List<CurrentEmail>> GetAllByCurrentId(int currentId);
        IServiceResult Add(CurrentEmail currentEmail);
        IServiceResult Update(CurrentEmail currentEmail);
        IServiceResult Delete(CurrentEmail currentEmail);
        IServiceResult DeleteByCurrent(Customer customer);
        IDataServiceResult<CurrentEmail> Save(Customer customer, List<CurrentEmailDto> currentEmailDtos);
    }
}
