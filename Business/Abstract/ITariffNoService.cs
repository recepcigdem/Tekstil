using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ITariffNoService
    {
        IDataServiceResult<List<TariffNo>> GetAll(int customerId);
        IDataServiceResult<TariffNo> GetById(int tariffNoId);
        IDataServiceResult<TariffNo> Save(TariffNo tariffNo,List<TariffNoDetail> tariffNoDetails);
        IServiceResult Delete(TariffNo tariffNo);
    }
}
