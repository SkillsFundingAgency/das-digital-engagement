using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Mapping;
using DAS.DigitalEngagement.Application.Mapping.Interfaces;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.DataModel;
using Das.Marketo.RestApiClient.Interfaces;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Services.Marketo
{
    public class MarketoDataModelConfigurationService : IDataModelConfigurationService
    {

        private readonly ILogger<MarketoDataModelConfigurationService> _logger;
        private readonly IMarketoCustomObjectSchemaClient _customObjectSchemaClient;
        private readonly ICreateCustomObjectSchemaRequestMapping _customObjectSchemaRequestMapping;
        private readonly ICreateCustomObjectFieldsRequestMapping _customObjectFieldsRequestMapping;

        public MarketoDataModelConfigurationService(ILogger<MarketoDataModelConfigurationService> logger, IMarketoCustomObjectSchemaClient customObjectSchemaClient, ICreateCustomObjectSchemaRequestMapping customObjectSchemaRequestMapping, ICreateCustomObjectFieldsRequestMapping customObjectFieldsRequestMapping)
        {
            _logger = logger;
            _customObjectSchemaClient = customObjectSchemaClient;
            _customObjectSchemaRequestMapping = customObjectSchemaRequestMapping;
            _customObjectFieldsRequestMapping = customObjectFieldsRequestMapping;
        }


        public async Task ConfigureTable(Table tableDefinition)
        {

            //Check if object already exists
            var customObject = await _customObjectSchemaClient.GetCustomObjectSchema(tableDefinition.apiName);

            //if doesnt exist
            if (String.IsNullOrWhiteSpace(customObject.ApiName))
            {
                //Create custom object

                if (await CreateCustomObject(tableDefinition) == false) return;

                //Create custom object fields
                if (await CreateCustomObjectFields(tableDefinition, customObject) == false) return;

                //approve object
                await ApproveCustomObject(tableDefinition);
            }
            else
            {
                _logger.LogWarning("Custom object already exists so cannot create");

            }

        }

        private async Task<bool> ApproveCustomObject(Table tableDefinition)
        {
            var customObjectSchemaResponse = await _customObjectSchemaClient.ApproveCustomObjectSchema(tableDefinition.apiName);

            if (customObjectSchemaResponse.Success == false)
            {
                _logger.LogError($"Unable to create custom object, Response: {customObjectSchemaResponse}");
                return false;
            }
            return true;
        }

        private async Task<bool> CreateCustomObjectFields(Table tableDefinition, CustomObject customObject)
        {
            var customObjectSchemaResponse =
                await _customObjectSchemaClient.PushCustomObjectFields(tableDefinition.apiName,
                    _customObjectFieldsRequestMapping.Map(tableDefinition.fields.ToList()));

            if (customObjectSchemaResponse.Success == false)
            {
                _logger.LogError($"Unable to create custom object fields, Response: {customObjectSchemaResponse.ToString()}");
                return false;
            }

            return true;
        }

        private async Task<bool> CreateCustomObject(Table tableDefinition)
        {
            var customObjectSchemaResponse = await _customObjectSchemaClient.PushCustomObjectSchema(_customObjectSchemaRequestMapping.Map(tableDefinition));

            if (customObjectSchemaResponse.Success == false)
            {
                _logger.LogError($"Unable to create custom object, Response: {customObjectSchemaResponse.ToString()}");
                return false;
            }

            return true;
        }
    }
}
