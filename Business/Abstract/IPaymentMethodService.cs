using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPaymentMethodService
    {
        IDataResult<List<PaymentMethod>> GetAll();
        IDataResult<PaymentMethod> GetById(int paymentMethodId);
        IResult Add(PaymentMethod paymentMethod);
        IResult Update(PaymentMethod paymentMethod);
        IResult Delete(PaymentMethod paymentMethod);
    }
}
