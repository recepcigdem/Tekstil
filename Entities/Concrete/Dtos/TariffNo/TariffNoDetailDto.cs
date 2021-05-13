using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Dtos.TariffNo
{
    public class TariffNoDetailDto : BaseCustomerEntity
    {
      
        public int TariffNoId { get; set; }
        public int SeasonId { get; set; }
        public int CountryId { get; set; }
        public int SeasonCurrencyId { get; set; }
        public bool IsUsed { get; set; }
        public int Tax { get; set; }
        public int Stamp { get; set; }
        public int AzoTest { get; set; }
        public int DifferentExpense { get; set; }
        public int Commission { get; set; }
        public int Kkdf { get; set; }
        public int AdditionalTax { get; set; }
        public int AdditionalTax1 { get; set; }
        public int Superintendence { get; set; }
        public string UnitKg { get; set; }
        public decimal KgTranslation { get; set; }


        public string Season { get; set; }
        public string Country { get; set; }
        public string SeasonCurrency { get; set; }

        public TariffNoDetailDto()
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

            Season = string.Empty;
            Country = string.Empty;
            SeasonCurrency = string.Empty;
        }
    }
}
