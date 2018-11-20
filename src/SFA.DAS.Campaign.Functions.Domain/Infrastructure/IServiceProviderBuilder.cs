using System;

namespace SFA.DAS.Campaign.Functions.Domain.Infrastructure
{
    public interface IServiceProviderBuilder
    {
        IServiceProvider Build();
    }
}
