using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Test
{
    public class TestListLine
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Norm { get; set; }
        public string Value { get; set; }

        public TestListLine()
        {
            Id = 0;
            IsUsed = false;
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
            Norm = string.Empty;
            Value = string.Empty;
        }
        public TestListLine(Entities.Concrete.Test test) : base()
        {
            Id = test.Id;
            IsActive = test.IsActive;
            Code = test.Code;
            Description = test.Description;
            Norm = test.Norm;
            Value = test.Value;
        }
    }
}
