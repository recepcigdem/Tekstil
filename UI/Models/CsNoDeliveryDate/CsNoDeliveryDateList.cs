using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.CsNoDeliveryDate
{
    public class CsNoDeliveryDateList
    {
        public List<CsNoDeliveryDateListLine> data { get; set; }

        private ICsNoDeliveryDateService _csNoDeliveryDateService;
        private ISeasonService _seasonService;
        public CsNoDeliveryDateList(HttpRequest request, ICsNoDeliveryDateService csNoDeliveryDateService, ISeasonService seasonService)
        {
            _csNoDeliveryDateService = csNoDeliveryDateService;
            _seasonService = seasonService;

            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;

            var listGrid = _csNoDeliveryDateService.GetAll(customerId);
            data = new List<CsNoDeliveryDateListLine>();
            if (listGrid != null)
            {
                foreach (var item in listGrid.Data)
                {
                    CsNoDeliveryDateListLine line = new CsNoDeliveryDateListLine(item);
                    
                    var season = _seasonService.GetById(item.SeasonId);
                    line.Season = season == null ? "" : season.Data.Code;
                   
                    
                    data.Add(line);
                }
            }
           
        }
    }
}
