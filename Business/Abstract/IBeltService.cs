using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBeltService
    {
        IDataResult<List<Belt>> GetAll();
        IDataResult<Belt> GetById(int beltId);
        IResult Add(Belt belt);
        IResult Update(Belt belt);
        IResult Delete(Belt belt);
    }
}
