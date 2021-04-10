using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Gender : BaseEntity
    {
        public bool IsActive { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string CardCode { get; set; }

        public Gender()
        {
            IsActive = false;
            Description = string.Empty;
            ShortDescription = string.Empty;
            CardCode = string.Empty;
        }
    }
}
