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
        IDataResult<List<SeasonCurrency>> GetAll();
        IDataResult<SeasonCurrency> GetById(int seasonCurrencyId);
        IResult Add(SeasonCurrency seasonCurrency);
        IResult Update(SeasonCurrency seasonCurrency);
        IResult Delete(SeasonCurrency seasonCurrency);
    }
}
