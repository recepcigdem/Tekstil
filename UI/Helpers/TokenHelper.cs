using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Helpers
{
    public class TokenHelper
    {
        public static string CreateLoginToken(string userName)
        {
            var applicationName = Core.Helper.SettingsHelper.GetValue("Token", "ApplicationName").ToString();
            var gmtTarih = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss");

            var user = userName + "|" + applicationName + "|" + gmtTarih;

            var createToken = Core.Helper.StringHelper.Encrypt(user, Core.Helper.SettingsHelper.GetValue("Token", "ConfigSecret").ToString());

            return createToken;
        }
    }
}
