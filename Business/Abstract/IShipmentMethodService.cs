using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IShipmentMethodService
    {
        IDataResult<List<ShipmentMethod>> GetAll();
        IDataResult<ShipmentMethod> GetById(int shipmentMethodId);
        IResult Add(ShipmentMethod shipmentMethod);
        IResult Update(ShipmentMethod shipmentMethod);
        IResult Delete(ShipmentMethod shipmentMethod);
    }
}
