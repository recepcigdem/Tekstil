using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("StaffEmail",Schema = "staff")]
    public class StaffEmail : BaseEntity
    {
        [Column("staffId")]
        public int StaffId { get; set; }
        [Column("emailId")]
        public int EmailId { get; set; }
        [Column("isMain")]
        public bool IsMain { get; set; }

        public StaffEmail()
        {
            StaffId = 0;
            EmailId = 0;
            IsMain = false;
        }
    }
}
