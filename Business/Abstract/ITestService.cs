using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ITestService
    {
        IDataServiceResult<List<Test>> GetAll(int customerId);
        IDataServiceResult<Test> GetById(int testId);
        IDataServiceResult<Test> Save(Test test);
        IServiceResult Delete(Test test);
    }
}
