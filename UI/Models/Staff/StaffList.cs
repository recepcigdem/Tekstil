using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Http;
using UI.Helpers;

namespace UI.Models.Staff
{
    public class StaffList
    {
        public List<StaffListLine> data { get; set; }

        private IStaffService _staffService;

        public StaffList(HttpRequest request ,IStaffService staffService)
        {
            var customerId= SessionHelper.GetStaff(request).CustomerId;
            _staffService = staffService;
            var listGrid = _staffService.GetAll(customerId).Data;
            data = new List<StaffListLine>();
            foreach (var item in listGrid)
            {
                StaffListLine line = new StaffListLine(item);
                data.Add(line);
            }
        }
    }
}
