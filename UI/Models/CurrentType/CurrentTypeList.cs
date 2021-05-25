using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;

namespace UI.Models.CurrentType
{
    public class CurrentTypeList
    {
        public List<CurrentTypeListLine> data { get; set; }

        private ICurrentTypeService _currentTypeService;

        public CurrentTypeList(HttpRequest request, ICurrentTypeService currentTypeService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;

            _currentTypeService = currentTypeService;

            var listGrid = _currentTypeService.GetAll(customerId).Data;
            if (listGrid != null)
            {
                data = new List<CurrentTypeListLine>();
                foreach (var item in listGrid)
                {
                    CurrentTypeListLine line = new CurrentTypeListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}

