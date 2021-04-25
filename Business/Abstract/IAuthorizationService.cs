using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IAuthorizationService
    {
        IDataServiceResult<List<Authorization>> GetAll();
        IDataServiceResult<Authorization> GetById(int authorizationId);
        IServiceResult Add(Authorization authorization);
        IServiceResult Update(Authorization authorization);
        IServiceResult Delete(Authorization authorization);
        IDataServiceResult<Authorization> Save(Authorization authorization);
    }
}
