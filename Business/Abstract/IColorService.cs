using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IColorService
    {
        IDataServiceResult<List<Color>> GetAll(int customerId);
        IDataServiceResult<Color> GetById(int colorId);
        IServiceResult Delete(Color color);
        IDataServiceResult<Color> Save(Color color);
    }
}
