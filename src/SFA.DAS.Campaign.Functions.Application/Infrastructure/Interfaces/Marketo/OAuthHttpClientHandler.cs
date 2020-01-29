using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.Infrastructure.Interfaces.Marketo
{
    public class OAuthHttpClientHandler : DelegatingHandler
    {
        private readonly MarketoConfiguration _marketoConfiguration;

        public OAuthHttpClientHandler(IOptions<MarketoConfiguration> marketoConfiguration, HttpMessageHandler innerHandler = null)
        {
            this._marketoConfiguration = marketoConfiguration.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {

                var client = new HttpClient();

                var response = await client.RequestTokenAsync(new TokenRequest
                {
                    Address = $"{_marketoConfiguration.ApiIdentityBaseUrl}/oauth/token",
                    GrantType = "client_credentials",

                    ClientId = _marketoConfiguration.ApiClientId,
                    ClientSecret = _marketoConfiguration.ApiClientSecret
                });

                if (response.IsError)
                {
                    throw new Exception($"Unable to get a new access token for the Marketo API. Error description: {response.ErrorDescription}");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, response.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
