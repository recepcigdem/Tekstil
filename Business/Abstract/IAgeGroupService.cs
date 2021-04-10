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
        IResult Add(AgeGroup ageGroup);
        IResult Update(AgeGroup ageGroup);
        IResult Delete(AgeGroup ageGroup);
    }
}
