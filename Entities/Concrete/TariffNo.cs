using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class TariffNo : BaseEntity
    {
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public TariffNo()
        {
            IsActive = false;

            Description = string.Empty;
            Code = string.Empty;
        }
    }
}
