using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Color", Schema = "definition")]
    public class Color : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("descriptionTr")]
        public string DescriptionTr { get; set; }
        [Column("descriptionEn")]
        public string DescriptionEn { get; set; }

        [Column("pantoneNo")]
        public string PantoneNo { get; set; }

        [Column("cmyk")]
        public string Cmyk { get; set; }

        public Color()
        {
            IsActive = true;
            Date= DateTime.Now;
            Code = string.Empty;
            DescriptionTr = string.Empty;
            DescriptionEn = string.Empty;
            PantoneNo = string.Empty;
            Cmyk = string.Empty;
        }

    }
}
