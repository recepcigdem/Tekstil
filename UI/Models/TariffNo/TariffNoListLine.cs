using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.TariffNo
{
    public class TariffNoListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        
        public TariffNoListLine() 
        {
            Id = 0;
            IsActive = false;
            Description = string.Empty;
            Code = string.Empty;
        }

        public TariffNoListLine(Entities.Concrete.TariffNo tariffNo)
        {
            Id = tariffNo.Id;
            IsActive = tariffNo.IsActive;
            Description = tariffNo.Description;
            Code = tariffNo.Code;
        }
    }
}
