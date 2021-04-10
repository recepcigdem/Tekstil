using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IArmService
    {
        IDataResult<List<Arm>> GetAll();
        IDataResult<Arm> GetById(int armId);
        IResult Add(Arm arm);
        IResult Update(Arm arm);
        IResult Delete(Arm arm);
    }
}
