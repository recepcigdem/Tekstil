using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CountryShippingMultiplier : BaseEntity
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

        public CountryShippingMultiplier()
        {
            SeasonId = 0;
            CountryId = 0;
            ShippingMethodId = 0;
            SeasonCurrencyId = 0;
            Multiplier = 0;
            TestPrice = 0;
            TestPercentage = 0;
            CargoPercentage = 0;
            InsurancePercentage = 0;
            FreightPercentage = 0;
        }
    }
}
