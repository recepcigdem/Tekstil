using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFabricSupplierService
    {
        IDataServiceResult<List<FabricSupplier>> GetAll(int customerId);
        IDataServiceResult<FabricSupplier> GetById(int fabricSupplierId);
        IServiceResult Delete(FabricSupplier fabricSupplier);
        IDataServiceResult<FabricSupplier> Save(FabricSupplier fabricSupplier);
    }
}
