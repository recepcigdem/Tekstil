using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Test : BaseEntity
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Norm { get; set; }
        public string Value { get; set; }

        public Test()
        {
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
            Norm = string.Empty;
            Value = string.Empty;
        }
    }
}
