using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ITariffNoDetailService
    {
        IDataServiceResult<List<TariffNoDetail>> GetAll(int customerId);
        IDataServiceResult<TariffNoDetail> GetById(int tariffNoDetailId);
        IDataServiceResult<TariffNoDetail> Save(int customerId, List<TariffNoDetail> tariffNoDetail);
        IServiceResult Delete(TariffNoDetail tariffNoDetail);
        IServiceResult DeleteByTariffNo(TariffNo tariffNo);
    }
}
