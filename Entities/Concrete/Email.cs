using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Email : BaseEntity
    {
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }

        public Email()
        {
            IsActive = false;
            EmailAddress = string.Empty;
        }
    }
}
