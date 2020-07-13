using Microsoft.Extensions.Configuration;

namespace DAS.DigitalEngagement.Framework.Infrastructure.Configuration
{
    static class ConfigurationHelper
    {
        public static string GetEnvironmentName(this IConfiguration configuration)
        {
            return configuration.GetConnectionStringOrSetting("EnvironmentName");
        }

        public static string GetAppName(this IConfiguration configuration)
        {
            return configuration.GetConnectionStringOrSetting("APPSETTING_AppName");
        }

        public static string GetAzureStorageConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionStringOrSetting("AzureWebJobsStorage");
        }
    }
}
