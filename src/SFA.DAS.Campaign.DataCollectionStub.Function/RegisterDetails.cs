using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Campaign.Functions.DataCollectionStub
{
    public static class RegisterDetails
    {
        [FunctionName("RegisterDetails")]
        public static async Task Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "create-person")]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger RegisterDetails processed a request.");
            
            var requestBody = await req.Content.ReadAsStringAsync();

            log.LogInformation($"Following data received: {requestBody}");

        }
    }
}
