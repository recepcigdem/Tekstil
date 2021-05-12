using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IHierarchyService
    {
        IDataServiceResult<List<Hierarchy>> GetAll(int customerId);
        IDataServiceResult<Hierarchy> GetById(int hierarchyId);
        IDataServiceResult<Hierarchy> Save(Hierarchy hierarchy);
        IServiceResult Delete(Hierarchy hierarchy);
    }
}
