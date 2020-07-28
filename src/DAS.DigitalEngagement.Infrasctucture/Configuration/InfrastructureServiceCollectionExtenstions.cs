using System;
using DAS.DigitalEngagement.Infrastructure.Handlers;
using DAS.DigitalEngagement.Infrastructure.Interfaces.Clients;
using Das.Marketo.RestApiClient.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Refit;

namespace DAS.DigitalEngagement.Infrastructure.Configuration
{
    public static class InfrastructureServiceCollectionExtenstions
    {
        private static ConfidentialClientApplicationOptions _applicationOptions;
        public static IServiceCollection AddEmployerUsersClient(this IServiceCollection services, IConfiguration configuration)
        {

            var employerUsersConfig = configuration.GetSection("EmployerUsersApi").Get<EmployerUsersConfiguration>();
            services.Configure<EmployerUsersConfiguration>(configuration.GetSection("EmployerUsersApi").Bind);


            _applicationOptions = new ConfidentialClientApplicationOptions();
            configuration.Bind("EmployerUsersApi", _applicationOptions);

            services.AddSingleton<IConfidentialClientApplication, ConfidentialClientApplication>(x =>
            {
               var  app  = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(_applicationOptions)
                    .Build();
               return app as ConfidentialClientApplication;
            });
 

            services.AddTransient<AzureAppAuthenticationHttpClientHandler>(x => new AzureAppAuthenticationHttpClientHandler(employerUsersConfig.Identifier, x.GetRequiredService<ILogger<AzureAppAuthenticationHttpClientHandler>>()));

            var httpBuilder = services.AddRefitClient<IEmployerUsersApiClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(employerUsersConfig.ApiBaseUrl));
            httpBuilder.AddHttpMessageHandler<AzureAppAuthenticationHttpClientHandler>();



            return services;
        }
    }
}
