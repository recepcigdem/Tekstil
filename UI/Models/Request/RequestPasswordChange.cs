using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Request
{
    public class RequestPasswordChange
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
