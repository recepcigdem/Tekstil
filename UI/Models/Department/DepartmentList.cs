using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;

namespace UI.Models.Department
{
    public class DepartmentList
    {
        public List<DepartmentListLine> data { get; set; }

        private IDepartmentService _departmentService;

        public DepartmentList(IDepartmentService departmentService)
        {
            _departmentService = departmentService;

            var listGrid = _departmentService.GetAll().Data;
            data = new List<DepartmentListLine>();
            foreach (var item in listGrid)
            {
                DepartmentListLine line = new DepartmentListLine(item);
                data.Add(line);
            }
        }
    }
}

