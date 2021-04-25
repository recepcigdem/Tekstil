using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPhoneService
    {
        IDataServiceResult<List<Phone>> GetAll();
        IDataServiceResult<Phone> GetById(int phoneId);
        IServiceResult Add(Phone phone);
        IServiceResult Update(Phone phone);
        IServiceResult Delete(Phone phone);
        IDataServiceResult<Phone> Save(Phone phone);
    }
}
