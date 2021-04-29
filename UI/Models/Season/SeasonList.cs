using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using UI.Helpers;

namespace UI.Models.Season
{
    public class SeasonList
    {

        public List<SeasonListLine> data { get; set; }

        private ISeasonService _seasonService;

        public SeasonList(HttpRequest request, ISeasonService seasonService)
        {
            var customerId = SessionHelper.GetStaff(request).CustomerId;
            _seasonService = seasonService;
            var listGrid = _seasonService.GetAll(customerId).Data;
            if (listGrid != null)
            {
                data = new List<SeasonListLine>();
                foreach (var item in listGrid)
                {
                    SeasonListLine line = new SeasonListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
