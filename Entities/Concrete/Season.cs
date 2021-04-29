using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Season : BaseCustomerEntity
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Season()
        {
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
        }
    }
}
