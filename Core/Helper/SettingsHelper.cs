using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Core.Helper
{
    public static class SettingsHelper
    {
        public static string GetValue(string section, string settingKey)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("settings.json").Build();
            string empty = string.Empty;
            string message;
            try
            {
                message = configurationRoot.GetSection(section).GetChildren().Where<IConfigurationSection>((Func<IConfigurationSection, bool>)(x => x.Key == settingKey)).FirstOrDefault<IConfigurationSection>().Value;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
    }
}
