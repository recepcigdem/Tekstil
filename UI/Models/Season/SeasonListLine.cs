using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Season
{
    public class SeasonListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public SeasonListLine():base()
        {
            Id = 0;
            IsActive = false;
            Code = string.Empty;
            Description = string.Empty;
        }

        public SeasonListLine(Entities.Concrete.Season season)
        {
            Id = season.Id;
            IsActive = season.IsActive;
            Code = season.Code;
            Description = season.Description;
        }
    }
}
