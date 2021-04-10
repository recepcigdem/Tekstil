using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Brand : BaseEntity
    {
        public bool IsActive { get; set; }
        public string BrandName { get; set; }

        public Brand()
        {
            IsActive = false;
            BrandName = string.Empty;
        }
    }
}
