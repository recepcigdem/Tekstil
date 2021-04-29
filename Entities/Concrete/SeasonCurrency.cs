using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SeasonCurrency : BaseCustomerEntity
    {
        public int SeasonId { get; set; }
        public bool IsDefault { get; set; }
        public string CurrencyType { get; set; }
        public decimal ExchangeRate { get; set; }

        public SeasonCurrency()
        {
            SeasonId = 0;
            IsDefault = false;
            CurrencyType = string.Empty;
            ExchangeRate = 0;
        }
    }
}
