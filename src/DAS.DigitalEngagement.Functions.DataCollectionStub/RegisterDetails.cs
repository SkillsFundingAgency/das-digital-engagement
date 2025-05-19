using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Functions.DataCollectionStub
{
    public class RegisterDetails
    {
        private readonly ILogger<RegisterDetails> _logger;

        public RegisterDetails(
            ILogger<RegisterDetails> logger)
        {
            _logger = logger;
        }

        [Function("RegisterDetails")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create-person")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger RegisterDetails processed a request.");

            var requestBody = await req.ReadAsStringAsync();

            _logger.LogInformation($"Following data received: {requestBody}");

        }
    }
}