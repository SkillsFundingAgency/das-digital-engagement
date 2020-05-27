using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Models.Marketo
{
    public class ApiResult
    {
        public string RequestId { get; set; }
        public bool Success { get; set; }
    }

    public class ApiResult<T> : ApiResult where T : class
    {
    }
}
