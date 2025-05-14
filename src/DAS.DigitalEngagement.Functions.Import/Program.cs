using DAS.DigitalEngagement.Application.Handlers.Configure;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Application.Mapping.Interfaces;
using DAS.DigitalEngagement.Application.Mapping;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Application.Services.Marketo;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.Configure;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Mapping.BulkImport;
using DAS.DigitalEngagement.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerUsers.Api.Client;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Infrastructure.Configuration;
using Das.Marketo.RestApiClient.Configuration;
using Azure.Storage.Blobs;
using DAS.DigitalEngagement.Functions.Import.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostBuilderContext, builder) => { builder.BuildDasConfiguration(); })
    .ConfigureLogging(logging =>
    {
        logging.AddFilter("Microsoft", LogLevel.Warning);
        logging.AddFilter("System", LogLevel.Warning);
        logging.AddFilter("DAS.DigitalEngagement", LogLevel.Information);
    })

    .ConfigureServices((context, s) =>
    {
        var configuration = context.Configuration;

        s.AddOptions();

        s.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        s.Configure<IEmployerUsersApiConfiguration>(configuration.GetSection("EmployerUsersApi"));
        s.Configure<List<DataMartSettings>>(configuration.GetSection("DataMart"));

        s.AddTransient<IImportPersonHandler, ImportPersonHandler>();
        s.AddTransient<IImportCampaignMembersHandler, ImportCampaignMembersHandler>();
        s.AddTransient<IChunkingService, ChunkingService>();
        s.AddTransient<ICsvService, CsvService>();
        s.AddTransient<IBulkImportService, MarketoBulkImportService>();
        s.AddTransient<IReportService, ReportService>();
        s.AddTransient<IBulkImportStatusMapper, BulkImportStatusMapper>();
        s.AddTransient<IBulkImportJobMapper, BulkImportJobMapper>();
        s.AddTransient<IBlobService, BlobService>();
        s.AddTransient<IImportEmployerUsersHandler, ImportEmployerUsersHandler>();
        s.AddTransient<IEmployerUsersRepository, EmployerUsersRepository>();
        s.AddTransient<IImportDataMartHandler, ImportDataMartHandler>();
        s.AddTransient<IDataModelConfigurationService, MarketoDataModelConfigurationService>();
        s.AddTransient<IConfigureDataModelHandler, ConfigureDataModelHandler>();
        s.AddTransient<ICreateCustomObjectFieldsRequestMapping, CreateCustomObjectFieldsRequestMapping>();
        s.AddTransient<ICreateCustomObjectSchemaRequestMapping, CreateCustomObjectSchemaRequestMapping>();

        s.AddSingleton(sp =>
        {
            var storageConnectionString = configuration.GetValue<string>("AzureWebJobsStorage");
            return new BlobServiceClient(storageConnectionString);
        });

        s.AddTransient<IBlobContainerClientWrapper, BlobContainerClientWrapper>(x =>
            new BlobContainerClientWrapper(configuration.GetValue<string>("AzureWebJobsStorage")));

        s.AddTransient<IPersonMapper, PersonMapper>();

        s.AddMarketoClient(configuration);
        s.AddEmployerUsersClient(configuration);
        s.AddDatamartConfiguration(configuration);
        s.AddLogging();
        s.AddApplicationInsightsTelemetryWorkerService(options =>
        {
            options.ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        });

    })

    .Build();

host.Run();

