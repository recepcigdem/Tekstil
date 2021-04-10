using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Department : BaseEntity
    {
        public bool IsActive { get; set; }
        public string DepartmentName { get; set; }

        public Department()
        {
            IsActive = false;
            DepartmentName = string.Empty;
        }
    }
}
