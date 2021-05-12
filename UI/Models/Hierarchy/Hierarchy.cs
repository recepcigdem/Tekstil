using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Hierarchy
{
    public class Hierarchy : BaseModel
    {
        
        public bool IsActive { get; set; }
        public bool IsUsed { get; set; }  
        public bool IsGarmentAccessory { get; set; }     
        public string Code { get; set; }     
        public string TotalDescription { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }
        public int MainProductGroupId { get; set; }
        public int DetailId { get; set; }
        public int ProductGroupId { get; set; }
        public int SubProductGroupId { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }

        public Hierarchy() : base()
        {
            IsActive = true;
            IsUsed = false;
            IsGarmentAccessory = true;
            Code = string.Empty;
            TotalDescription = string.Empty;
            BrandId = 0;
            GenderId = 0;
            MainProductGroupId = 0;
            DetailId = 0;
            ProductGroupId = 0;
            SubProductGroupId = 0;
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;
            Description4 = string.Empty;

        }

        public Hierarchy(HttpRequest request, Entities.Concrete.Hierarchy hierarchy, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = hierarchy.Id;
            CustomerId = hierarchy.CustomerId;
            IsActive = hierarchy.IsActive;
            IsGarmentAccessory = hierarchy.IsGarmentAccessory;
            Code = hierarchy.Code;
            TotalDescription = hierarchy.TotalDescription;
            BrandId = hierarchy.BrandId;
            GenderId = hierarchy.GenderId;
            MainProductGroupId = hierarchy.MainProductGroupId;
            DetailId = hierarchy.DetailId;
            ProductGroupId = hierarchy.ProductGroupId;
            SubProductGroupId = hierarchy.SubProductGroupId;
            Description1 = hierarchy.Description1;
            Description2 = hierarchy.Description2;
            Description3 = hierarchy.Description3;
            Description4 = hierarchy.Description4;
        }

        public Entities.Concrete.Hierarchy GetBusinessModel()
        {
            Entities.Concrete.Hierarchy hierarchy = new Entities.Concrete.Hierarchy();
            if (EntityId > 0)
            {
                hierarchy.Id = EntityId;
            }

            hierarchy.CustomerId = CustomerId;
            hierarchy.IsActive = IsActive;
            hierarchy.IsGarmentAccessory = IsGarmentAccessory;
            hierarchy.Code = Code;
            hierarchy.TotalDescription = TotalDescription;
            hierarchy.BrandId = BrandId;
            hierarchy.GenderId = GenderId;
            hierarchy.MainProductGroupId = MainProductGroupId;
            hierarchy.DetailId = DetailId;
            hierarchy.ProductGroupId = ProductGroupId;
            hierarchy.SubProductGroupId = SubProductGroupId;
            hierarchy.Description1 = Description1;
            hierarchy.Description2 = Description2;
            hierarchy.Description3 = Description3;
            hierarchy.Description4 = Description4;


            return hierarchy;
        }
    }
}
