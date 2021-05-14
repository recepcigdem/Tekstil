using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.TariffNo
{
    public class TariffNoList
    {
        public List<TariffNoListLine> data { get; set; }

        private ITariffNoService _tariffNoService;

        public TariffNoList(HttpRequest request, ITariffNoService tariffNoService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _tariffNoService = tariffNoService;

            var listGrid = _tariffNoService.GetAll(customerId);

            if (listGrid != null)
            {
                data = new List<TariffNoListLine>();

                foreach (var item in listGrid.Data)
                {
                    TariffNoListLine line = new TariffNoListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
