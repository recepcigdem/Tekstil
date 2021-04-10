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
        IDataResult<List<Test>> GetAll();
        IDataResult<Test> GetById(int testId);
        IResult Add(Test test);
        IResult Update(Test test);
        IResult Delete(Test test);
    }
}
