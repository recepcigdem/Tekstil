using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFabricService
    {
        IDataServiceResult<List<Fabric>> GetAll(int customerId);
        IDataServiceResult<Fabric> GetById(int fabricId);
        IServiceResult Delete(Fabric fabric);
        IDataServiceResult<Fabric> Save(Fabric fabric);
    }
}
