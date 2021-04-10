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
        IDataResult<List<TariffNo>> GetAll();
        IDataResult<TariffNo> GetById(int tariffNoId);
        IResult Add(TariffNo tariffNo);
        IResult Update(TariffNo tariffNo);
        IResult Delete(TariffNo tariffNo);
    }
}
