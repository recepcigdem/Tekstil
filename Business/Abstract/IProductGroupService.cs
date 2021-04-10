using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProductGroupService
    {
        IDataResult<List<ProductGroup>> GetAll();
        IDataResult<ProductGroup> GetById(int productGroupId);
        IResult Add(ProductGroup productGroup);
        IResult Update(ProductGroup productGroup);
        IResult Delete(ProductGroup productGroup);
    }
}
