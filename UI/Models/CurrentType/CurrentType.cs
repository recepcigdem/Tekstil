using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.CurrentType
{
    public class CurrentType : BaseModel
    {
        public bool IsActive { get; set; }
        public string Description { get; set; }

        public CurrentType()
        {
            IsActive = false;
            Description = string.Empty;
        }
        public CurrentType(HttpRequest request, Entities.Concrete.CurrentType currentType, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = currentType.Id;
            CustomerId = currentType.CustomerId;
            IsActive = currentType.IsActive;
            Description = currentType.Description;
        }

        public Entities.Concrete.CurrentType GetBusinessModel()
        {
            Entities.Concrete.CurrentType currentType = new Entities.Concrete.CurrentType();
            if (EntityId > 0)
            {
                currentType.Id = EntityId;
            }

            currentType.CustomerId = CustomerId;
            currentType.IsActive = IsActive;
            currentType.Description = Description;

            return currentType;
        }
    }
}
