using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;

namespace UI.Models.Current
{
    public class CurrentList
    {
        public List<CurrentListLine> data { get; set; }

        private ICurrentService _currentService;

        public CurrentList(HttpRequest request, ICurrentService currentService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _currentService = currentService;

            var listGrid = _currentService.GetAll(true, customerId).Data;
            if (listGrid != null)
            {
                data = new List<CurrentListLine>();
                foreach (var item in listGrid)
                {
                    CurrentListLine line = new CurrentListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}