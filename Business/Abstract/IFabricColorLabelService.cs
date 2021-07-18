using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFabricColorLabelService
    {
        IDataServiceResult<List<FabricColorLabel>> GetAll(int customerId);
        IDataServiceResult<FabricColorLabel> GetById(int fabricColorLabelId);
        IServiceResult Delete(FabricColorLabel fabricColorLabel);
        IDataServiceResult<FabricColorLabel> Save(FabricColorLabel fabricColorLabel);
    }
}
