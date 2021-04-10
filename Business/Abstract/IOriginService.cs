using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IOriginService
    {
        IDataResult<List<Origin>> GetAll();
        IDataResult<Origin> GetById(int originId);
        IResult Add(Origin origin);
        IResult Update(Origin origin);
        IResult Delete(Origin origin);
    }
}
