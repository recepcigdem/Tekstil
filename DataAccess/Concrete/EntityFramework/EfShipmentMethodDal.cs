using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfShipmentMethodDal : EfEntityRepositoryBase<ShipmentMethod, TekstilContext>, IShipmentMethodDal
    {
    }
}
