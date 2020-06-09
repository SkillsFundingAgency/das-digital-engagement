using System;
using System.IO;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Infrastructure.Interfaces.Marketo;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Application.Services.Marketo;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Framework.Infrastructure.Configuration;
using DAS.DigitalEngagement.Functions.Import;
using DAS.DigitalEngagement.Functions.Import.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.Configuration.AzureTableStorage;
using Refit;
using DAS.DigitalEngagement.Models.Infrastructure;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DAS.DigitalEngagement.Functions.Import
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup() { }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            builder.AddConfiguration((configBuilder) =>
            {
                var tempConfig = configBuilder
                    .Build();

                var configuration = configBuilder
                     //.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                     .AddAzureTableStorage(options =>
                     {
                         options.ConfigurationKeys = new[] { tempConfig.GetValue<string>("configName") };
                         options.EnvironmentNameEnvironmentVariableName = "EnvironmentName";
                         options.StorageConnectionStringEnvironmentVariableName = "ConfigurationStorageConnectionString";
                         options.PreFixConfigurationKeys = false;
                     })
                    .Build();

                return configuration;
            });

            Configuration = builder.GetCurrentConfiguration();
            ConfigureServices(builder.Services);

            


        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            var marketoConfig = Configuration.GetSection("Marketo").Get<MarketoConfiguration>();
            services.AddOptions();
            services.Configure<MarketoConfiguration>(Configuration.GetSection("Marketo"));

            services.AddTransient<IImportPersonHandler, ImportPersonHandler>();
            services.AddTransient<IChunkingService, ChunkingService>();
            services.AddTransient<ICsvService, CsvService>();
            services.AddTransient<IMarketoBulkImportService, BulkImportService>();
            services.AddTransient<OAuthHttpClientHandler>();

            var httpBuilder = services.AddRefitClient<IMarketoBulkImportClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(marketoConfig.ApiBaseUrl + marketoConfig.ApiBulkImportPrefix));
            httpBuilder.AddHttpMessageHandler<OAuthHttpClientHandler>();

            var executioncontextoptions = services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>().Value;
            var currentDirectory = executioncontextoptions.AppDirectory;

            var nLogConfiguration = new NLogConfiguration(currentDirectory);

            services.AddLogging((options) =>
            {
                options.SetMinimumLevel(LogLevel.Trace);
                options.SetMinimumLevel(LogLevel.Trace);
                options.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });

                nLogConfiguration.ConfigureNLog(Configuration);
            });


            services.RemoveAll<IConfigureOptions<LoggerFilterOptions>>();
            services.ConfigureOptions<LoggerFilterConfigureOptions>();

            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }
    }
}