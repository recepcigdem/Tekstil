using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFurService
    {
        IDataResult<List<Fur>> GetAll();
        IDataResult<Fur> GetById(int furId);
        IResult Add(Fur fur);
        IResult Update(Fur fur);
        IResult Delete(Fur fur);
    }
}
