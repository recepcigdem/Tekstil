using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class StaffEmail : BaseEntity
    {
        public int StaffId { get; set; }
        public int EmailId { get; set; }
        public bool IsMain { get; set; }

        public StaffEmail()
        {
            StaffId = 0;
            EmailId = 0;
            IsMain = false;
        }
    }
}
