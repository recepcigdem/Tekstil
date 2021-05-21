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
        IDataServiceResult<List<CsNoDeliveryDate>> GetAllBySeasonId(int customerId,int seasonId);
        IDataServiceResult<CsNoDeliveryDate> GetById(int csNoDeliveryDateId);
        IDataServiceResult<CsNoDeliveryDate> GetBySeasonId(int seasonId);
        IDataServiceResult<CsNoDeliveryDate> Save(int staffId,CsNoDeliveryDate csNoDeliveryDate);
        IServiceResult Delete(CsNoDeliveryDate csNoDeliveryDate);
    }
}
