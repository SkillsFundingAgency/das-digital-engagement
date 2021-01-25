 
using System.Collections.Generic;
using DAS.DigitalEngagement.Application.Handlers.Configure;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Mapping;
using DAS.DigitalEngagement.Application.Mapping.Interfaces;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Application.Services.Marketo;
using DAS.DigitalEngagement.Domain.Configure;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Domain.Mapping.BulkImport;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Framework.Infrastructure.Configuration;
using DAS.DigitalEngagement.Functions.Import;
using DAS.DigitalEngagement.Functions.Import.Extensions;
using DAS.DigitalEngagement.Infrastructure.Configuration;
using DAS.DigitalEngagement.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.Configuration.AzureTableStorage;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerUsers.Api.Client;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DAS.DigitalEngagement.Functions.Import
{
    public class Startup : FunctionsStartup
    {
        public Startup() { }
      
        public override void Configure(IFunctionsHostBuilder builder)
        {                                    
            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var configBuilder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            if (!configuration["EnvironmentName"].Equals("LOCAL_ACCEPTANCE_TESTS", StringComparison.CurrentCultureIgnoreCase))
            {
                configBuilder.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigName"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                });
            }

            configBuilder.AddJsonFile("local.settings.json", optional: true);

            var config = configBuilder.Build();

            ConfigureServices(builder.Services, config);

        }

        public void ConfigureServices(IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<IEmployerUsersApiConfiguration>(Configuration.GetSection("EmployerUsersApi"));
            services.Configure<List<DataMartSettings>>(Configuration.GetSection("DataMart"));

            services.AddOptions();


            services.AddTransient<IImportPersonHandler, ImportPersonHandler>();
            services.AddTransient<IImportCampaignMembersHandler, ImportCampaignMembersHandler>();
            services.AddTransient<IChunkingService, ChunkingService>();
            services.AddTransient<ICsvService, CsvService>();
            services.AddTransient<IBulkImportService, MarketoBulkImportService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IBulkImportStatusMapper, BulkImportStatusMapper>();
            services.AddTransient<IBulkImportJobMapper, BulkImportJobMapper>();
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IImportEmployerUsersHandler, ImportEmployerUsersHandler>();
            services.AddTransient<IEmployerUsersRepository, EmployerUsersRepository>();
            services.AddTransient<IImportDataMartHandler, ImportDataMartHandler>();
            services.AddTransient<IDataModelConfigurationService, MarketoDataModelConfigurationService>();
            services.AddTransient<IConfigureDataModelHandler, ConfigureDataModelHandler>();
            services.AddTransient<ICreateCustomObjectFieldsRequestMapping, CreateCustomObjectFieldsRequestMapping>();
            services.AddTransient<ICreateCustomObjectSchemaRequestMapping, CreateCustomObjectSchemaRequestMapping>();




            services.AddTransient<IBlobContainerClientWrapper, BlobContainerClientWrapper>(x =>
                new BlobContainerClientWrapper(Configuration.GetValue<string>("AzureWebJobsStorage")));

            services.AddTransient<IPersonMapper, PersonMapper>();

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

            services.AddMarketoClient(Configuration);
            services.AddEmployerUsersClient(Configuration);
            services.AddDatamartConfiguration(Configuration);
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }
    }
}
