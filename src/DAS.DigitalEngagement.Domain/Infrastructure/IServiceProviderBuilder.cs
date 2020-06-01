using System;

namespace DAS.DigitalEngagement.Domain.Infrastructure
{
    public interface IServiceProviderBuilder
    {
        IServiceProvider Build();
    }
}
