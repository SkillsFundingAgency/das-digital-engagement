using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.DataCollection;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClient<Person> _httpClient;
        private readonly IOptions<Configuration> _configuration;

        public UserService(IHttpClient<Person> httpClient, IOptions<Configuration> configuration)
        {
            _httpClient = httpClient;
            _httpClient.XFunctionsKey = configuration.Value.ApiXFunctionsKey;
            _configuration = configuration;
        }

        public async Task RegisterUser(UserData user)
        {
            var baseAddress = _configuration.Value.ApiBaseUrl;

            var person = new Person().MapFromUserData(user);

            var response = await _httpClient.PostAsync($"{baseAddress}/create-person", person);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                await UnregisterUser(user);
                return;
            }

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Error registering user: {JsonConvert.SerializeObject(person)}");
        }

        public async Task UnregisterUser(UserData user)
        {
            var baseAddress = _configuration.Value.ApiBaseUrl;
            var person = new Person().MapFromUserData(user);
            var response = await _httpClient.PostAsync($"{baseAddress}/update-person", person);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Error un-registering user: {JsonConvert.SerializeObject(user)}");
        }
    }
}
