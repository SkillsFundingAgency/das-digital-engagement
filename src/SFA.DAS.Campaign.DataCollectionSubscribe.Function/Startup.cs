using System;
using System.IO;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Handlers;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Services;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Validators;
using SFA.DAS.Campaign.Functions.Application.Infrastructure.Interfaces.Marketo;
using SFA.DAS.Campaign.Functions.Application.Services;
using SFA.DAS.Campaign.Functions.DataCollection;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Framework.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SFA.DAS.Campaign.Functions.DataCollection
{
    
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            
            builder.AddDependencyInjection<ServiceProviderBuilder>();
        }
    }

    internal class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly ILoggerFactory _loggerFactory;
        public IConfiguration Configuration { get; }
        public ServiceProviderBuilder(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            var tempConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true).Build();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = new[] { tempConfig.GetValue<string>("AppName") };
                    options.EnvironmentNameEnvironmentVariableName = tempConfig.GetValue<string>("EnvironmentName");
                    options.StorageConnectionStringEnvironmentVariableName = tempConfig.GetValue<string>("ConfigurationStorageConnectionString");
                    options.PreFixConfigurationKeys = false;
                })
                .AddJsonFile("local.settings.json",true).Build();
            Configuration = config;
        }

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();

            services.Configure<Models.Infrastructure.Configuration>(Configuration.GetSection("Values"));
            services.Configure<MarketoConfiguration>(Configuration.GetSection("Marketo"));
            services.AddOptions();

            // Important: We need to call CreateFunctionUserCategory, otherwise our log entries might be filtered out.
            services.AddSingleton<ILogger>(_ => _loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory("Common")));

            services.AddTransient<IRegisterHandler, RegisterHandler>();
            services.AddTransient<IUnregisterHandler, UnregisterHandler>();
            services.AddTransient<IUserDataValidator, UserDataValidator>();
            services.AddTransient<IUserUnregisterDataValidator, UserUnregisterDataValidator>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient(typeof(IHttpClient<>), typeof(HttpClient<>));
            services.AddTransient<IMarketoService, MarketoLeadService>();
            services.AddTransient<OAuthHttpClientHandler>();

            var marketoConfig = Configuration.GetSection("Marketo").Get<MarketoConfiguration>();

            var builder = services.AddRefitClient<IMarketoLeadClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(marketoConfig.ApiBaseUrl));

            builder.AddHttpMessageHandler<OAuthHttpClientHandler>();

        return services.BuildServiceProvider();
        }
    }
}