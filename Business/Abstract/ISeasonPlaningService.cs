using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISeasonPlaningService
    {
        IDataResult<List<SeasonPlaning>> GetAll();
        IDataResult<SeasonPlaning> GetById(int seasonPlaningId);
        IResult Add(SeasonPlaning seasonPlaning);
        IResult Update(SeasonPlaning seasonPlaning);
        IResult Delete(SeasonPlaning seasonPlaning);
    }
}
