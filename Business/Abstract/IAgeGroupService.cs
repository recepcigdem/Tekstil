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
        IServiceResult<List<AgeGroup>> GetAll();
        IServiceResult<AgeGroup> GetById(int ageGroupId);
        IServiceResult<AgeGroup> Add(AgeGroup ageGroup);
        IServiceResult<AgeGroup> Update(AgeGroup ageGroup);
        IServiceResult<AgeGroup> Save(AgeGroup ageGroup);
        IServiceResult<AgeGroup> Delete(AgeGroup ageGroup);
    }
}
