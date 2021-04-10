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
        IDataResult<List<CsNoDeliveryDate>> GetAll();
        IDataResult<CsNoDeliveryDate> GetById(int csNoDeliveryDateId);
        IResult Add(CsNoDeliveryDate csNoDeliveryDate);
        IResult Update(CsNoDeliveryDate csNoDeliveryDate);
        IResult Delete(CsNoDeliveryDate csNoDeliveryDate);
    }
}
