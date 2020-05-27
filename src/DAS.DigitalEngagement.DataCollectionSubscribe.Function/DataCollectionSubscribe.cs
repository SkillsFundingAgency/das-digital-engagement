using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Framework;
using DAS.DigitalEngagement.Framework.Attributes;
using DAS.DigitalEngagement.Models.DataCollection;

namespace DAS.DigitalEngagement.Functions.DataCollection
{
    public static class DataCollectionSubscribe
    {
        [FunctionName("DataCollectionSubscribe")]
        public static async Task Run([QueueTrigger(QueueNames.DataCollectionSubscribe)]string message, ILogger log, [Inject]IRegisterHandler handler)
        {
            try
            {
                var userData = JsonConvert.DeserializeObject<UserData>(message);

                await handler.Handle(userData);

                log.LogTrace($"C# Queue trigger function processed message: {message}");
            }
            catch (Exception e)
            {
                
                log.LogError(e, "Error in Function.  Message: {0}  Stack: {1}", e.Message, e.StackTrace);
                
                throw;
            }
        }
    }
}
