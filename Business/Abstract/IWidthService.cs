using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IWidthService
    {
        IDataResult<List<Width>> GetAll();
        IDataResult<Width> GetById(int widthId);
        IResult Add(Width width);
        IResult Update(Width width);
        IResult Delete(Width width);
    }
}
