using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Fabric", Schema = "fabric")]
    public class Fabric : BaseCustomerEntity
    {
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [Column("categoryId")]
        public int CategoryId { get; set; }
        [Column("complatedUserId")]
        public int ComplatedUserId { get; set; }
        [Column("labelId")]
        public int LabelId { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("isComplated")]
        public bool IsComplated { get; set; }
        [Column("complatedDate")]
        public DateTime ComplatedDate { get; set; }
        [Column("definition")]
        public string Definition { get; set; }
        [Column("fabricCode")]
        public string FabricCode { get; set; }
        [Column("fabricNameTr")]
        public string FabricNameTr { get; set; }
        [Column("fabricNameEn")]
        public string FabricNameEn { get; set; }
        [Column("fabricGroup")]
        public string FabricGroup { get; set; }
        [Column("fabricType")]
        public string FabricType { get; set; }
        [Column("unit")]
        public string Unit { get; set; }
        [Column("minimumProductionQuantity")]
        public int MinimumProductionQuantity { get; set; }
        [Column("minimumProductionQuantityColor")]
        public int MinimumProductionQuantityColor { get; set; }
        [Column("fabricMixTr")]
        public string FabricMixTr { get; set; }
        [Column("fabricMixEn")]
        public string FabricMixEn { get; set; }
        [Column("notes")]
        public string Notes { get; set; }

        public Fabric()
        {
            SeasonId = 0;
            CategoryId = 0;
            ComplatedUserId = 0;
            LabelId = 0;
            IsActive = true;
            IsUsed = false;
            IsComplated = false;
            ComplatedDate = new DateTime(2000,01,01);
            Definition = string.Empty;
            FabricCode = string.Empty;
            FabricNameTr = string.Empty;
            FabricNameEn = string.Empty;
            FabricGroup = string.Empty;
            FabricType = string.Empty;
            Unit = string.Empty;
            MinimumProductionQuantity = 0;
            MinimumProductionQuantityColor = 0;
            FabricMixTr = string.Empty;
            FabricMixEn = string.Empty;
            Notes = string.Empty;
        }
    }
}
