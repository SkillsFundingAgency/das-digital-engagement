using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.Services
{
    public class HttpClient<T> : IHttpClient<T>
    {
        public string AuthKey { get; set; }

        public async Task<HttpResponseMessage> PostAsync(string url, T data)
        {
            string mediaType;
            HttpContent content;

            if (typeof(T).IsAssignableFrom(typeof(Dictionary<string, string>)))
            {
                mediaType = "multipart/form-data";
                content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)data);
            }
            else
            {
                mediaType = "application/json";
                content = new StringContent(JsonConvert.SerializeObject(data));
            }
               
            var client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue(mediaType));

            if (!string.IsNullOrWhiteSpace(AuthKey))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthKey);
            }

            return await client.PostAsync(url, content);
        }

    }
}