using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SubBrand : BaseEntity
    {
        public int BrandId { get; set; }
        public bool IsActive { get; set; }
        public string SubBrandName { get; set; }

        public SubBrand()
        {
            BrandId = 0;
            IsActive = false;
            SubBrandName = string.Empty;
        }
    }
}
