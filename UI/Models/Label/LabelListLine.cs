using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Label
{
    public class LabelListLine
    {

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
       

        public LabelListLine() : base()
        {
            Id = 0;
            IsActive = true;
            Code = string.Empty;
            Description = string.Empty;
           
        }
        public LabelListLine(Entities.Concrete.Label label)
        {
            Id = label.Id;           
            IsActive = label.IsActive;         
            Code = label.Code;
            Description = label.Description;
           
        }
    }
}
