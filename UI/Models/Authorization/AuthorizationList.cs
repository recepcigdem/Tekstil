using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;

namespace UI.Models.Authorization
{
    public class AuthorizationList
    {
        public List<AuthorizationListLine> data { get; set; }

        private IAuthorizationService _authorizationService;

        public AuthorizationList(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;

            var listGrid = _authorizationService.GetAll().Data;
            if (listGrid != null)
            {
                data = new List<AuthorizationListLine>();
                foreach (var item in listGrid)
                {
                    AuthorizationListLine line = new AuthorizationListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
