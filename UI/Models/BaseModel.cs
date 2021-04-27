using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UI.Models
{
    public class BaseModel
    {
        public string Culture { get; set; }
        public int EntityId { get; set; }

        public int CustomerId { get; set; }

        public string DateFormat { get; set; }


        public BaseModel()
        {

            Culture = "tr";

        }
        public BaseModel(HttpRequest request)
        {
            Culture = Helpers.HttpHelper.GetRequestCulture(request);
            if (request.HttpContext.Session.GetString("Staff") != null)
            {
                CustomerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            }


            if (Culture == "tr")
                DateFormat = "DD.MM.YYYY";
            else
                DateFormat = "MM/DD/YYYY";
        }
    }
}
