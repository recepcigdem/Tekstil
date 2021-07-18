using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFabricDetailService
    {
        IDataServiceResult<List<FabricDetail>> GetAll(int customerId);
        IDataServiceResult<FabricDetail> GetById(int fabricDetailId);
        IServiceResult Delete(FabricDetail fabricDetail);
        IDataServiceResult<FabricDetail> Save(FabricDetail fabricDetail);
    }
}
