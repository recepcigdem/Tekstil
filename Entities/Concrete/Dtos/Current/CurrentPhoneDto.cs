using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete.Dtos.Current
{
    public class CurrentPhoneDto : BaseEntity
    {
        public int CurrentId { get; set; }
        public int PhoneId { get; set; }
        public bool IsMain { get; set; }

        public bool IsActive { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }

        public CurrentPhoneDto()
        {
            CurrentId = 0;
            PhoneId = 0;
            IsMain = false;

            IsActive = true;
            CountryCode = string.Empty;
            AreaCode = string.Empty;
            PhoneNumber = string.Empty;
        }
    }
}
