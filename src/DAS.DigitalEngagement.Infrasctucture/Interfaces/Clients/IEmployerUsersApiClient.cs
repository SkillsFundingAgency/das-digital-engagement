using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Infrastructure.Models;
using Refit;

namespace DAS.DigitalEngagement.Infrastructure.Interfaces.Clients
{
  
    public interface IEmployerUsersApiClient
    {
        [Get("/api/users?pageNumber={pageNumber}&pageSize={pageSize}")]
        [Headers("Authorization: Bearer")]
        Task<EmployerUserResponse<EmployerUser>> GetUsers([AliasAs("pageNumber")] int pageNumber = 1, int pageSize = 1000);

    }
}
