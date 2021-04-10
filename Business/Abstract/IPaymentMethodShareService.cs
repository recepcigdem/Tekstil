using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPaymentMethodShareService
    {
        IDataResult<List<PaymentMethodShare>> GetAll();
        IDataResult<PaymentMethodShare> GetById(int paymentMethodShareId);
        IResult Add(PaymentMethodShare paymentMethodShare);
        IResult Update(PaymentMethodShare paymentMethodShare);
        IResult Delete(PaymentMethodShare paymentMethodShare);
    }
}
