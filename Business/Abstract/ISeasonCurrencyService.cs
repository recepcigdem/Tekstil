using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISeasonCurrencyService
    {
        IDataServiceResult<List<SeasonCurrency>> GetAll(int customerId);
        IDataServiceResult<List<SeasonCurrency>> GetAllBySeasonId(int seasonId);
        IDataServiceResult<SeasonCurrency> GetById(int seasonCurrencyId);
        IServiceResult Add(SeasonCurrency seasonCurrency);
        IServiceResult Update(SeasonCurrency seasonCurrency);
        IServiceResult Delete(SeasonCurrency seasonCurrency);
        IServiceResult DeleteBySeason(Season season);
        IDataServiceResult<SeasonCurrency> Save(int customerId, List<SeasonCurrency> seasonCurrencies);
    }
}
