using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.Services
{
    public class HttpClient<T> : IHttpClient<T>
    {
        public async Task<HttpResponseMessage> PostAsync(string url, T data)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(data)));
        }
    }
}
