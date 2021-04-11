using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;

namespace UI.Utils
{
    public class CustomCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            await Task.Yield();

            string culture = string.Empty;

            string url = httpContext.Request.Path.Value.Substring(1);
            if (string.IsNullOrWhiteSpace(url))
                return new ProviderCultureResult("en-US");

            int pos = url.IndexOf('/');
            if (pos < 0)
                culture = url;

            if (string.IsNullOrWhiteSpace(culture))
                culture = url.Substring(0, pos);

            if (culture == "tr")
                return new ProviderCultureResult("tr-TR");
            else if (culture == "en")
                return new ProviderCultureResult("en-US");
            else
                return new ProviderCultureResult("en-US");
        }
    }
}
