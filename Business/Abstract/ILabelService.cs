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
        IDataServiceResult<List<Label>> GetAll(int customerId);
        IDataServiceResult<Label> GetById(int labelId);
        IDataServiceResult<Label> Save(Label label);
        IServiceResult Delete(Label label);
    }
}
