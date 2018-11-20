using Microsoft.Azure.WebJobs.Host.Config;
using SFA.DAS.Campaign.Functions.Framework.Attributes;

namespace SFA.DAS.Campaign.Functions.Framework.Infrastructure
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