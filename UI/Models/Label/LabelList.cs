using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Label
{
    public class LabelList
    {
        public List<LabelListLine> data { get; set; }

        private ILabelService _labelService;

        public LabelList(HttpRequest request, ILabelService labelService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _labelService = labelService;

            var listGrid = _labelService.GetAll(customerId);

            if (listGrid != null)
            {
                data = new List<LabelListLine>();

                foreach (var item in listGrid.Data)
                {
                    LabelListLine line = new LabelListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
