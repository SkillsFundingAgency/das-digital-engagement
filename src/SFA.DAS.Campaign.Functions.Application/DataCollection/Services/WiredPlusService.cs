using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Campaign.Functions.Domain.DataCollection;
using SFA.DAS.Campaign.Functions.Domain.Infrastructure;
using SFA.DAS.Campaign.Functions.Models.DataCollection;
using SFA.DAS.Campaign.Functions.Models.Infrastructure;

namespace SFA.DAS.Campaign.Functions.Application.DataCollection.Services
{
    public class WiredPlusService : IWiredPlusService
    {
        private readonly IHttpClient<Dictionary<string, string>> _httpClient;
        private readonly IOptions<Configuration> _configuration;

        public WiredPlusService(IHttpClient<Dictionary<string, string>> httpClient, IOptions<Configuration> configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(_configuration.Value.WiredPlusAuthKey));
        }

        public async Task CreateUser(UserData user)
        {
            var data = UserDataToDictionary(user);

            var response = await _httpClient.PostAsync($"{_configuration.Value.WiredPlusBaseUrl}/v1/CreateContact", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Error creating user in wiredplus: {JsonConvert.SerializeObject(user)}");
            }
        }

        public async Task UnsubscribeUser(UserData user)
        {
            var data = UserDataToDictionary(user);

            var response = await _httpClient.PostAsync($"{_configuration.Value.WiredPlusBaseUrl}/v1/UnsubscribeContact", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Error Unsubscribing Contact in wiredplus: {JsonConvert.SerializeObject(user)}");
            }
        }
        public async Task SubscribeUser(UserData user)
        {
            var data = UserDataToDictionary(user);

            var response = await _httpClient.PostAsync($"{_configuration.Value.WiredPlusBaseUrl}/v1/ResubscribeContact", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Error subscribing Contact in wiredplus: {JsonConvert.SerializeObject(user)}");
            }
        }

        private static Dictionary<string, string> UserDataToDictionary(UserData user)
        {
            return user.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(key => key.CustomAttributes.ToList().Any())
                .ToDictionary(key => 
                        key.CustomAttributes.ToList()[0].ConstructorArguments[0].Value.ToString(),
                    value => value.GetValue(user) == null ? "" : value.GetValue(user).ToString());
        }

        public async Task<bool> UserExists(string email)
        {
            var user = new UserData{Email = email};
            var data = UserDataToDictionary(user);

            var response = await _httpClient.PostAsync($"{_configuration.Value.WiredPlusBaseUrl}/v1/GetContactByEmail", data);
            var userResponse = await response.Content.ReadAsStringAsync();
            var userRecord = JsonConvert.DeserializeObject<UserData>(userResponse);
            return !string.IsNullOrEmpty(userRecord.Email);
        }
    }
}
