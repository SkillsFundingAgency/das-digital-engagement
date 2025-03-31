using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace DAS.DigitalEngagement.Functions.DataCollection.MessageHandlers.EmployerAccounts
{
    public class UpsertedUserEventHandler : IHandleMessages<UpsertedUserEvent>
    {
        private readonly ILogger<UpsertedUserEventHandler> _logger;

        public UpsertedUserEventHandler(
            ILogger<UpsertedUserEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(UpsertedUserEvent message, IMessageHandlerContext context)
        {
            try
            {
                _logger.LogInformation("Processed UpsertedUserEvent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling UpsertedUserEvent. Message: {@Message}", message);
                throw;
            }
        }
    }
}