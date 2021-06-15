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
    public interface ICurrentPhoneService
    {
        IDataServiceResult<List<CurrentPhone>> GetAll(int customerId);
        IDataServiceResult<CurrentPhone> GetById(int currentPhoneId);
        IDataServiceResult<CurrentPhone> GetByPhoneId(int phoneId);
        IDataServiceResult<List<CurrentPhone>> GetAllByCurrentId(int currentId);
        IServiceResult Add(CurrentPhone currentPhone);
        IServiceResult Update(CurrentPhone currentPhone);
        IServiceResult Delete(CurrentPhone currentPhone);
        IServiceResult DeleteByCurrent(Customer customer);
        IDataServiceResult<CurrentPhone> Save(Customer customer, List<CurrentPhoneDto> currentPhoneDtos);
    }
}
