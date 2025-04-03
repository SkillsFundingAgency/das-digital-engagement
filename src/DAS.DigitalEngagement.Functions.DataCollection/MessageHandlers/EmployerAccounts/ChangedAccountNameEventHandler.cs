using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Messages.Events;
using DAS.DigitalEngagement.Domain.DataCollection;

namespace DAS.DigitalEngagement.Functions.DataCollection.MessageHandlers.EmployerAccounts
{
    public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
    {
        private readonly ILogger<ChangedAccountNameEventHandler> _logger;
        private readonly IChangedAccountNameHandler _changedAccountNameHandler;

        public ChangedAccountNameEventHandler(
            ILogger<ChangedAccountNameEventHandler> logger,
            IChangedAccountNameHandler changedAccountNameHandler)
        {
            _logger = logger;
            _changedAccountNameHandler = changedAccountNameHandler;
        }

        public async Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            try
            {
                _logger.LogInformation("Process ChangedAccountNameEvent started");

                await _changedAccountNameHandler.Handle(message);

                _logger.LogInformation("Process ChangedAccountNameEvent completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ChangedAccountNameEvent. Message: {@Message}", ex.Message);
                throw;
            }
        }
    }
}