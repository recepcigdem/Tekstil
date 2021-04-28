using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Http;
using UI.Models.Authorization;
using UI.Models.DefinitionTitle;

namespace UI.Models.Definition
{
    public class DefinitionList
    {
        public List<DefinitionListLine> data { get; set; }

        private IDefinitionService _definitionService;
        private IDefinitionTitleService _definitionTitleService;
        public DefinitionList(HttpRequest request, IDefinitionService definitionService, IDefinitionTitleService definitionTitleService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _definitionService = definitionService;
            _definitionTitleService = definitionTitleService;

            var listGrid = _definitionService.GetAll(customerId);
            data = new List<DefinitionListLine>();
            if (listGrid !=null )
            {
                foreach (var item in listGrid.Data)
                {
                    DefinitionListLine line = new DefinitionListLine(item);
                    var definitionTitle = _definitionTitleService.GetById(item.DefinitionTitleId).Data;
                    line.DefinitionTitle = definitionTitle == null ? "" : definitionTitle.Title;
                    data.Add(line);
                }
            }
        }
    }
}
