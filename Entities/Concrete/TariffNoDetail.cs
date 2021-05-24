#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("TariffNoDetail", Schema = "definition")]
    public class TariffNoDetail : BaseCustomerEntity
    {
        [Column("tariffNoId")]
        public int TariffNoId { get; set; }

        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("countryId")]
        public int CountryId { get; set; }
        [Column("seasonCurrencyId")]
        public int? SeasonCurrencyId { get; set; }

        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("tax")]
        public int? Tax { get; set; }
        [Column("stamp")]
        public int? Stamp { get; set; }
        [Column("azoTest")]
        public int? AzoTest { get; set; }
        [Column("differentExpense")]
        public int? DifferentExpense { get; set; }
        [Column("commission")]
        public int? Commission { get; set; }
        [Column("kkdf")]
        public int? Kkdf { get; set; }
        [Column("additionalTax")]
        public int? AdditionalTax { get; set; }
        [Column("additionalTax1")]
        public int? AdditionalTax1 { get; set; }
        [Column("superintendence")]
        public int? Superintendence { get; set; }
        [Column("unitKg")]
        public string? UnitKg { get; set; }
        [Column("kgTranslation")]
        public decimal? KgTranslation { get; set; }
       
        public TariffNoDetail()
        {
            TariffNoId = 0;
            SeasonId = 0;
            CountryId = 0;
            SeasonCurrencyId = 0;
            IsUsed = false;
            Tax = 0;
            Stamp = 0;
            AzoTest = 0;
            DifferentExpense = 0;
            Commission = 0;
            Kkdf = 0;
            AdditionalTax = 0;
            AdditionalTax1 = 0;
            Superintendence = 0;
            UnitKg = string.Empty;
            KgTranslation = 0;
        }
    }
}
