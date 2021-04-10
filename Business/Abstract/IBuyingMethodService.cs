using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBuyingMethodService
    {
        IDataResult<List<BuyingMethod>> GetAll();
        IDataResult<BuyingMethod> GetById(int buyingMethodId);
        IResult Add(BuyingMethod buyingMethod);
        IResult Update(BuyingMethod buyingMethod);
        IResult Delete(BuyingMethod buyingMethod);
    }
}
