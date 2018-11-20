using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Functions.Framework;

namespace SFA.DAS.Campaign.Functions.DataCollectionUnsubscribe
{
    public static class DataCollectionUnsubscribe
    {
        [FunctionName("DataCollectionUnsubscribe")]
        public static void Run([QueueTrigger(QueueNames.DataCollectionUnsubscribe)]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
