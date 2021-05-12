using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICsNoDeliveryDateHistoryService
    {
        IDataServiceResult<List<CsNoDeliveryDateHistory>> GetAll(int customerId);
        IDataServiceResult<List<CsNoDeliveryDateHistory>> GetAllByCsNoDeliveryDate(int csNoDeliveryDateId);
        IDataServiceResult<CsNoDeliveryDateHistory> GetById(int csNoDeliveryDateHistoryId);
        IDataServiceResult<CsNoDeliveryDateHistory> GetByCsNoDeliveryDateId(int csNoDeliveryDateId);
        IServiceResult Add(CsNoDeliveryDateHistory csNoDeliveryDateHistory);
        IServiceResult Delete(CsNoDeliveryDateHistory csNoDeliveryDateHistory);
        IServiceResult DeleteByCsNoDeliveryDateId(int csNoDeliveryDateId);
    }
}
