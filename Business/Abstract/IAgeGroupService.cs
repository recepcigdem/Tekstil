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
        IDataServiceResult<List<AgeGroup>> GetAll();
        IDataServiceResult<AgeGroup> GetById(int ageGroupId);
        IServiceResult Add(AgeGroup ageGroup);
        IServiceResult Update(AgeGroup ageGroup);
        IDataServiceResult<AgeGroup> Save(AgeGroup ageGroup);
        IServiceResult Delete(AgeGroup ageGroup);
    }
}
