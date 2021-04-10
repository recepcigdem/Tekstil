using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IDetailService
    {
        IDataResult<List<Detail>> GetAll();
        IDataResult<Detail> GetById(int detailId);
        IResult Add(Detail detail);
        IResult Update(Detail detail);
        IResult Delete(Detail detail);
    }
}
