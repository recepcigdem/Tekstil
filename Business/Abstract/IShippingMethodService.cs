using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IShippingMethodService
    {
        IDataResult<List<ShippingMethod>> GetAll();
        IDataResult<ShippingMethod> GetById(int shippingMethodId);
        IResult Add(ShippingMethod shippingMethod);
        IResult Update(ShippingMethod shippingMethod);
        IResult Delete(ShippingMethod shippingMethod);
    }
}
