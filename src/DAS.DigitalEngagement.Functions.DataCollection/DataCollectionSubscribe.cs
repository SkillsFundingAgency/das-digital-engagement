using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Models.DataCollection;
using Microsoft.Azure.Functions.Worker;

namespace DAS.DigitalEngagement.Functions.DataCollection
{
    public class DataCollectionSubscribe
    {
        private readonly IRegisterHandler _registerHandler;
        private readonly ILogger<DataCollectionSubscribe> _logger;

        public DataCollectionSubscribe(
            IRegisterHandler registerHandler,
            ILogger<DataCollectionSubscribe> logger)
        {
            _registerHandler = registerHandler;
            _logger = logger;
        }

        [Function("DataCollectionSubscribe")]
        public async Task Run([QueueTrigger(QueueNames.DataCollectionSubscribe)] string message)
        {
            try
            {
                var userData = JsonConvert.DeserializeObject<UserData>(message);

                await _registerHandler.Handle(userData);

                _logger.LogTrace($"C# Queue trigger function processed message: {message}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Function.  Message: {0}  Stack: {1}", e.Message, e.StackTrace);

                throw;
            }
        }
    }
}
