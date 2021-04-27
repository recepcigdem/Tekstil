using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Department
{
    public class Department : BaseModel
    {
        public bool IsActive { get; set; }
        public string DepartmentName { get; set; }

        public Department()
        {
            IsActive = false;
            DepartmentName = string.Empty;
        }
        public Department(HttpRequest request, Entities.Concrete.Department department, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = department.Id;
            //CustomerId = department.CustomerId;
            IsActive = department.IsActive;
            DepartmentName = department.DepartmentName;
        }

        public Entities.Concrete.Department GetBusinessModel()
        {
            Entities.Concrete.Department department = new Entities.Concrete.Department();
            if (EntityId > 0)
            {
                department.Id = EntityId;
            }

            department.CustomerId = CustomerId;
            department.IsActive = IsActive;
            department.DepartmentName = DepartmentName;

            return department;
        }
    }
}
