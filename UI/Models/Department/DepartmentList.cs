using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;

namespace UI.Models.Department
{
    public class DepartmentList
    {
        public List<DepartmentListLine> data { get; set; }

        private IDepartmentService _departmentService;

        public DepartmentList(HttpRequest request,IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            var listGrid = _departmentService.GetAll(customerId).Data;
            if (listGrid != null)
            {
                data = new List<DepartmentListLine>();
                foreach (var item in listGrid)
                {
                    DepartmentListLine line = new DepartmentListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}

