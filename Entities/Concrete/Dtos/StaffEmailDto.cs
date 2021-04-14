using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Dtos
{
    public class StaffEmailDto :BaseEntity
    {
        public int StaffId { get; set; }
        public int EmailId { get; set; }
        public bool IsMain { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }

        public StaffEmailDto()
        {
            StaffId = 0;
            EmailId = 0;
            IsMain = false;

            IsActive = false;
            EmailAddress = string.Empty;
        }
    }
}
