using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Request
{
    public class Register
    {
        public int StaffId { get; set; }
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsLeaving { get; set; }
        public DateTime LeavingDate { get; set; }
        public bool IsSendMail { get; set; }
        public string Photo { get; set; }

        public bool IsSuperAdmin { get; set; }
        public bool IsCompanyAdmin { get; set; }
    }
}
