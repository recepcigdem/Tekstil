using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Staff
{
    public class StaffListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public StaffListLine() : base()
        {
            Id = 0;
            IsActive = false;
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
        }
        public StaffListLine(Entities.Concrete.Staff staff)
        {
            Id = staff.Id;
            IsActive = staff.IsActive;
            Title = staff.Title;
            FirstName = staff.FirstName;
            LastName = staff.LastName;
        }

    }
}
