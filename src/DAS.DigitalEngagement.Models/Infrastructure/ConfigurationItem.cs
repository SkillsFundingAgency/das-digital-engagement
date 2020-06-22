using Microsoft.Azure.Cosmos.Table;

namespace DAS.DigitalEngagement.Models.Infrastructure
{
    public class ConfigurationItem : TableEntity
    {
        public string Data { get; set; }
    }
}