using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ILabelService
    {
        IDataResult<List<Label>> GetAll();
        IDataResult<Label> GetById(int labelId);
        IResult Add(Label label);
        IResult Update(Label label);
        IResult Delete(Label label);
    }
}
