using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IAgeGroupService
    {
        IDataResult<List<AgeGroup>> GetAll();
        IDataResult<AgeGroup> GetById(int ageGroupId);
        IDataResult<AgeGroup> Add(AgeGroup ageGroup);
        IDataResult<AgeGroup> Update(AgeGroup ageGroup);
        IResult Delete(AgeGroup ageGroup);
    }
}
