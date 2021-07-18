using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFabricPriceService
    {
        IDataServiceResult<List<FabricPrice>> GetAll(int customerId);
        IDataServiceResult<FabricPrice> GetById(int fabricPriceId);
        IServiceResult Delete(FabricPrice fabricPrice);
        IDataServiceResult<FabricPrice> Save(FabricPrice fabricPrice);
    }
}
