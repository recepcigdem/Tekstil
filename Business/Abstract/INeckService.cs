using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface INeckService
    {
        IDataResult<List<Neck>> GetAll();
        IDataResult<Neck> GetById(int neckId);
        IResult Add(Neck neck);
        IResult Update(Neck neck);
        IResult Delete(Neck neck);
    }
}
