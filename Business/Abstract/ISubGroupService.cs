using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISubGroupService
    {
        IDataResult<List<SubGroup>> GetAll();
        IDataResult<SubGroup> GetById(int subGroupId);
        IResult Add(SubGroup subGroup);
        IResult Update(SubGroup subGroup);
        IResult Delete(SubGroup subGroup);
    }
}
