using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Campaign.Functions.DataCollectionStub
{
    public static class UnregisterDetails
    {
        [FunctionName("UnregisterDetails")]
        public static async Task Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "update-person")]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger UnregisterDetails processed a request.");

            var requestBody = await req.Content.ReadAsStringAsync();

            log.LogInformation($"Following data received: {requestBody}");

        }
    }
}