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
        IDataServiceResult<List<Season>> GetAll(int customerId);
        IDataServiceResult<Season> GetById(int seasonId);
        IServiceResult Add(Season season);
        IServiceResult Update(Season season);
        IServiceResult DeleteAll(Season season);
        IDataServiceResult<Season> SaveAll(Season season);
    }
}
