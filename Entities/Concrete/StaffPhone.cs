using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("StaffPhone",Schema = "staff")]
    public class StaffPhone : BaseEntity
    {
        [Column("staffId")]
        public int StaffId { get; set; }
        [Column("phoneId")]
        public int PhoneId { get; set; }
        [Column("isMain")]
        public bool IsMain { get; set; }

        public StaffPhone()
        {
            StaffId = 0;
            PhoneId = 0;
            IsMain = false;
        }
    }
}
