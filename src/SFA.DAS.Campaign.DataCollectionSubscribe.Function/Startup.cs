using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Handlers;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Services;
using SFA.DAS.Campaign.Functions.Application.DataCollection.Validators;
using SFA.DAS.Campaign.Functions.Application.Services;
using SFA.DAS.Campaign.Functions.DataCollectionSubscribe;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Framework.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SFA.DAS.Campaign.Functions.DataCollectionSubscribe
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
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json").Build();
            Configuration = config;
        }

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();

            services.Configure<Configuration>(Configuration.GetSection("Values"));
            // Important: We need to call CreateFunctionUserCategory, otherwise our log entries might be filtered out.
            services.AddSingleton<ILogger>(_ => _loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory("Common")));

            services.AddTransient<IRegisterHandler, RegisterHandler>();
            services.AddTransient<IUserDataValidator, UserDataValidator>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient(typeof(IHttpClient<>), typeof(HttpClient<>));

        return services.BuildServiceProvider();
        }
    }
}

