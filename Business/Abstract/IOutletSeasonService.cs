using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IOutletSeasonService
    {
        IDataResult<List<OutletSeason>> GetAll();
        IDataResult<OutletSeason> GetById(int outletSeasonId);
        IResult Add(OutletSeason outletSeason);
        IResult Update(OutletSeason outletSeason);
        IResult Delete(OutletSeason outletSeason);
    }
}
