using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IHoodService
    {
        IDataResult<List<Hood>> GetAll();
        IDataResult<Hood> GetById(int hoodId);
        IResult Add(Hood hood);
        IResult Update(Hood hood);
        IResult Delete(Hood hood);
    }
}
