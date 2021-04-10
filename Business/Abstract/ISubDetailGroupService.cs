using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISubDetailGroupService
    {
        IDataResult<List<SubDetailGroup>> GetAll();
        IDataResult<SubDetailGroup> GetById(int subDetailGroupId);
        IResult Add(SubDetailGroup subDetailGroup);
        IResult Update(SubDetailGroup subDetailGroup);
        IResult Delete(SubDetailGroup subDetailGroup);
    }
}
