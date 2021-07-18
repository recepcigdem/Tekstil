using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("FabricPrice", Schema = "fabric")]
    public class FabricPrice : BaseCustomerEntity
    {
        [Column("fabricId")]
        public int FabricId { get; set; }
        [Column("fabricSupplierId")]
        public int FabricSupplierId { get; set; }
        [Column("currencyTypeId")]
        public int CurrencyTypeId { get; set; }
        [Column("translationCurrencyTypeId")]
        public int TranslationCurrencyTypeId { get; set; }
        [Column("isOrginal")]
        public bool IsOrginal { get; set; }
        [Column("isRevision")]
        public bool IsRevision { get; set; }
        [Column("isImpotedDomestic")]
        public bool IsImpotedDomestic { get; set; }
        [Column("templateNo")]
        public string TemplateNo { get; set; }
        [Column("unit")]
        public string Unit { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("unitMultiplier")]
        public int UnitMultiplier { get; set; }
        [Column("totalPrice")]
        public int TotalPrice { get; set; }
        [Column("importCoefficient")]
        public decimal ImportCoefficient { get; set; }
        [Column("contractPrice")]
        public decimal ContractPrice { get; set; }
        [Column("exchangeRate")]
        public decimal ExchangeRate { get; set; }
        [Column("translationExchangeRate")]
        public decimal TranslationExchangeRate { get; set; }
        [Column("finalTranslationPrice")]
        public decimal FinalTranslationPrice { get; set; }

        public FabricPrice()
        {
            FabricId = 0;
            FabricSupplierId = 0;
            CurrencyTypeId = 0;
            TranslationCurrencyTypeId = 0;
            IsOrginal = false;
            IsRevision = false;
            IsImpotedDomestic = false;
            TemplateNo = string.Empty;
            Unit = string.Empty;
            Price = 0;
            UnitMultiplier = 0;
            TotalPrice = 0;
            ImportCoefficient = 0;
            ContractPrice = 0;
            ExchangeRate = 0;
            TranslationExchangeRate = 0;
            FinalTranslationPrice = 0;


        }
    }
}
