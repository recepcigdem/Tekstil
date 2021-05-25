using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICurrentTypeService
    {
        IDataServiceResult<List<CurrentType>> GetAll(int customerId);
        IDataServiceResult<CurrentType> GetById(int currentTypeId);
        IServiceResult Delete(CurrentType currentType);
        IDataServiceResult<CurrentType> Save(CurrentType currentType);
    }
}
