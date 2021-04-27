using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Department", Schema = "staff")]
    public class Department : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("departmentName")]
        public string DepartmentName { get; set; }

        public Department()
        {
            IsActive = false;
            DepartmentName = string.Empty;
        }
    }
}
