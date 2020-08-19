using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Infrastructure.Interfaces.Clients;
using DAS.DigitalEngagement.Infrastructure.Models;

namespace DAS.DigitalEngagement.Application.Repositories
{
    public interface IEmployerUsersRepository
    {
        Task<IList<EmployerUser>> GetAllUsers();
    }

    public class EmployerUsersRepository : IEmployerUsersRepository
    {
        private readonly IEmployerUsersApiClient _employerUsersApiClient;

        public EmployerUsersRepository(IEmployerUsersApiClient employerUsersApiClient)
        {
            _employerUsersApiClient = employerUsersApiClient;
        }

        public async Task<IList<EmployerUser>> GetAllUsers()
        {

            var users = await _employerUsersApiClient.GetUsers(1, 9999999);

            return users.Data;

        }


    }
}
