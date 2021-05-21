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
        IDataServiceResult<List<SeasonPlaning>> GetAll(int customerId);
        IDataServiceResult<List<SeasonPlaning>> GetAllBySeasonId(int seasonId);
        IDataServiceResult<List<SeasonPlaning>> GetAllByProductGroupId(int customerId, int productGroupId);
        IDataServiceResult<SeasonPlaning> GetById(int seasonPlanningId);
        IServiceResult Add(SeasonPlaning seasonPlaning);
        IServiceResult Update(SeasonPlaning seasonPlaning);
        IServiceResult Delete(SeasonPlaning seasonPlaning);
        IServiceResult DeleteBySeason(Season season);
        IDataServiceResult<SeasonPlaning> Save(int seasonId, int customerId, List<SeasonPlaning> seasonPlanings);
    }
}
