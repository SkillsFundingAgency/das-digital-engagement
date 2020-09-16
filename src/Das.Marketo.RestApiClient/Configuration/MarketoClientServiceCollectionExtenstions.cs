using System;
using System.Collections.Generic;
using System.Text;
using Das.Marketo.RestApiClient.Handlers;
using Das.Marketo.RestApiClient.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace Das.Marketo.RestApiClient.Configuration
{
   public static class MarketoClientServiceCollectionExtenstions
    {
        public static IServiceCollection AddMarketoClient(this IServiceCollection services, IConfiguration configuration)
        {

            var marketoConfig = configuration.GetSection("Marketo").Get<MarketoConfiguration>();
            services.Configure<MarketoConfiguration>(configuration.GetSection("Marketo").Bind);
            services.AddTransient<OAuthHttpClientHandler>();

            var httpBuilder = services.AddRefitClient<IMarketoBulkImportClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(marketoConfig.ApiBaseUrl + marketoConfig.ApiBulkImportPrefix));
            httpBuilder.AddHttpMessageHandler<OAuthHttpClientHandler>();
            
            var builder = services.AddRefitClient<IMarketoLeadClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(marketoConfig.ApiBaseUrl + marketoConfig.ApiRestPrefix));
            builder.AddHttpMessageHandler<OAuthHttpClientHandler>();

            var customObjectClientbuilder = services.AddRefitClient<IMarketoCustomObjectSchemaClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(marketoConfig.ApiBaseUrl + marketoConfig.ApiRestPrefix));
            customObjectClientbuilder.AddHttpMessageHandler<OAuthHttpClientHandler>();

            return services;
        }
    }
}
