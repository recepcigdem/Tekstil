using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Width : BaseEntity
    {
        public int ProductGroupId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Width()
        {
            ProductGroupId = 0;
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
        }
    }
}
