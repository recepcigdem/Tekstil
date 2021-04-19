using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("StaffAuthorization", Schema = "staff")]
    public class StaffAuthorization : BaseEntity
    {
        [Column("staffId")]
        public int StaffId { get; set; }
        [Column("authorizationId")]
        public int AuthorizationId { get; set; }

        public StaffAuthorization()
        {
            StaffId = 0;
            AuthorizationId = 0;
        }
    }
}
