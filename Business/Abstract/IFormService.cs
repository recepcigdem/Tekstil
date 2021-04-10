using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IFormService
    {
        IDataResult<List<Form>> GetAll();
        IDataResult<Form> GetById(int formId);
        IResult Add(Form form);
        IResult Update(Form form);
        IResult Delete(Form form);
    }
}
