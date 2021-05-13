using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Label", Schema = "definition")]
    public class Label : BaseCustomerEntity
    {
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("image")]
        public string Image { get; set; }

        public Label()
        {
            IsUsed = false;
            IsActive = true;
            Code = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
        }
    }
}
