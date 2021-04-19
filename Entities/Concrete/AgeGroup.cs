using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    [Table("AgeGroup", Schema = "modelDefinition")]
    public class AgeGroup : BaseEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("shortDescription")]
        public string ShortDescription { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("cardCode")]
        public string CardCode { get; set; }

        public AgeGroup()
        {
            Id = 0;
            IsActive = false;
            ShortDescription = string.Empty;
            Description = string.Empty;
            CardCode = string.Empty;
        }
    }
}
