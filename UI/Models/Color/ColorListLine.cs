using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Color
{
    public class ColorListLine
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string DescriptionTr { get; set; }
        public string DescriptionEn { get; set; }
        public string PantoneNo { get; set; }
        public string Cmyk { get; set; }

        public ColorListLine() : base()
        {
            Id = 0;
            CustomerId = 0;
            IsActive = false;
            Date= DateTime.UtcNow;
            Code = string.Empty;
            DescriptionTr = string.Empty;
            DescriptionEn = string.Empty;
            PantoneNo = string.Empty;
            Cmyk = string.Empty;
        }
        public ColorListLine(Entities.Concrete.Color color)
        {
            Id = color.Id;
            CustomerId = color.CustomerId;
            IsActive = color.IsActive;
            Date = color.Date;
            Code = color.Code;
            DescriptionTr = color.DescriptionTr;
            DescriptionEn = color.DescriptionEn;
            PantoneNo = color.PantoneNo;
            Cmyk = color.Cmyk;
        }
    }
}
