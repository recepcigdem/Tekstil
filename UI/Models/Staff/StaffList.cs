using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;

namespace UI.Models.Staff
{
    public class StaffList
    {
        public List<StaffListLine> data { get; set; }

        private IStaffService _staffService;

        public StaffList(IStaffService staffService)
        {
            _staffService = staffService;
            var listGrid = _staffService.GetAll().Data;
            data = new List<StaffListLine>();
            foreach (var item in listGrid)
            {
                StaffListLine line = new StaffListLine(item);
                data.Add(line);
            }
        }
    }
}
