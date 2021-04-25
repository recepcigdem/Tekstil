using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IDepartmentService
    {
        IDataServiceResult<List<Department>> GetAll();
        IDataServiceResult<Department> GetById(int departmentId);
        IServiceResult Add(Department department);
        IServiceResult Update(Department department);
        IServiceResult Delete(Department department);
        IDataServiceResult<Department> Save(Department department);
    }
}
