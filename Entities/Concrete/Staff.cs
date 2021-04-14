using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Staff : BaseEntity
    {
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsLeaving { get; set; }
        public DateTime LeavingDate { get; set; }
        public bool IsSendMail { get; set; }
        public string Photo { get; set; }

        public bool IsSuperAdmin { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public bool IsStandartUser { get; set; }
        public bool IsSuperAdminControl { get; set; }
        public bool IsCompanyAdminControl { get; set; }


        public Staff()
        {
            CustomerId = 0;
            DepartmentId = 0;
            IsActive = false;
            Title = string.Empty;
            FirstName = string.Empty;
            LastName= string.Empty;
            Password = string.Empty;
            PasswordSalt = string.Empty;
            RegisterDate =DateTime.UtcNow;
            IsLeaving = false;
            LeavingDate = DateTime.UtcNow;
            IsSendMail = false;
            Photo = string.Empty;
            IsSuperAdmin = false;
            IsCompanyAdmin = false;
            IsStandartUser = false;
            IsSuperAdminControl = false;
            IsCompanyAdminControl = false;
        }
    }
}
