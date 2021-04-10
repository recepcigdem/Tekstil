using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ILinerService
    {
        IDataResult<List<Liner>> GetAll();
        IDataResult<Liner> GetById(int linerId);
        IResult Add(Liner liner);
        IResult Update(Liner liner);
        IResult Delete(Liner liner);
    }
}
