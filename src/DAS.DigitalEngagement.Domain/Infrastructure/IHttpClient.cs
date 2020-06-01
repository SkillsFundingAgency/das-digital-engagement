using System.Net.Http;
using System.Threading.Tasks;

namespace DAS.DigitalEngagement.Domain.Infrastructure
{
    public interface IHttpClient<in T>
    {
        string AuthKey { get; set; }
        string XFunctionsKey { get; set; }
        Task<HttpResponseMessage> PostAsync(string url, T data);
    }
}
