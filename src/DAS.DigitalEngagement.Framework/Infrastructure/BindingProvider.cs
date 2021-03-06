﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace DAS.DigitalEngagement.Framework.Infrastructure
{
    internal class InjectBindingProvider : IBindingProvider
    {
        private readonly ServiceProviderHolder _serviceProvider;

        public InjectBindingProvider(ServiceProviderHolder serviceProvider) =>
            _serviceProvider = serviceProvider;

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new InjectBinding(_serviceProvider, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }
    }
}
