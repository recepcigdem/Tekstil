using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("FabricDetail", Schema = "fabric")]
    public class FabricDetail : BaseCustomerEntity
    {
        [Column("fabricId")]
        public int FabricId { get; set; }
        [Column("colorId")]
        public int ColorId { get; set; }
        [Column("csNoId")]
        public int CsNoId { get; set; }
        [Column("productGroupId")]
        public int ProductGroupId { get; set; }
        [Column("washingId")]
        public int WashingId { get; set; }
        [Column("currencyTypeId")]
        public int CurrencyTypeId { get; set; }
        [Column("supplierId")]
        public int SupplierId { get; set; }
        [Column("rowNumber")]
        public string RowNumber { get; set; }
        [Column("fabricMixTr")]
        public string FabricMixTr { get; set; }
        [Column("fabricMixEn")]
        public string FabricMixEn { get; set; }
        [Column("picture")]
        public string Picture { get; set; }
        [Column("mainMiniMidi")]
        public string MainMiniMidi { get; set; }
        [Column("planningQuantity")]
        public int PlanningQuantity{ get; set; }
        [Column("unitQuantity")]
        public int UnitQuantity { get; set; }
        [Column("totalNeed")]
        public int TotalNeed { get; set; }
        [Column("importedDomestic")]
        public string ImportedDomestic { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("description")]
        public string Description { get; set; }


        public FabricDetail()
        {
            FabricId = 0;
            ColorId = 0;
            CsNoId = 0;
            ProductGroupId = 0;
            WashingId = 0;
            CurrencyTypeId = 0;
            SupplierId = 0;
            RowNumber = string.Empty;
            FabricMixTr = string.Empty;
            FabricMixEn = string.Empty;
            Picture = string.Empty;
            MainMiniMidi = string.Empty;
            PlanningQuantity = 0;
            UnitQuantity = 0;
            TotalNeed = 0;
            ImportedDomestic = string.Empty;
            Price = 0;
            Description = string.Empty;

        }
    }
}
