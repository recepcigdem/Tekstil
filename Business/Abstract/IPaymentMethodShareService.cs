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
        IDataServiceResult<List<PaymentMethodShare>> GetAll(int customerId);
        IDataServiceResult<List<PaymentMethodShare>> GetAllBySeasonId(int seasonId);
        IDataServiceResult<List<PaymentMethodShare>> GetAllBySeasonCurrencyId(int customerId,int seasonCurrencyId);
        IDataServiceResult<List<PaymentMethodShare>> GetAllByPaymentMethodId(int customerId, int paymentMethodId);
        IDataServiceResult<PaymentMethodShare> GetById(int paymentMethodShareId);
        IServiceResult Add(PaymentMethodShare paymentMethodShare);
        IServiceResult Update(PaymentMethodShare paymentMethodShare);
        IServiceResult Delete(PaymentMethodShare paymentMethodShare);
        IServiceResult DeleteBySeason(Season season);
        IDataServiceResult<PaymentMethodShare> Save(int seasonId, int customerId, List<PaymentMethodShare> paymentMethodShares);
    }
}
