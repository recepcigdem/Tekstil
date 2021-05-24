
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Definition
{
    public class DefinitionListLine
    {
        public int Id { get; set; }
        public int DefinitionTitleId { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductGroupId { get; set; }
        public bool Status { get; set; }
        public bool? IsDefault { get; set; }
        public string? Code { get; set; }
        public string? ShortDescription { get; set; }
        public string DescriptionTr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? Kdv { get; set; }

        public string DefinitionTitle { get; set; }

        public DefinitionListLine() : base()
        {
            Id = 0;
            DefinitionTitleId = 0;
            CategoryId = 0;
            ProductGroupId = 0;
            Status = false;
            IsDefault = false;
            Code = string.Empty;
            ShortDescription = string.Empty;
            DescriptionTr = string.Empty;
            DescriptionEn = string.Empty;
            Kdv = 0;
        }
        public DefinitionListLine(Entities.Concrete.Definition definition)
        {
            Id = definition.Id;
            DefinitionTitleId = definition.DefinitionTitleId;
            CategoryId = definition.CategoryId;
            ProductGroupId = definition.ProductGroupId;
            Status = definition.Status;
            IsDefault = definition.IsDefault;
            Code = definition.Code;
            ShortDescription = definition.ShortDescription;
            DescriptionTr = definition.DescriptionTr;
            DescriptionEn = definition.DescriptionEn;
            Kdv = definition.Kdv;
        }

    }
}
