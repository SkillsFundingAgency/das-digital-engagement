using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DAS.DigitalEngagement.Application.DataCollection.Handlers;
using DAS.DigitalEngagement.Application.DataCollection.Services;
using DAS.DigitalEngagement.Application.DataCollection.Validators;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Infrastructure;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.Extensions.Hosting;
using DAS.DigitalEngagement.Functions.DataCollection.Extensions;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Infrastructure.Configuration;

[assembly: NServiceBusTriggerFunction("SFA.DAS.DigitalEngagement.Functions.DataCollection")]
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostBuilderContext, builder) => { builder.BuildDasConfiguration(); })
    .ConfigureNServiceBus()
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

        s.Configure<DAS.DigitalEngagement.Models.Infrastructure.Configuration>(configuration.GetSection("Values"));

        s.AddTransient<IRegisterHandler, RegisterHandler>();
        s.AddTransient<IUpsertedUserHandler, UpsertedUserHandler>();
        s.AddTransient<IAddedPayeSchemeHandler, AddedPayeSchemeHandler>();
        s.AddTransient<ICreatedAccountTaskListCompleteHandler, CreatedAccountTaskListCompleteHandler>();
        s.AddTransient<ICreatedAccountTaskListCompleteValidator, CreatedAccountTaskListCompleteValidator>();
        s.AddTransient<IChangedAccountNameHandler, ChangedAccountNameHandler>();
        s.AddTransient<ISignedAgreementHandler, SignedAgreementHandler>();
        s.AddTransient<IUserDataValidator, UserDataValidator>();
        s.AddTransient<IUpsertedUserValidator, UpsertedUserValidator>();
        s.AddTransient<IAddedPayeSchemeValidator, AddedPayeSchemeValidator>();
        s.AddTransient<IChangedAccountNameValidator, ChangedAccountNameValidator>();
        s.AddTransient<ISignedAgreementValidator, SignedAgreementValidator>();
        s.AddTransient<IEmployerAccountsRepository, EmployerAccountsRepository>();
        s.AddTransient(typeof(IHttpClient<>), typeof(HttpClient<>));
        s.AddTransient<IMarketoService, MarketoLeadService>();

        s.AddMarketoClient(configuration);
        s.AddEmployerAccountsClient(configuration);

        s.AddApplicationInsightsTelemetryWorkerService(options =>
        {
            options.ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        });

    })

    .Build();

host.Run();

