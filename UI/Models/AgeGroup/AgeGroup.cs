using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.AgeGroup
{
    public class AgeGroup : BaseModel
    {
        public bool IsActive { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string CardCode { get; set; }

        public AgeGroup() : base()
        {
            EntityId = 0;
            IsActive = false;
            ShortDescription = string.Empty;
            Description = string.Empty;
            CardCode = string.Empty;
        }
        public AgeGroup(HttpRequest request, Entities.Concrete.AgeGroup ageGroup, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = ageGroup.Id;
            IsActive = ageGroup.IsActive;
            ShortDescription = ageGroup.ShortDescription;
            Description = ageGroup.Description;
            CardCode = ageGroup.CardCode;
        }

        public Entities.Concrete.AgeGroup GetBusinessModel()
        {
            Entities.Concrete.AgeGroup ageGroup = new Entities.Concrete.AgeGroup();
            if (EntityId > 0)
            {
                ageGroup.Id = this.EntityId;
            }
            ageGroup.IsActive = this.IsActive;
            ageGroup.ShortDescription = this.ShortDescription;
            ageGroup.Description = this.Description;
            ageGroup.CardCode = this.CardCode;

            return ageGroup;
        }
    }
}
