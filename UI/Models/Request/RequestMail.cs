using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Request
{
    public class RequestMail
    {
        public string TemplateName { get; set; }
        public string BaseUrl { get; set; }
        public string ProjectName { get; set; }
        public string LogoUrl { get; set; }
        public string CompanyName { get; set; }
        public string MailAddress { get; set; }
        public string MailHeader { get; set; }
        public string MailInfo { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
}
