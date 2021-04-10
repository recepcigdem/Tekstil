using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IManufacturingHeadersService
    {
        IDataResult<List<ManufacturingHeaders>> GetAll();
        IDataResult<ManufacturingHeaders> GetById(int manufacturingHeadersId);
        IResult Add(ManufacturingHeaders manufacturingHeaders);
        IResult Update(ManufacturingHeaders manufacturingHeaders);
        IResult Delete(ManufacturingHeaders manufacturingHeaders);
    }
}
