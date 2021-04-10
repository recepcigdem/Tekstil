using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ShippingMethod : BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ShippingMethod()
        {
            IsActive = false;
            IsDefault = false;
            Code = string.Empty;
            Description = string.Empty;
        }
    }
}
