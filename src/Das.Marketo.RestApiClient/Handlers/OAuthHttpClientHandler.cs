using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Das.Marketo.RestApiClient.Configuration;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Das.Marketo.RestApiClient.Handlers
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
