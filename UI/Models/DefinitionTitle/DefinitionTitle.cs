using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.DefinitionTitle
{
    public class DefinitionTitle : BaseModel
    {
       
        public string Title { get; set; }
        public int Value { get; set; }

        public DefinitionTitle() : base()
        {
            EntityId = 0;
            Title = string.Empty;
            Value = 0;
        }
        public DefinitionTitle(HttpRequest request, Entities.Concrete.DefinitionTitle definitionTitle, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = definitionTitle.Id;
            Title = definitionTitle.Title;
            Value = definitionTitle.Value;
        }

        public Entities.Concrete.DefinitionTitle GetBusinessModel()
        {
            Entities.Concrete.DefinitionTitle definitionTitle = new Entities.Concrete.DefinitionTitle();
            if (EntityId > 0)
            {
                definitionTitle.Id = this.EntityId;
            }
            definitionTitle.Title = Title;
            definitionTitle.Value = Value;
            

            return definitionTitle;
        }
    }
}
