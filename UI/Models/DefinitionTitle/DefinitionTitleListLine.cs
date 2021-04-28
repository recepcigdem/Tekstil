using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.DefinitionTitle
{
    public class DefinitionTitleListLine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }

        public DefinitionTitleListLine() : base()
        {
            Id = 0;
            Title = string.Empty;
            Value = 0;
        }
        public DefinitionTitleListLine(Entities.Concrete.DefinitionTitle definitionTitle)
        {
            Id = definitionTitle.Id;
            Title = definitionTitle.Title;
            Value = definitionTitle.Value;
        }

    }
}
