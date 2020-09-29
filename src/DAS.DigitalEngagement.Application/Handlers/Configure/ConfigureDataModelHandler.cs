using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Import.Handlers;
using DAS.DigitalEngagement.Domain.Configure;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.DataModel;
using DAS.DigitalEngagement.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Application.Handlers.Configure
{
    public class ConfigureDataModelHandler : IConfigureDataModelHandler
    {
        private readonly IDataModelConfigurationService _dataModelConfigurationService;
        private readonly ILogger<ConfigureDataModelHandler> _logger;
        private readonly IGithubRepository _githubRepository;


        public ConfigureDataModelHandler(ILogger<ConfigureDataModelHandler> logger, IDataModelConfigurationService dataModelConfigurationService, IGithubRepository githubRepository)
        {
            _logger = logger;
            _dataModelConfigurationService = dataModelConfigurationService;
            _githubRepository = githubRepository;
        }

        public async Task ConfigureDataModel(IList<DataMartSettings> dataMartSettings)
        {
            foreach (var dataMartSetting in dataMartSettings)
            {
                if (dataMartSetting.ObjectName != "Lead")
                {
                    var config = await _githubRepository.GetFile(dataMartSetting.ConfigFileLocation);
                    var tableConfig = JsonConvert.DeserializeObject<Table>(config);

                    await _dataModelConfigurationService.ConfigureTable(tableConfig);
                }
            }
        }
    }
}
