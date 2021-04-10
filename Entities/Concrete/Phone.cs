using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Phone : BaseEntity
    {
        public bool IsActive { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }

        public Phone()
        {
            IsActive = false;
            CountryCode = string.Empty;
            AreaCode = string.Empty;
            PhoneNumber= string.Empty;
        }
    }
}
