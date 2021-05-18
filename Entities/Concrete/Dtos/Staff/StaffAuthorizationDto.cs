using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Dtos.Staff
{
    public class StaffAuthorizationDto : BaseEntity
    {
        public int StaffId { get; set; }
        public int AuthorizationId { get; set; }
        public string AuthorizationName { get; set; }

        public StaffAuthorizationDto()
        {
            StaffId = 0;
            AuthorizationId = 0;
            AuthorizationName = string.Empty;
        }
    }
}
