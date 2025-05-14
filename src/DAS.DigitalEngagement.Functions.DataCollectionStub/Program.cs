using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostBuilderContext, builder) =>
    {
        var configuration = hostBuilderContext.Configuration;
        builder.AddConfiguration(configuration);
    })
    .ConfigureLogging(logging =>
    {
        logging.AddFilter("Microsoft", LogLevel.Warning);
        logging.AddFilter("System", LogLevel.Warning);
        logging.AddFilter("DAS.DigitalEngagement", LogLevel.Information);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddApplicationInsightsTelemetryWorkerService(options =>
        {
            options.ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        });
    })
    .Build();

host.Run();