using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IStyleService
    {
        IDataResult<List<Style>> GetAll();
        IDataResult<Style> GetById(int styleId);
        IResult Add(Style style);
        IResult Update(Style style);
        IResult Delete(Style style);
    }
}
