using Microsoft.Extensions.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace DAS.DigitalEngagement.Functions.Import.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration BuildDasConfiguration(this IConfigurationBuilder configBuilder)
        {
            configBuilder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddEnvironmentVariables();

#if DEBUG
            configBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
#endif
            var configuration = configBuilder.Build();

            configBuilder.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = configuration["configName"].Split(",");
                options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                options.EnvironmentName = configuration["EnvironmentName"];
                options.PreFixConfigurationKeys = false;
            });

            return configBuilder.Build();
        }
    }
}