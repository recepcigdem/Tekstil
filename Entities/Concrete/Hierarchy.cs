
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Hierarchy", Schema = "definition")]
    public class Hierarchy : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("isGarmentAccessory")]
        public bool IsGarmentAccessory { get; set; }
        [Column("code")]
        public string? Code { get; set; }
        [Column("totalDescription")]
        public string TotalDescription { get; set; }
        [Column("brandId")]
        public int? BrandId { get; set; }
        [Column("genderId")]
        public int? GenderId { get; set; }
        [Column("mainProductGroupId")]
        public int? MainProductGroupId { get; set; }
        [Column("detailId")]
        public int? DetailId { get; set; }
        [Column("productGroupId")]
        public int? ProductGroupId { get; set; }
        [Column("subProductGroupId")]
        public int? SubProductGroupId { get; set; }
        [Column("description1")]
        public string? Description1 { get; set; }
        [Column("description2")]
        public string? Description2 { get; set; }
        [Column("description3")]
        public string? Description3 { get; set; }
        [Column("description4")]
        public string? Description4 { get; set; }

        public Hierarchy()
        {
            IsActive = true;
            IsUsed = false;
            IsGarmentAccessory = true;
            Code = string.Empty;
            TotalDescription = string.Empty;
            BrandId = 0;
            GenderId = 0;
            MainProductGroupId = 0;
            DetailId = 0;
            ProductGroupId = 0;
            SubProductGroupId = 0;
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;
            Description4 = string.Empty;
            
        }
    }
}
