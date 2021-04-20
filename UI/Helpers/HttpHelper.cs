using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using UI.Models.Common;
using UI.Models.Response;

namespace UI.Helpers
{
    public class HttpHelper
    {
        public static string GetRequestCulture(HttpRequest request)
        {
            var rqf = request.HttpContext.Features.Get<IRequestCultureFeature>();

            var culture = rqf.RequestCulture.Culture;
            return culture.TwoLetterISOLanguageName;
        }

        public static Response GetDataFromAPI(string url, string token, object data)
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = Core.Helper.SettingsHelper.GetValue("Info", "BaseURL");
                //string url = "token/verifytoken";
                client.Headers.Add("Token", token);
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string serializeData = JsonConvert.SerializeObject(data);
                var response = client.UploadString(url, serializeData);

                Response result = JsonConvert.DeserializeObject<Response>(response);

                return result;
            }
        }

        public static Response StaffSessionControl(HttpRequest request)
        {
            StaffSession resultUserSession = Helpers.SessionHelper.GetStaff(request);
            if (resultUserSession == null)
            {
                return new Response(false, "200", "Error_StaffNotFound", string.Empty);

            }
            //string url = "token/verifytoken";

            //var tokenResult = Helpers.HttpHelper.GetDataFromAPI(url, resultUserSession.Token,
            //    new Entities.Concrete.Staff() { Id = resultUserSession.StaffId });
            //if (!tokenResult.IsSuccess)
            //{
            //    return new Response(false, "200", tokenResult.ErrorMessage, string.Empty);

            //}
            return new Response(true, "200", string.Empty, string.Empty);
        }

    }
}
