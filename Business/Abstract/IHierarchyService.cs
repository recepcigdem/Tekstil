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
        IDataServiceResult<List<Hierarchy>> GetAll(int customerId);
        IDataServiceResult<List<Hierarchy>> GetAllByBrandId(int customerId,int brandId);
        IDataServiceResult<List<Hierarchy>> GetAllByGenderId(int customerId, int genderId);
        IDataServiceResult<List<Hierarchy>> GetAllByMainProductGroupId(int customerId, int mainProductGroupId);
        IDataServiceResult<List<Hierarchy>> GetAllByDetailId(int customerId, int detailId);
        IDataServiceResult<List<Hierarchy>> GetAllByProductGroupId(int customerId, int productGroupId);
        IDataServiceResult<List<Hierarchy>> GetAllBySubProductGroupId(int customerId, int subProductGroupId);
        IDataServiceResult<Hierarchy> GetById(int hierarchyId);
        IDataServiceResult<Hierarchy> Save(Hierarchy hierarchy);
        IServiceResult Delete(Hierarchy hierarchy);
    }
}
