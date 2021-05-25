using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.CurrentType
{
    public class CurrentTypeListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }

        public CurrentTypeListLine() : base()
        {
            Id = 0;
            IsActive = false;
            Description = string.Empty;
        }
        public CurrentTypeListLine(Entities.Concrete.CurrentType currentType)
        {
            Id = currentType.Id;
            IsActive = currentType.IsActive;
            Description = currentType.Description;
        }
    }
}
