using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class StaffAuthorization : BaseEntity
    {
        public int StaffId { get; set; }
        public int AuthorizationId { get; set; }

        public StaffAuthorization()
        {
            StaffId = 0;
            AuthorizationId = 0;
        }
    }
}
