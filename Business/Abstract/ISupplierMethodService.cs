using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISupplierMethodService
    {
        IDataResult<List<SupplierMethod>> GetAll();
        IDataResult<SupplierMethod> GetById(int supplierMethodId);
        IResult Add(SupplierMethod supplierMethod);
        IResult Update(SupplierMethod supplierMethod);
        IResult Delete(SupplierMethod supplierMethod);
    }
}
