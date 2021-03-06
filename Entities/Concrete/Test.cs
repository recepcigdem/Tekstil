using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Test", Schema = "definition")]
    public class Test : BaseCustomerEntity
    {
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("norm")]
        public string Norm { get; set; }
        [Column("value")]
        public string Value { get; set; }

        public Test()
        {
            IsUsed = false;
            IsActive = true;
            Code = string.Empty;
            Description = string.Empty;
            Norm = string.Empty;
            Value = string.Empty;
        }

        
    }
}
