using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class StaffPhone : BaseEntity
    {
        public int StaffId { get; set; }
        public int PhoneId { get; set; }
        public bool IsMain { get; set; }

        public StaffPhone()
        {
            StaffId = 0;
            PhoneId = 0;
            IsMain = false;
        }
    }
}
