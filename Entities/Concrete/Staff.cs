using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;

namespace Entities.Concrete
{
    [Table("Staff", Schema = "staff")]
    public class Staff : BaseCustomerEntity
    {
        [Column("departmentId")]
        public int DepartmentId { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("passwordSalt")]
        public string PasswordSalt { get; set; }
        [Column("registerDate")]
        public DateTime RegisterDate { get; set; }
        [Column("isLeaving")]
        public bool IsLeaving { get; set; }
        [Column("leavingDate")]
        public DateTime LeavingDate { get; set; }
        [Column("isSendEmail")]
        public bool IsSendEmail { get; set; }
        [Column("photo")]
        public string Photo { get; set; }
        [Column("isSuperAdmin")]
        public bool IsSuperAdmin { get; set; }
        [Column("isCompanyAdmin")]
        public bool IsCompanyAdmin { get; set; }
        [Column("isStandartUser")]
        public bool IsStandartUser { get; set; }
        [Column("isSuperAdminControl")]
        public bool IsSuperAdminControl { get; set; }
        [Column("isCompanyAdminControl")]
        public bool IsCompanyAdminControl { get; set; }

        public List<OperationClaim> OperationClaims { get; set; }


        public Staff()
        {
            DepartmentId = 0;
            IsActive = false;
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            PasswordSalt = string.Empty;
            RegisterDate = DateTime.UtcNow;
            IsLeaving = false;
            LeavingDate = DateTime.UtcNow;
            IsSendEmail = false;
            Photo = string.Empty;
            IsSuperAdmin = false;
            IsCompanyAdmin = false;
            IsStandartUser = false;
            IsSuperAdminControl = false;
            IsCompanyAdminControl = false;
            OperationClaims = new List<OperationClaim>();
        }
    }
}
