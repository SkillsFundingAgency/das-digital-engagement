using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.DataCollectionStub
{
    public class UnregisterDetails
    {
        private readonly ILogger<UnregisterDetails> _logger;

        public UnregisterDetails(
            ILogger<UnregisterDetails> logger)
        {
            _logger = logger;
        }

        [Function("UnregisterDetails")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "update-person")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger UnregisterDetails processed a request.");

            var requestBody = await req.ReadAsStringAsync();

            _logger.LogInformation($"Following data received: {requestBody}");

        }
    }
}