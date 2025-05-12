using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Messages.Events;
using DAS.DigitalEngagement.Domain.DataCollection;

namespace DAS.DigitalEngagement.Functions.DataCollection.MessageHandlers.EmployerAccounts
{
    public class CreatedAccountTaskListCompleteEventHandler : IHandleMessages<CreatedAccountTaskListCompleteEvent>
    {
        private readonly ILogger<CreatedAccountTaskListCompleteEventHandler> _logger;
        private readonly ICreatedAccountTaskListCompleteEventHandler _createdAccountTaskListCompleteEventHandler;

        public CreatedAccountTaskListCompleteEventHandler(
            ILogger<CreatedAccountTaskListCompleteEventHandler> logger,
            ICreatedAccountTaskListCompleteEventHandler createdAccountTaskListCompleteEventHandler)
        {
            _logger = logger;
            _createdAccountTaskListCompleteEventHandler = createdAccountTaskListCompleteEventHandler;
        }

        public async Task Handle(CreatedAccountTaskListCompleteEvent message, IMessageHandlerContext context)
        {
            try
            {
                _logger.LogInformation("Process CreatedAccountTaskListCompleteEvent started");

                //await _upsertedUserHandler.Handle(message);

                _logger.LogInformation("Process CreatedAccountTaskListCompleteEvent completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CreatedAccountTaskListCompleteEvent");
                throw;
            }
        }
    }
}