using System;
using System.Collections.Generic;
using System.Text;

namespace Das.Marketo.RestApiClient.Models
{
    public class CustomObjectResponse
    {
        public string State { get; set; }
        public CustomObjectStateSummary Approved { get; set; }
    }

    public class CustomObjectStateSummary
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ApiName { get; set; }
    }
}
