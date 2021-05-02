using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Dtos.Season
{
    public class SeasonCountryShippingMultiplierDto:BaseCustomerEntity
    {
        public int SeasonId { get; set; }
        public int CountryId { get; set; }
        public int ShippingMethodId { get; set; }
        public int SeasonCurrencyId { get; set; }
        public int Multiplier { get; set; }
        public decimal TestPrice { get; set; }
        public int TestPercentage { get; set; }
        public int CargoPercentage { get; set; }
        public int InsurancePercentage { get; set; }
        public int FreightPercentage { get; set; }

        public string Country { get; set; }
        public string ShippingMethod { get; set; }
        public string SeasonCurrency { get; set; }
        public decimal ExchangeRates { get; set; }
        public decimal TestPriceTl { get; set; }
    }
}
