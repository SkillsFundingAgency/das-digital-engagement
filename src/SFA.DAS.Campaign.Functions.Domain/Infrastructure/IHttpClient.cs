using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Functions.Domain.Infrastructure
{
    public interface IHttpClient<in T>
    {
        string AuthKey { get; set; }
        Task<HttpResponseMessage> PostAsync(string url, T data);
    }
}
