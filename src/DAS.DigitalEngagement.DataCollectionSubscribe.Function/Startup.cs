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
using DAS.DigitalEngagement.Application.DataCollection.Handlers;
using DAS.DigitalEngagement.Application.DataCollection.Services;
using DAS.DigitalEngagement.Application.DataCollection.Validators;
using DAS.DigitalEngagement.Application.Infrastructure.Interfaces.Marketo;
using DAS.DigitalEngagement.Application.Services;
using DAS.DigitalEngagement.Functions.DataCollection;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Infrastructure;
using DAS.DigitalEngagement.Framework.Infrastructure;
using DAS.DigitalEngagement.Models.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;

[assembly: WebJobsStartup(typeof(Startup))]
namespace DAS.DigitalEngagement.Functions.DataCollection
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
                    options.ConfigurationKeys = new[] { tempConfig.GetValue<string>("configNames") };
                    options.EnvironmentNameEnvironmentVariableName = "EnvironmentName";
                    options.StorageConnectionStringEnvironmentVariableName = "ConfigurationStorageConnectionString";
                    options.PreFixConfigurationKeys = false;
                })
                .AddJsonFile("local.settings.json",true).Build();
            Configuration = config;
        }

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();

            services.Configure<global::DAS.DigitalEngagement.Models.Infrastructure.Configuration>(Configuration.GetSection("Values"));
            services.Configure<MarketoConfiguration>(Configuration.GetSection("Marketo"));
            services.AddOptions();

            // Important: We need to call CreateFunctionUserCategory, otherwise our log entries might be filtered out.
            services.AddSingleton<ILogger>(_ => _loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory("Common")));

            services.AddTransient<IRegisterHandler, RegisterHandler>();
            services.AddTransient<IUserDataValidator, UserDataValidator>();
            services.AddTransient(typeof(IHttpClient<>), typeof(HttpClient<>));
            services.AddTransient<IMarketoService, MarketoLeadService>();
            services.AddTransient<OAuthHttpClientHandler>();

            var marketoConfig = Configuration.GetSection("Marketo").Get<MarketoConfiguration>();

            var builder = services.AddRefitClient<IMarketoLeadClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(marketoConfig.ApiBaseUrl + marketoConfig.ApiRestPrefix));

            builder.AddHttpMessageHandler<OAuthHttpClientHandler>();

        return services.BuildServiceProvider();
        }
    }
}