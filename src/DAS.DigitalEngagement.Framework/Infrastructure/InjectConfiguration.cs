using DAS.DigitalEngagement.Framework.Attributes;
using Microsoft.Azure.WebJobs.Host.Config;

namespace DAS.DigitalEngagement.Framework.Infrastructure
{
    internal class InjectConfiguration : IExtensionConfigProvider
    {
        public readonly InjectBindingProvider InjectBindingProvider;

        public InjectConfiguration(InjectBindingProvider injectBindingProvider) =>
            InjectBindingProvider = injectBindingProvider;

        public void Initialize(ExtensionConfigContext context) => context
            .AddBindingRule<InjectAttribute>()
            .Bind(InjectBindingProvider);
    }
}