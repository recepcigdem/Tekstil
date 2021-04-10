using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Authorization : BaseEntity
    {
   
        public string AuthorizationName { get; set; }

        public Authorization()
        {
            AuthorizationName = string.Empty;
        }
    }
}
