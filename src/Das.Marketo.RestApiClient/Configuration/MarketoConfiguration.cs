namespace Das.Marketo.RestApiClient.Configuration
{
    public class MarketoConfiguration
    {
        public virtual string ApiBaseUrl { get; set; }
        public virtual string ApiClientId { get; set; }
        public virtual string ApiClientSecret { get; set; }
        public virtual RegisterInterestProgramConfiguration RegisterInterestProgramConfiguration { get; set; }
        public virtual string ApiIdentityBaseUrl { get; set; }

        public virtual string ApiRestPrefix => "/rest/v1";
        public virtual string ApiBulkImportPrefix => "/bulk/v1";
    }

    public class RegisterInterestProgramConfiguration
    {
        public virtual string ProgramName { get; set; }
        public virtual string Source { get; set; }
        public virtual string LookupField { get; set; }
        public virtual string CitizenReason { get; set; }
        public virtual string EmployerReason { get; set; }

    }
}
