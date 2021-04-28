using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Definition", Schema = "definition")]
    public class Definition: BaseCustomerEntity
    {
        [Column("definitionTitleId")]
        public int DefinitionTitleId { get; set; }
        [Column("categoryId")]
        public int CategoryId { get; set; }
        [Column("productGroupId")]
        public int ProductGroupId { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("isDefault")]
        public bool IsDefault { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("shortDescription")]
        public string ShortDescription { get; set; }
        [Column("descriptionTr")]
        public string DescriptionTr { get; set; }
        [Column("descriptionEn")]
        public string DescriptionEn { get; set; }
        [Column("kdv")]
        public int Kdv { get; set; }

        public Definition()
        {
            DefinitionTitleId = 0;
            CategoryId = 0;
            ProductGroupId = 0;
            Status = false;
            IsDefault = false;
            Code = string.Empty;
            ShortDescription = string.Empty;
            DescriptionTr = string.Empty;
            DescriptionEn = string.Empty;
            Kdv = 0;
        }
    }
}
