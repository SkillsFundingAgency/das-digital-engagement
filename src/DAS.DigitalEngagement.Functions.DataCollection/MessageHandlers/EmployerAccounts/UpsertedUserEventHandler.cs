using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Messages.Events;
using DAS.DigitalEngagement.Domain.DataCollection;

namespace DAS.DigitalEngagement.Functions.DataCollection.MessageHandlers.EmployerAccounts
{
    public class UpsertedUserEventHandler : IHandleMessages<UpsertedUserEvent>
    {
        private readonly ILogger<UpsertedUserEventHandler> _logger;
        private readonly IUpsertedUserHandler _upsertedUserHandler;

        public UpsertedUserEventHandler(
            ILogger<UpsertedUserEventHandler> logger,
            IUpsertedUserHandler upsertedUserHandler)
        {
            _logger = logger;
            _upsertedUserHandler = upsertedUserHandler;
        }

        public async Task Handle(UpsertedUserEvent message, IMessageHandlerContext context)
        {
            try
            {
                _logger.LogInformation("Process UpsertedUserEvent started");

                await _upsertedUserHandler.Handle(message);

                _logger.LogInformation("Process UpsertedUserEvent completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling UpsertedUserEvent");
                throw;
            }
        }
    }
}