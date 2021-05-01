using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("CountryShippingMultiplier", Schema = "season")]
    public class CountryShippingMultiplier : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("countryId")]
        public int CountryId { get; set; }
        [Column("shippingMethodId")]
        public int ShippingMethodId { get; set; }
        [Column("seasonCurrencyId")]
        public int SeasonCurrencyId { get; set; }
        [Column("multiplier")]
        public int Multiplier { get; set; }
        [Column("testPrice")]
        public decimal TestPrice { get; set; }
        [Column("testPercentage")]
        public int TestPercentage { get; set; }
        [Column("cargoPercentage")]
        public int CargoPercentage { get; set; }
        [Column("insurancePercentage")]
        public int InsurancePercentage { get; set; }
        [Column("freightPercentage")]
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
