using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Color
{
    public class Color:BaseModel
    {
        public bool IsActive { get; set; }
        public bool IsUsed { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string DescriptionTr { get; set; }
        public string DescriptionEn { get; set; }
        public string PantoneNo { get; set; }
        public string Cmyk { get; set; }

        public Color():base()
        {
            IsActive = true;
            Date = DateTime.Now;
            Code = string.Empty;
            DescriptionTr = string.Empty;
            DescriptionEn = string.Empty;
            PantoneNo = string.Empty;
            Cmyk = string.Empty;
        }

        public Color(HttpRequest request, Entities.Concrete.Color color, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = color.Id;
            CustomerId = color.CustomerId;
            IsActive = color.IsActive;
            Date = color.Date;
            Code = color.Code;
            DescriptionTr = color.DescriptionTr;
            DescriptionEn = color.DescriptionEn;
            PantoneNo = color.PantoneNo;
            Cmyk = color.Cmyk;

        }

        public Entities.Concrete.Color GetBusinessModel()
        {
            Entities.Concrete.Color color = new Entities.Concrete.Color();
            if (EntityId > 0)
            {
                color.Id = EntityId;
            }

            color.CustomerId = CustomerId;
            color.IsActive = IsActive;
            color.Date = Date;
            color.Code = Code;
            color.DescriptionTr = DescriptionTr;
            color.DescriptionEn = DescriptionEn;
            color.PantoneNo = PantoneNo;
            color.Cmyk = Cmyk;


            return color;
        }
    }
}
