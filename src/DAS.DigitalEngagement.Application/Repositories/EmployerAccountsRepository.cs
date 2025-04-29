using System.Threading.Tasks;
using DAS.DigitalEngagement.Infrastructure.Interfaces.Clients;
using DAS.DigitalEngagement.Models;

namespace DAS.DigitalEngagement.Application.Repositories
{
    public interface IEmployerAccountsRepository
    {
        Task<EmployerAccountsUser> GetUserByRef(string userRef);
    }

    public class EmployerAccountsRepository : IEmployerAccountsRepository
    {
        private readonly IEmployerAccountsApiClient _employerAccountsApiClient;

        public EmployerAccountsRepository(IEmployerAccountsApiClient employerAccountsApiClient)
        {
            _employerAccountsApiClient = employerAccountsApiClient;
        }

        public async Task<EmployerAccountsUser> GetUserByRef(string userRef)
        {
            var response = await _employerAccountsApiClient.GetUserByRef(userRef);

            return response == null ? null : new EmployerAccountsUser
            {
                Email = response.Email,
            };
        }
    }
}