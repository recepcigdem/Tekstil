using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Style : BaseEntity
    {
        public int ProductGroupId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Style()
        {
            ProductGroupId = 0;
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
        }
    }
}
