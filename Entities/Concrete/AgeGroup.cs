using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    public class AgeGroup:BaseEntity
    {
        public bool IsActive { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string CardCode { get; set; }

        public AgeGroup()
        {
            Id = 0;
            IsActive = false;
            ShortDescription = string.Empty;
            Description = string.Empty;
            CardCode = string.Empty;
        }
    }
}
