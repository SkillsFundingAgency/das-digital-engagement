using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Campaign.Functions.Models.Infrastructure
{
    public class Configuration
    {
        public virtual string ApiBaseUrl { get; set; }
        public virtual string WiredPlusBaseUrl { get; set; }
        public virtual string WiredPlusAuthKey { get; set; }
        public virtual string ApiXFunctionsKey { get; set; }
    }
}
