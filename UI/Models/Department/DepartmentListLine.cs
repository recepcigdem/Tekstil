using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Department
{
    public class DepartmentListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string DepartmentName { get; set; }

        public DepartmentListLine() : base()
        {
            Id = 0;
            IsActive = false;
            DepartmentName = string.Empty;
        }
        public DepartmentListLine(Entities.Concrete.Department department)
        {
            Id = department.Id;
            IsActive = department.IsActive;
            DepartmentName = department.DepartmentName;
        }
    }
}
