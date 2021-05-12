using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICsNoDeliveryDateService
    {
        IDataServiceResult<List<CsNoDeliveryDate>> GetAll(int customerId);
        IDataServiceResult<CsNoDeliveryDate> GetById(int csNoDeliveryDateId);
        IDataServiceResult<CsNoDeliveryDate> Save(int staffId,CsNoDeliveryDate csNoDeliveryDate);
        IServiceResult Delete(CsNoDeliveryDate csNoDeliveryDate);
    }
}
