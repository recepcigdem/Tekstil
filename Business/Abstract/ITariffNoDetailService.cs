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
        IDataServiceResult<List<TariffNoDetail>> GetAllByTariffNo(int tariffNoId);
        IDataServiceResult<List<TariffNoDetail>> GetAllBySeasonId(int customerId, int seasonId);
        IDataServiceResult<List<TariffNoDetail>> GetAllBySeasonCurrencyId(int customerId,int seasonCurrencyId);
        IDataServiceResult<List<TariffNoDetail>> GetAllByCountryId(int customerId, int countryId);
        IDataServiceResult<TariffNoDetail> GetById(int tariffNoDetailId);
        IDataServiceResult<TariffNoDetail> GetBySeasonId(int seasonId);
        IDataServiceResult<TariffNoDetail> Save(int tariffNoId,int customerId, List<TariffNoDetail> tariffNoDetail);
        IServiceResult Delete(TariffNoDetail tariffNoDetail);
        IServiceResult DeleteByTariffNo(TariffNo tariffNo);
    }
}
