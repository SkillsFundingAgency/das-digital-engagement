using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Functions.Domain.Infrastructure
{
    public interface IHttpClient<in T>
    {
        Task<HttpResponseMessage> PostAsync(string url, T data);
    }
}
