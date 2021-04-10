using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISupplierService
    {
        IDataResult<List<Supplier>> GetAll();
        IDataResult<Supplier> GetById(int supplierId);
        IResult Add(Supplier supplier);
        IResult Update(Supplier supplier);
        IResult Delete(Supplier supplier);
    }
}
