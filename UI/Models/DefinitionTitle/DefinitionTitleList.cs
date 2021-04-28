using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Http;
using UI.Models.Authorization;
using UI.Models.DefinitionTitle;

namespace UI.Models.DefinitionTitle
{
    public class DefinitionTitleList
    {
        public List<DefinitionTitleListLine> data { get; set; }

        private IDefinitionTitleService _definitionTitleService;

        public DefinitionTitleList(HttpRequest request,IDefinitionTitleService definitionTitleService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _definitionTitleService = definitionTitleService;

            var listGrid = _definitionTitleService.GetAll(customerId).Data;
            if (listGrid != null)
            {
                data = new List<DefinitionTitleListLine>();
                foreach (var item in listGrid)
                {
                    DefinitionTitleListLine line = new DefinitionTitleListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
