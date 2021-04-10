using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISeasonService
    {
        IDataResult<List<Season>> GetAll();
        IDataResult<Season> GetById(int seasonId);
        IResult Add(Season season);
        IResult Update(Season season);
        IResult Delete(Season season);
    }
}
