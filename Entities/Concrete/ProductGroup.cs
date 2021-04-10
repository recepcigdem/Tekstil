using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductGroup : BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsGarment { get; set; }
        public bool IsAccessory { get; set; }
        public bool IsDecoration { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Kdv { get; set; }

        public ProductGroup()
        {
            IsActive = false;
            IsGarment = false;
            IsAccessory = false;
            IsDecoration = false;
            Code = string.Empty;
            Description = string.Empty;
            Kdv = 0;
        }
    }
}
