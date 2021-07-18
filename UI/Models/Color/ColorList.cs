using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Color
{
    public class ColorList
    {
        public List<ColorListLine> data { get; set; }

        private IColorService _colorService;

        public ColorList(HttpRequest request, IColorService colorService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _colorService = colorService;

            var listGrid = _colorService.GetAll(customerId);

            if (listGrid != null)
            {
                data = new List<ColorListLine>();

                foreach (var item in listGrid.Data)
                {
                    ColorListLine line = new ColorListLine(item);
                    line.PantoneNo = line.PantoneNo == null ? "" : line.PantoneNo;
                    line.Cmyk = line.Cmyk == null ? "" : line.Cmyk;
                    data.Add(line);
                }
            }
        }
    }
}
