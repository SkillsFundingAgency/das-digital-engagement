using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Models.Infrastructure
{
    public class MarketoConfiguration
    {
        public virtual string ApiBaseUrl { get; set; }
        public virtual string ApiClientId { get; set; }
        public virtual string ApiClientSecret { get; set; }
        public virtual RegisterInterestProgramConfiguration RegisterInterestProgramConfiguration { get; set; }
        public virtual string ApiIdentityBaseUrl { get; set; }
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
