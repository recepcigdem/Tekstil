using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Authorization
{
    public class AuthorizationListLine
    {
        public int Id { get; set; }
        public string AuthorizationName { get; set; }

        public AuthorizationListLine() : base()
        {
            Id = 0;
            AuthorizationName = string.Empty;
        }
        public AuthorizationListLine(Entities.Concrete.Authorization authorization)
        {
            Id = authorization.Id;
            AuthorizationName = authorization.AuthorizationName;
        }

    }
}
