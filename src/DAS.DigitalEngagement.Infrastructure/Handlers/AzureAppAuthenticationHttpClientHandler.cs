using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace DAS.DigitalEngagement.Infrastructure.Handlers
{
    public class AzureAppAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly string _identifier;
        private readonly ILogger<AzureAppAuthenticationHttpClientHandler> _logger;

        public AzureAppAuthenticationHttpClientHandler(string identifier, ILogger<AzureAppAuthenticationHttpClientHandler> logger, HttpMessageHandler innerHandler = null)
        {
            _identifier = identifier;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                try
                { 
                   var azureServiceTokenProvider = new AzureServiceTokenProvider();
                   var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_identifier);

                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, accessToken);
                }
                catch (Exception ex)
                {
                   _logger.LogError(ex, "Unable to authenticate application to API using azure app authentication");
                }

            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}