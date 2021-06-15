using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Dtos.Current
{
    public class CurrentEmailDto :BaseEntity
    {
        public int CurrentId { get; set; }
        public int EmailId { get; set; }
        public bool IsMain { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }

        public CurrentEmailDto()
        {
            CurrentId = 0;
            EmailId = 0;
            IsMain = false;

            IsActive = true;
            EmailAddress = string.Empty;
        }
    }
}
