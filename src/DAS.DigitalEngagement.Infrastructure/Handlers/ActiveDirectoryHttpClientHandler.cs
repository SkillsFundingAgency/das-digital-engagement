using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace DAS.DigitalEngagement.Infrastructure.Handlers
{
    public class ActiveDirectoryHttpClientHandler : DelegatingHandler
    {
        private readonly IConfidentialClientApplication _app;
        private readonly string _resource;

        public ActiveDirectoryHttpClientHandler(IConfidentialClientApplication app, string resource,
            HttpMessageHandler innerHandler = null)
        {
            _app = app;
            _resource = resource;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                string[] scopes = new string[] {$"{_resource}/.default"};

                AuthenticationResult result = null;
                try
                {
                    result = await _app.AcquireTokenForClient(scopes)
                        .ExecuteAsync();

                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, result.AccessToken);
                }
                catch (MsalServiceException ex)
                {
                    // Case when ex.Message contains:
                    // AADSTS70011 Invalid scope. The scope has to be of the form "https://resourceUrl/.default"
                    // Mitigation: change the scope to be as expected
                }

            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}