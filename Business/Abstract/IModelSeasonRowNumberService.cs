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
        IDataServiceResult<List<ModelSeasonRowNumber>> GetAll(int customerId);
        IDataServiceResult<List<ModelSeasonRowNumber>> GetAllBySeasonId(int seasonId);
        IDataServiceResult<ModelSeasonRowNumber> GetById(int modelSeasonRowNumbersId);
        IDataServiceResult<ModelSeasonRowNumber> GetByProductGroupIdAndRowNumber(int productGroupId, int rowNumber);
        IServiceResult Delete(ModelSeasonRowNumber modelSeasonRowNumbers);
        IServiceResult DeleteBySeason(Season season);
        IDataServiceResult<ModelSeasonRowNumber> Save(int seasonId, int customerId, List<ModelSeasonRowNumber> modelSeasonRowNumbers);
    }
}
