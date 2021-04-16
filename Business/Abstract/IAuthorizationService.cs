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
        IDataResult<List<Authorization>> GetAll();
        IDataResult<Authorization> GetById(int authorizationId);
        IResult Add(Authorization authorization);
        IResult Update(Authorization authorization);
        IResult Delete(Authorization authorization);
        IResult DeleteByAuthorizationId(int authorizationId);
        IResult Save(Authorization authorization);
    }
}
