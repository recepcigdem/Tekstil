using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Hierarchy : BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsGarmentAccessory { get; set; }
        public string Code { get; set; }
        public string TotalDescription { get; set; }
        public string Brand { get; set; }
        public string Gender { get; set; }
        public string MainProductGroup { get; set; }
        public string Detail { get; set; }
        public string ProductGroup { get; set; }
        public string SubProductGroup { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }

        public Hierarchy()
        {
            IsActive = false;
            IsGarmentAccessory = true;
            Code = string.Empty;
            TotalDescription = string.Empty;
            Brand = string.Empty;
            Gender = string.Empty;
            MainProductGroup = string.Empty;
            Detail = string.Empty;
            ProductGroup = string.Empty;
            SubProductGroup = string.Empty;
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;
            Description4 = string.Empty;
            
        }
    }
}
