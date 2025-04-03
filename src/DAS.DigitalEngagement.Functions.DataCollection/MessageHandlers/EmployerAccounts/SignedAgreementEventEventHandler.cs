using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Messages.Events;
using DAS.DigitalEngagement.Domain.DataCollection;

namespace DAS.DigitalEngagement.Functions.DataCollection.MessageHandlers.EmployerAccounts
{
    public class SignedAgreementEventHandler : IHandleMessages<SignedAgreementEvent>
    {
        private readonly ILogger<SignedAgreementEventHandler> _logger;
        private readonly ISignedAgreementHandler _upsertedUserHandler;

        public SignedAgreementEventHandler(
            ILogger<SignedAgreementEventHandler> logger,
            ISignedAgreementHandler addedPayeSchemeHandler)
        {
            _logger = logger;
            _upsertedUserHandler = addedPayeSchemeHandler;
        }

        public async Task Handle(SignedAgreementEvent message, IMessageHandlerContext context)
        {
            try
            {
                _logger.LogInformation("Process SignedAgreementEvent started");

                await _upsertedUserHandler.Handle(message);

                _logger.LogInformation("Process SignedAgreementEvent completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling SignedAgreementEvent. Message: {@Message}", ex.Message);
                throw;
            }
        }
    }
}