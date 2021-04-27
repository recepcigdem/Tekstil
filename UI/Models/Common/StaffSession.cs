using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Common
{
    public class StaffSession
    {
        public int StaffId { get; set; }
        public int CustomerId { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsCompanyAdmin { get; set; }

    }
}
