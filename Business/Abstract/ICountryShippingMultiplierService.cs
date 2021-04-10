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
        IDataResult<List<CountryShippingMultiplier>> GetAll();
        IDataResult<CountryShippingMultiplier> GetById(int countryShippingMultiplierId);
        IResult Add(CountryShippingMultiplier countryShippingMultiplier);
        IResult Update(CountryShippingMultiplier countryShippingMultiplier);
        IResult Delete(CountryShippingMultiplier countryShippingMultiplier);
    }
}
