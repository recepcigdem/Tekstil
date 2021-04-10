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
        IDataResult<List<CsNoDeliveryDateHistory>> GetAll();
        IDataResult<CsNoDeliveryDateHistory> GetByCsNoDeliveryDateId(int csNoDeliveryDateId);
        IResult Add(CsNoDeliveryDateHistory csNoDeliveryDateHistory);
        IResult Update(CsNoDeliveryDateHistory csNoDeliveryDateHistory);
        IResult Delete(CsNoDeliveryDateHistory csNoDeliveryDateHistory);
    }
}
