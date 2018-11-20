using System;
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
        private readonly IHttpClient<UserData> _httpClient;
        private readonly IOptions<Configuration> _configuration;

        public UserService(IHttpClient<UserData> httpClient, IOptions<Configuration> configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task RegisterUser(UserData user)
        {
            var baseAddress = _configuration.Value.ApiBaseUrl;
            var response = await _httpClient.PostAsync($"{baseAddress}/registerdetails", user);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Error registering user: {JsonConvert.SerializeObject(user)}");
        }

        public async Task UnregisterUser(UserData user)
        {
            var baseAddress = _configuration.Value.ApiBaseUrl;
            var response = await _httpClient.PostAsync($"{baseAddress}/UnRegisterDetails", user);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Error un-registering user: {JsonConvert.SerializeObject(user)}");
        }
    }
}
