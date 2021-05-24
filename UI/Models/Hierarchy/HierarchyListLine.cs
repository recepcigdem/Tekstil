using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Hierarchy
{
    public class HierarchyListLine
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsUsed { get; set; }
        public bool IsGarmentAccessory { get; set; }
        public string Code { get; set; }
        public string TotalDescription { get; set; }
        public int? BrandId { get; set; }
        public int? GenderId { get; set; }
        public int? MainProductGroupId { get; set; }
        public int? DetailId { get; set; }
        public int? ProductGroupId { get; set; }
        public int? SubProductGroupId { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description3 { get; set; }
        public string? Description4 { get; set; }

        public HierarchyListLine() : base()
        {
            Id = 0;
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

        public HierarchyListLine(Entities.Concrete.Hierarchy hierarchy)
        {
            Id = hierarchy.Id;
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
    }
}
