using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UI.Models.Login
{
    public class Login :BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public Login() : base()
        {
            //
        }
        public Login(HttpRequest request) : base(request)
        {
            //
        }
    }
}
