using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IHierarchyService
    {
        IDataResult<List<Hierarchy>> GetAll();
        IDataResult<Hierarchy> GetById(int hierarchyId);
        IResult Add(Hierarchy hierarchy);
        IResult Update(Hierarchy hierarchy);
        IResult Delete(Hierarchy hierarchy);
    }
}
