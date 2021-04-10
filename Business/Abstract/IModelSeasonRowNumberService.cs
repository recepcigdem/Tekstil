using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IModelSeasonRowNumberService
    {
        IDataResult<List<ModelSeasonRowNumber>> GetAll();
        IDataResult<ModelSeasonRowNumber> GetById(int seasonId, int productGroupId);
        IResult Add(ModelSeasonRowNumber modelSeasonRowNumber);
        IResult Update(ModelSeasonRowNumber modelSeasonRowNumber);
        IResult Delete(ModelSeasonRowNumber modelSeasonRowNumber);
    }
}
