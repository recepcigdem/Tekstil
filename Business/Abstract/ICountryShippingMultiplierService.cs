using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICountryShippingMultiplierService
    {
        IDataServiceResult<List<CountryShippingMultiplier>> GetAll(int customerId);
        IDataServiceResult<List<CountryShippingMultiplier>> GetAllBySeasonId(int seasonId);
        IDataServiceResult<List<CountryShippingMultiplier>> GetAllBySeasonCurrencyId(int customerId, int seasonCurrencyId);
        IDataServiceResult<List<CountryShippingMultiplier>> GetAllByShippingMethodId(int customerId, int shippingMethodId);
        IDataServiceResult<List<CountryShippingMultiplier>> GetAllByCountryId(int customerId, int countryId);
        IDataServiceResult<CountryShippingMultiplier> GetById(int countryShippingMultipliersId);
        IServiceResult Add(CountryShippingMultiplier countryShippingMultiplier);
        IServiceResult Update(CountryShippingMultiplier countryShippingMultiplier);
        IServiceResult Delete(CountryShippingMultiplier countryShippingMultiplier); 
        IServiceResult DeleteBySeason(Season season);
        IDataServiceResult<CountryShippingMultiplier> Save(int seasonId, int customerId, List<CountryShippingMultiplier> countryShippingMultipliers);
    }
}
