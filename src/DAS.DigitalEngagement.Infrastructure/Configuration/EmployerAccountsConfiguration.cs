namespace DAS.DigitalEngagement.Infrastructure.Configuration
{
    public class EmployerAccountsConfiguration
    {
        public string Tenant { get; set; }
        public string Identifier { get; set; }
        public virtual string ApiBaseUrl { get; set; }
    }
}
