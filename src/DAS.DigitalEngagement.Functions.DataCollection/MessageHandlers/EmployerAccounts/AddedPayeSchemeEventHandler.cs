using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Messages.Events;
using DAS.DigitalEngagement.Domain.DataCollection;

namespace DAS.DigitalEngagement.Functions.DataCollection.MessageHandlers.EmployerAccounts
{
    public class AddedPayeSchemeEventHandler : IHandleMessages<AddedPayeSchemeEvent>
    {
        private readonly ILogger<AddedPayeSchemeEventHandler> _logger;
        private readonly IAddedPayeSchemeHandler _addedPayeSchemeHandler;

        public AddedPayeSchemeEventHandler(
            ILogger<AddedPayeSchemeEventHandler> logger,
            IAddedPayeSchemeHandler addedPayeSchemeHandler)
        {
            _logger = logger;
            _addedPayeSchemeHandler = addedPayeSchemeHandler;
        }

        public async Task Handle(AddedPayeSchemeEvent message, IMessageHandlerContext context)
        {
            try
            {
                _logger.LogInformation("Process AddedPayeSchemeEvent started");

                await _addedPayeSchemeHandler.Handle(message);

                _logger.LogInformation("Process AddedPayeSchemeEvent completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling AddedPayeSchemeEvent. Message: {@Message1}", ex.Message);
                throw;
            }
        }
    }
}