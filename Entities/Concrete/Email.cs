using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    [Table("Email", Schema = "staff")]
    public class Email : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }

        [Column("emailAddress")]
        public string EmailAddress { get; set; }

        public Email()
        {
            IsActive = false;
            EmailAddress = string.Empty;
        }
    }
}
