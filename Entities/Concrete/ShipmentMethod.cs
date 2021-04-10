using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ShipmentMethod : BaseEntity
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ShipmentMethod()
        {
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
        }
    }
}
