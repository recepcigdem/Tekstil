using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category : BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsGarment { get; set; }
        public bool IsAccessory { get; set; }
        public bool IsDecoration { get; set; }
        public string Description { get; set; }
        public string CardCode { get; set; }
        public int Kdv { get; set; }

        public Category()
        {
            IsActive = false;
            IsGarment = false;
            IsAccessory = false;
            IsDecoration = false;
            Description = string.Empty;
            CardCode = string.Empty;
            Kdv = 0;
        }
    }
}
