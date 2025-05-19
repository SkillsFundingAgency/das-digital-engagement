using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting;

namespace DAS.DigitalEngagement.Functions.DataCollection.Extensions;

public static partial class ConfigureNServiceBusExtension
{
    const string ErrorEndpointName = "SFA.DAS.DigitalEngagement.Functions.DataCollection-error";

    public static IHostBuilder ConfigureNServiceBus(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseNServiceBus((configuration, endpointConfiguration) =>
        {
            endpointConfiguration.Transport.SubscriptionRuleNamingConvention = AzureRuleNameShortener.Shorten;

            endpointConfiguration.AdvancedConfiguration.EnableInstallers();

            TimeSpan regexTimeout = TimeSpan.FromSeconds(1);

            endpointConfiguration.AdvancedConfiguration.Conventions()
                .DefiningCommandsAs(t => Regex.IsMatch(t.Name, "Command(V\\d+)?$", RegexOptions.None, regexTimeout))
                .DefiningEventsAs(t => Regex.IsMatch(t.Name, "Event(V\\d+)?$", RegexOptions.None, regexTimeout));

            endpointConfiguration.AdvancedConfiguration.SendFailedMessagesTo(ErrorEndpointName);

            var persistence = endpointConfiguration.AdvancedConfiguration.UsePersistence<AzureTablePersistence>();
            persistence.ConnectionString(configuration["AzureWebJobsStorage"]);

            var decodedLicence = WebUtility.HtmlDecode(configuration["NServiceBusConfiguration:NServiceBusLicense"]);
            endpointConfiguration.AdvancedConfiguration.License(decodedLicence);
        });
        return hostBuilder;
    }
}
