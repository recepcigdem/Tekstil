using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Authorization
{
    public class Authorization :BaseModel
    {
        public string AuthorizationName { get; set; }

        public Authorization()
        {
            EntityId = 0;
            AuthorizationName = string.Empty;
        }

        public Authorization(HttpRequest request, Entities.Concrete.Authorization authorization, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = authorization.Id;
            AuthorizationName = authorization.AuthorizationName;
        }

        public Entities.Concrete.Authorization GetBusinessModel()
        {
            Entities.Concrete.Authorization authorization = new Entities.Concrete.Authorization();
            if (EntityId>0)
            {
                authorization.Id = EntityId;
            }

            authorization.AuthorizationName = AuthorizationName;

            return authorization;
        }
    }
}
