using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Hierarchy
{
    public class HierarchyList
    {

        public List<HierarchyListLine> data { get; set; }

        private IHierarchyService _hierarchyService;

        public HierarchyList(HttpRequest request, IHierarchyService hierarchyService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _hierarchyService = hierarchyService;

            var listGrid = _hierarchyService.GetAll(customerId);

            if (listGrid != null)
            {
                data = new List<HierarchyListLine>();

                foreach (var item in listGrid.Data)
                {
                    HierarchyListLine line = new HierarchyListLine(item);
                    data.Add(line);
                }
            }
        }

    }
}
