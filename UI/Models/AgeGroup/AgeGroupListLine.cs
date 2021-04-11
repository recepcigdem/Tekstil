using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.AgeGroup
{
    public class AgeGroupListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string CardCode { get; set; }

        public AgeGroupListLine() : base()
        {
            Id = 0;
            IsActive = false;
            ShortDescription = string.Empty;
            Description = string.Empty;
            CardCode = string.Empty;
        }
        public AgeGroupListLine(Entities.Concrete.AgeGroup ageGroup)
        {
            Id = ageGroup.Id;
            IsActive = ageGroup.IsActive;
            ShortDescription = ageGroup.ShortDescription;
            Description = ageGroup.Description;
            CardCode = ageGroup.CardCode;
        }

    }
}
