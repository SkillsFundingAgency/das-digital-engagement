using System.Threading.Tasks;
using DAS.DigitalEngagement.Models;
using Refit;

namespace DAS.DigitalEngagement.Infrastructure.Interfaces.Clients
{
    public interface IEmployerAccountsApiClient
    {
        [Get("/api/user/by-ref/{userRef}")]
        [Headers("Authorization: Bearer")]
        Task<EmployerAccountsUser> GetUserByRef([AliasAs("userRef")] string userRef);
    }
}
