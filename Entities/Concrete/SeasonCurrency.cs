using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("SeasonCurrency", Schema = "season")]
    public class SeasonCurrency : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("isDefault")]
        public bool IsDefault { get; set; }
        [Column("currencyType")]
        public string CurrencyType { get; set; }
        [Column("exchangeRate")]
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
